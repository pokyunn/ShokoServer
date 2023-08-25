using System;
using Shoko.Server.Models.Filters.Interfaces;

namespace Shoko.Server.Models.Filters.Selectors;

public class TotalEpisodeCountSelector : INumberSelector
{
    public bool UserDependent => false;
    public Func<IFilterable, double> Selector => f => f.TotalEpisodeCount;
}
