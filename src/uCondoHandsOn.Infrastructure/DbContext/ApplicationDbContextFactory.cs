using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using uCondoHandsOn.Infra.Context;

namespace uCondoHandsOn.Infrastructure.DbContext
{
    public class ApplicationDbContextContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("localhost")
                    .EnableSensitiveDataLogging()
                    .Options;

            return new ApplicationDbContext(options);
        }
    }
       
 }
