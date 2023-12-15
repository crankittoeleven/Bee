using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("MainConnectionString")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasRequired(p => p.Author).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Post>().HasRequired(p => p.Owner).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Like>().HasRequired(l => l.Post).WithMany().WillCascadeOnDelete(true);
            modelBuilder.Entity<Comment>().HasRequired(c => c.Post).WithMany().WillCascadeOnDelete(true);
            modelBuilder.Entity<Work>().HasRequired(w => w.User).WithMany().WillCascadeOnDelete(true);
            modelBuilder.Entity<Education>().HasRequired(e => e.User).WithMany().WillCascadeOnDelete(true);
            modelBuilder.Entity<Skill>().HasRequired(s => s.User).WithMany().WillCascadeOnDelete(true);
            modelBuilder.Entity<Language>().HasRequired(l => l.User).WithMany().WillCascadeOnDelete(true);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Language> Languages { get; set; }
    }
}