using Talktoyeat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talktoyeat.Core
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<ImageUri> ImageUris { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        // public DbSet<Event> Events { get; set; } = null!;
        // public DbSet<Group> Groups { get; set; } = null!;




        //follows
    }
}
