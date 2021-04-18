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
            modelBuilder.Entity<ForumPost>().HasMany(x => x.Opinions).WithOne(x => x.ForumPost)
                .HasForeignKey(x => x.ForumPostId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ForumPost>().HasMany(x => x.Images).WithOne(x => x.ForumPost)
                .HasForeignKey(x => x.ForumPostId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasMany(x => x.ForumPosts).WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Images).WithOne(x => x.User).HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Opinions).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(x => x.ForumPosts).WithOne(x => x.User)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        

        private void Seed(ModelBuilder modelBuilder) // utwórz nową klasę która będzie przyjmować w konstruktorze dbContext i operować na nim
        {
            ;
        }
    }
}
