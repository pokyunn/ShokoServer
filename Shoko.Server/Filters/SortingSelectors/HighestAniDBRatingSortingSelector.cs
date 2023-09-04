using System;

namespace Shoko.Server.Filters.SortingSelectors;

public class HighestAniDBRatingSortingSelector : SortingExpression
{
    public override bool TimeDependent => false;
    public override bool UserDependent => false;

    public override object Evaluate(Filterable f)
    {
        return Convert.ToDouble(f.HighestAniDBRating);
    }
}
