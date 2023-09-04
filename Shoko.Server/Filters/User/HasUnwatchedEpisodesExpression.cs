namespace Shoko.Server.Filters.User;

public class HasUnwatchedEpisodesExpression : UserDependentFilterExpression<bool>
{
    public override bool TimeDependent => false;
    public override bool UserDependent => true;

    public override bool Evaluate(UserDependentFilterable filterable)
    {
        return filterable.UnwatchedEpisodes > 0;
    }
}
