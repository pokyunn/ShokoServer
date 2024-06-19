﻿using NutzCode.InMemoryIndex;
using Shoko.Models.Server;
using Shoko.Server.Databases;

namespace Shoko.Server.Repositories.Cached;

public class AnimeStaffRepository : BaseCachedRepository<AnimeStaff, int>
{
    private PocoIndex<int, AnimeStaff, int> AniDBIDs;

    public override void RegenerateDb()
    {
    }

    protected override int SelectKey(AnimeStaff entity)
    {
        return entity.StaffID;
    }

    public override void PopulateIndexes()
    {
        AniDBIDs = new PocoIndex<int, AnimeStaff, int>(Cache, a => a.AniDBID);
    }


    public AnimeStaff GetByAniDBID(int id)
    {
        return ReadLock(() => AniDBIDs.GetOne(id));
    }

    public AnimeStaffRepository(DatabaseFactory databaseFactory) : base(databaseFactory)
    {
    }
}
