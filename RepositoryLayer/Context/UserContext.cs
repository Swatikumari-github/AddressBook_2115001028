using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public virtual DbSet<UsersEntity> Users { get; set; }


        public virtual DbSet<AddressBookEntity> AddressBookEntries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Foreign Key Relationship
            modelBuilder.Entity<AddressBookEntity>()
                .HasOne(u => u.User)
                .WithMany(a => a.AddressBookEntries)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique Email Constraint
            modelBuilder.Entity<UsersEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
