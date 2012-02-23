﻿using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public class AuthorHandler
  {
    public Author Get(int id)
    {
      AuthorDB.AuthorEntry entry = AuthorDB.Get(id);

      Author a = new Author
      {
        Id = entry.Id,
        Name = entry.Name,
        EMail = entry.EMail
      };

      a.SelfLink = typeof(Author).CreateUri(new { Id = a.Id });

      return a;
    }
  }
}