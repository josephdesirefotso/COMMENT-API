using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Entities
{
    public class CommentContext: DbContext 
    {
        public CommentContext(DbContextOptions<CommentContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Reply> Replies { get; set; }
    }
}
