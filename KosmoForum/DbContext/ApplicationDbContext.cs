using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KosmoForum.Models;
using Microsoft.EntityFrameworkCore;

namespace KosmoForum.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ForumPost>()
                .HasMany(frm => frm.Images).WithOne(frm => frm.ForumPost).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ForumPost>()
                .HasMany(x => x.Opinions).WithOne(x => x.ForumPost).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User>()
                .HasMany(x => x.Images).WithOne(x => x.User).OnDelete(DeleteBehavior.NoAction);
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
