﻿using System;
using OpenRasta.Web;


namespace Ramone.Tests.Server.Handlers.Blog
{
  public class BlogItemHandler
  {
    public BlogItem Get(int id)
    {
      BlogDB.PostEntry postEntry = BlogDB.Get(id);
      AuthorDB.AuthorEntry authorEntry = AuthorDB.Get(postEntry.AuthorId);

      BlogItem item = new BlogItem
      {
        Id = postEntry.Id,
        Title = postEntry.Title,
        Text = postEntry.Text,
        CreatedDate = postEntry.CreatedDate,
        AuthorName = authorEntry.Name
      };

      item.SelfLink = typeof(BlogItem).CreateUri(new { Id = postEntry.Id });
      item.UpLink = typeof(BlogList).CreateUri();
      item.AuthorLink = typeof(Author).CreateUri(new { Id = authorEntry.Id });

      return item;
    }
  }
}