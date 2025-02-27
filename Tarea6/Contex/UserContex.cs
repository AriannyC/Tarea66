using Microsoft.EntityFrameworkCore;
using Tarea6.Models;

namespace Tarea6.Contex
{
    public class UserContex : DbContext
    {
        public UserContex(DbContextOptions<UserContex> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<RegiUs> RegiUss { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //fluen Api

            #region tabla

            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<RegiUs>().ToTable("Regis");


            #endregion

            #region "Primary Key"
            modelBuilder.Entity<User>().HasKey(User => User.Id);
            modelBuilder.Entity<RegiUs>().HasKey(User => User.IdR);


            #endregion


            #region "Configuration"

            modelBuilder.Entity<User>().Property(User => User.Name).IsRequired();

            modelBuilder.Entity<User>().Property(User => User.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<User>().Property(User => User.FechaN).IsRequired();

            #endregion


            #region "Configuration Regi"

            modelBuilder.Entity<RegiUs>().Property(User => User.Username).IsRequired();

            modelBuilder.Entity<RegiUs>().Property(User => User.Password).IsRequired().HasMaxLength(100);

            #endregion
        }
    }
}

