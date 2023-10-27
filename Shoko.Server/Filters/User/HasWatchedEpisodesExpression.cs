using System;
using Shoko.Server.Filters.Interfaces;

namespace Shoko.Server.Filters.User;

public class HasWatchedEpisodesExpression : FilterExpression<bool>
{
    public override bool TimeDependent => false;
    public override bool UserDependent => true;
    public override string HelpDescription => "This passes if the current user has any watched episodes in the filterable";

    public override bool Evaluate(IFilterable filterable, IFilterableUserInfo userInfo)
    {
        ArgumentNullException.ThrowIfNull(userInfo);
        return userInfo.WatchedEpisodes > 0;
    }

    protected bool Equals(HasWatchedEpisodesExpression other)
    {
        return base.Equals(other);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((HasWatchedEpisodesExpression)obj);
    }

    public override int GetHashCode()
    {
        return GetType().FullName!.GetHashCode();
    }

    public static bool operator ==(HasWatchedEpisodesExpression left, HasWatchedEpisodesExpression right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(HasWatchedEpisodesExpression left, HasWatchedEpisodesExpression right)
    {
        return !Equals(left, right);
    }
}
