using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoko.Models.Enums;
using Shoko.Server.API.Annotations;
using Shoko.Server.API.v3.Helpers;
using Shoko.Server.API.v3.Models.Shoko;
using Shoko.Server.Models;
using Shoko.Server.Repositories;

namespace Shoko.Server.API.v3.Controllers
{
    /// <summary>
    /// This Controller is intended to provide the tree. An example would be "api/v3/filter/4/group/12/series".
    /// This is to support filtering with Apply At Series Level and any other situations that might involve the need for it.
    /// </summary>
    [ApiController, Route("/api/v{version:apiVersion}"), ApiV3]
    [Authorize]
    public class TreeController : BaseController
    {
        #region Filter

        /// <summary>
        /// Get a list of all the sub-<see cref="Filter"/> for the <see cref="Filter"/> with the given <paramref name="filterID"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="Filter"/> must have <see cref="Filter.Directory"/> set to true to use
        /// this endpoint.
        /// </remarks>
        /// <param name="filterID"><see cref="Filter"/> ID</param>
        /// <param name="page">The page index.</param>
        /// <param name="pageSize">The page size. Set to <code>0</code> to disable pagination.</param>
        /// <returns></returns>
        [HttpGet("Filter/{filterID}/Filter")]
        public ActionResult<List<Filter>> GetSubFilters(int filterID, [FromQuery] int page = 0, [FromQuery] int pageSize = 50)
        {
            var groupFilter = RepoFactory.GroupFilter.GetByID(filterID);
            if (groupFilter == null)
                return NotFound(FilterController.FilterNotFound);

            if (!((GroupFilterType)groupFilter.FilterType).HasFlag(GroupFilterType.Directory))
                return BadRequest("Filter contains no sub-filters.");

            IEnumerable<SVR_GroupFilter> subGroupFilters = RepoFactory.GroupFilter.GetByParentID(filterID)
                .OrderBy(OrderByName);

            if (pageSize > 0)
                subGroupFilters = subGroupFilters
                    .Skip(page > 0 ? page * pageSize : 0)
                    .Take(pageSize);

            return subGroupFilters
                .Select(f => new Filter(HttpContext, f))
                .ToList();
        }

        /// <summary>
        /// Get a paginated list of all the top-level <see cref="Group"/>s for the <see cref="Filter"/> with the given <paramref name="filterID"/>.
        /// </summary>
        /// <param name="filterID"><see cref="Filter"/> ID</param>
        /// <param name="page">The page index.</param>
        /// <param name="pageSize">The page size. Set to <code>0</code> to disable pagination.</param>
        /// <returns></returns>
        [HttpGet("Filter/{filterID}/Group")]
        public ActionResult<List<Group>> GetFilteredGroups([FromRoute] int filterID, [FromQuery] int page = 0, [FromQuery] int pageSize = 0)
        {
            // Return the top level groups with no filter.
            IEnumerable<SVR_AnimeGroup> groups;
            if (filterID == 0)
            {
                groups = RepoFactory.AnimeGroup.GetAll()
                    .Where(group => group.AnimeGroupParentID.HasValue && User.AllowedGroup(group))
                    .OrderBy(OrderByName);
            }
            else
            {
                var groupFilter = RepoFactory.GroupFilter.GetByID(filterID);
                if (groupFilter == null)
                    return NotFound(FilterController.FilterNotFound);

                // Fast path when user is not in the filter
                if (!groupFilter.GroupsIds.TryGetValue(User.JMMUserID, out var groupIds))
                    return new List<Group>();

                groups = groupIds
                    .Select(group => RepoFactory.AnimeGroup.GetByID(group))
                    .Where(group => group != null)
                    .OrderByGroupFilter(groupFilter);
            }

            if (pageSize > 0)
                groups = groups
                    .Skip(page > 0 ? page * pageSize : 0)
                    .Take(pageSize);

            return groups
                .Select(group => new Group(HttpContext, group))
                .ToList();
        }

        /// <summary>
        /// Get a list of all the sub-<see cref="Group"/>s belonging to the <see cref="Group"/> with the given <paramref name="groupID"/> and which are present within the <see cref="Filter"/> with the given <paramref name="filterID"/>.
        /// </summary>
        /// <param name="filterID"><see cref="Filter"/> ID</param>
        /// <param name="groupID"><see cref="Group"/> ID</param>
        /// <param name="randomImages">Randomise images shown for the <see cref="Group"/>.</param>
        /// <returns></returns>
        [HttpGet("Filter/{filterID}/Group/{groupID}/Group")]
        public ActionResult<List<Group>> GetFilteredSubGroups([FromRoute] int filterID, [FromRoute] int groupID, [FromQuery] bool randomImages = false)
        {
            // Return sub-groups with no group filter applied.
            if (filterID == 0)
                return GetSubGroups(groupID);

            var groupFilter = RepoFactory.GroupFilter.GetByID(filterID);
            if (groupFilter == null)
                return NotFound(FilterController.FilterNotFound);

            // Check if the group exists.
            var group = RepoFactory.AnimeGroup.GetByID(groupID);
            if (group == null)
                return NotFound(GroupController.GroupNotFound);

            // Just return early because the every gropup will be filtered out.
            if (!groupFilter.SeriesIds.TryGetValue(User.JMMUserID, out var seriesIDs))
                return new List<Group>();

            return group.GetChildGroups()
                .Where(subGroup =>
                {
                    if (subGroup == null)
                        return false;

                    if (User.AllowedGroup(subGroup))
                        return false;

                    if (groupFilter.ApplyToSeries != 1)
                        return true;

                    return subGroup.GetAllSeries().Any(series => seriesIDs.Contains(series.AnimeSeriesID));
                })
                .OrderByGroupFilter(groupFilter)
                .Select(group => new Group(HttpContext, group, randomImages))
                .ToList();
        }

        /// <summary>
        /// Get a list of all the <see cref="Series"/> for the <see cref="Group"/> within the <see cref="Filter"/>.
        /// </summary>
        /// <remarks>
        ///  Pass a <paramref name="filterID"/> of <code>0</code> to disable filter or .
        /// </remarks>
        /// <param name="filterID"><see cref="Filter"/> ID</param>
        /// <param name="groupID"><see cref="Group"/> ID</param>
        /// <param name="recursive">Show all the <see cref="Series"/> within the <see cref="Group"/>. Even the <see cref="Series"/> within the sub-<see cref="Group"/>s.</param>
        /// <param name="includeMissing">Include <see cref="Series"/> with missing <see cref="Episode"/>s in the list.</param>
        /// <param name="randomImages">Randomise images shown for each <see cref="Series"/> within the <see cref="Group"/>.</param>
        /// /// <returns></returns>
        [HttpGet("Filter/{filterID}/Group/{groupID}/Series")]
        public ActionResult<List<Series>> GetSeriesInFilteredGroup([FromRoute] int filterID, [FromRoute] int groupID, [FromQuery] bool recursive = false, [FromQuery] bool includeMissing = false, [FromQuery] bool randomImages = false)
        {
            // Return the groups with no group filter applied.
            if (filterID == 0)
                return GetSeriesInGroup(groupID, recursive);

            // Check if the group filter exists.
            var groupFilter = RepoFactory.GroupFilter.GetByID(filterID);
            if (groupFilter == null)
                return NotFound(FilterController.FilterNotFound);

            // Check if the group exists.
            var group = RepoFactory.AnimeGroup.GetByID(groupID);
            if (group == null)
                return NotFound(GroupController.GroupNotFound);

            if (groupFilter.ApplyToSeries != 1)
                return (recursive ? group.GetAllSeries() : group.GetSeries())
                    .Where(series => User.AllowedSeries(series))
                    .OrderBy(OrderByAirDate)
                    .Select(series => new Series(HttpContext, series))
                    .Where(series => series.Size > 0 || includeMissing)
                    .ToList();

            // Just return early because the every series will be filtered out.
            if (!groupFilter.SeriesIds.TryGetValue(User.JMMUserID, out var seriesIDs))
                return new List<Series>();

            return (recursive ? group.GetAllSeries() : group.GetSeries())
                .Where(series => seriesIDs.Contains(series.AnimeSeriesID))
                .OrderBy(OrderByAirDate)
                .Select(series => new Series(HttpContext, series))
                .Where(series => series.Size > 0 || includeMissing)
                .ToList();
        }

        #endregion
        #region Group

        /// <summary>
        /// Get a list of sub-<see cref="Group"/>s a the <see cref="Group"/>.
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="randomImages">Randomise images shown for the <see cref="Group"/>.</param>
        /// <returns></returns>
        [HttpGet("Group/{groupID}/Group")]
        public ActionResult<List<Group>> GetSubGroups([FromRoute] int groupID, [FromQuery] bool randomImages = false)
        {
            // Check if the group exists.
            var group = RepoFactory.AnimeGroup.GetByID(groupID);
            if (group == null)
                return NotFound(GroupController.GroupNotFound);

            return group.GetChildGroups()
                .Where(group => User.AllowedGroup(group))
                .OrderBy(OrderByName)
                .Select(group => new Group(HttpContext, group, randomImages))
                .ToList();
        }

        /// <summary>
        /// Get a list of <see cref="Series"/> within a <see cref="Group"/>.
        /// </summary>
        /// <remarks>
        /// It will return all the <see cref="Series"/> within the group and all sub-groups if
        /// <paramref name="recursive"/> is set to <code>true</code>.
        /// </remarks>
        /// <param name="groupID"><see cref="Group"/> ID</param>
        /// <param name="recursive">Show all the <see cref="Series"/> within the <see cref="Group"/></param>
        /// <param name="includeMissing">Include <see cref="Series"/> with missing <see cref="Episode"/>s in the list.</param>
        /// <param name="randomImages">Randomise images shown for each <see cref="Series"/> within the <see cref="Group"/>.</param>
        /// <returns></returns>
        [HttpGet("Group/{groupID}/Series")]
        public ActionResult<List<Series>> GetSeriesInGroup([FromRoute] int groupID, [FromQuery] bool recursive = false, [FromQuery] bool includeMissing = false, [FromQuery] bool randomImages = false)
        {
            // Check if the group exists.
            var group = RepoFactory.AnimeGroup.GetByID(groupID);
            if (group == null)
                return NotFound(GroupController.GroupNotFound);

            return (recursive ? group.GetAllSeries() : group.GetSeries())
                .Where(a => User.AllowedSeries(a))
                .OrderBy(OrderByAirDate)
                .Select(series => new Series(HttpContext, series))
                .Where(series => series.Size > 0 || includeMissing)
                .ToList();
        }

        #endregion
        #region Series

        /// <summary>
        /// Get the <see cref="Episode"/>s for the <see cref="Series"/> with <paramref name="seriesID"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="Filter"/> or <see cref="Group"/> is irrelevant at this level.
        /// </remarks>
        /// <param name="seriesID">Series ID</param>
        /// <param name="includeMissing">Include missing episodes in the list.</param>
        /// <returns></returns>
        [HttpGet("Series/{seriesID}/Episode")]
        public ActionResult<List<Episode>> GetEpisodes([FromRoute] int seriesID, [FromQuery] bool includeMissing = false)
        {
            var series = RepoFactory.AnimeSeries.GetByID(seriesID);
            if (series == null)
                return NotFound(SeriesController.SeriesNotFoundWithSeriesID);
            if (!User.AllowedSeries(series))
                return Forbid(SeriesController.SeriesForbiddenForUser);

            return series.GetAnimeEpisodes()
                .Select(a => new Episode(HttpContext, a))
                .Where(a => a.Size > 0 || includeMissing)
                .ToList();
        }

        #endregion
        #region Episode

        /// <summary>
        /// Get the <see cref="File.FileDetailed"/>s for the <see cref="Episode"/> with the given <paramref name="episodeID"/>.
        /// </summary>
        /// <param name="episodeID">Episode ID</param>
        /// <returns></returns>
        [HttpGet("Episode/{episodeID}/File")]
        public ActionResult<List<File.FileDetailed>> GetFiles([FromRoute] int episodeID)
        {
            var episode = RepoFactory.AnimeEpisode.GetByID(episodeID);
            if (episode == null)
                return NotFound(EpisodeController.EpisodeNotFoundWithEpisodeID);

            var series = episode.GetAnimeSeries();
            if (series == null)
                return InternalError("No Series entry for given Episode");
            if (!User.AllowedSeries(series))
                return Forbid(EpisodeController.EpisodeForbiddenForUser);

            return episode.GetVideoLocals()
                .Select(file => new File.FileDetailed(file))
                .ToList();
        }

        #endregion
        #region Ordering helpers

        [NonAction]
        private DateTime? OrderByAirDate(SVR_AnimeSeries series)
        {
            var anime = RepoFactory.AniDB_Anime.GetByAnimeID(series.AniDB_ID);
            if (anime.AirDate.HasValue && anime.AirDate.Value != DateTime.MinValue)
                return anime.AirDate;

            return null;
        }

        [NonAction]
        private string OrderByName(SVR_AnimeGroup group)
        {
            return group.SortName;
        }

        [NonAction]
        private string OrderByName(SVR_GroupFilter group)
        {
            return group.GroupFilterName;
        }

        #endregion
    }
}