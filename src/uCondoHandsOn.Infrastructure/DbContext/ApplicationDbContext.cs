using Microsoft.EntityFrameworkCore;
using uCondoHandsOn.Domain.Entities;

namespace uCondoHandsOn.Infra.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<DbContext> options): base(options) { }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                        .HasOne(x => x.Parent)
                        .WithMany(x => x.Children)
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .HasForeignKey(x => x.ParentCode);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Account> Accounts { get; set; }
    }
}