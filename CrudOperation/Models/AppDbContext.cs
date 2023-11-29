using CrudOperation.VM;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace CrudOperation.Models
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RegisterdataViewModel>().HasNoKey();
            modelBuilder.Entity<LoginVM>().HasNoKey();
            modelBuilder.Entity<RoleVM>().HasNoKey();
            modelBuilder.Entity<ChangePassword>().HasNoKey();

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Reports> Reports { get; set; }
        public DbSet<CrudOperation.VM.ChangePassword> ChangePassword { get; set; } = default!;


    }
}
