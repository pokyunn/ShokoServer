﻿using System.Linq;
using NHibernate;
using Shoko.Server.Models.Trakt;
using Shoko.Server.Databases;

namespace Shoko.Server.Repositories.Direct;

public class Trakt_ShowRepository : BaseDirectRepository<Trakt_Show, int>
{
    public Trakt_Show GetByTraktSlug(string slug)
    {
        return Lock(() =>
        {
            using var session = _databaseFactory.SessionFactory.OpenSession();
            return GetByTraktSlugUnsafe(session, slug);
        });
    }


    public Trakt_Show GetByTraktSlug(ISession session, string slug)
    {
        return Lock(() => GetByTraktSlugUnsafe(session, slug));
    }

    private static Trakt_Show GetByTraktSlugUnsafe(ISession session, string slug)
    {
        return session.Query<Trakt_Show>()
            .Where(a => a.TraktID == slug)
            .Take(1).SingleOrDefault();
    }

    public Trakt_ShowRepository(DatabaseFactory databaseFactory) : base(databaseFactory)
    {
    }
}
