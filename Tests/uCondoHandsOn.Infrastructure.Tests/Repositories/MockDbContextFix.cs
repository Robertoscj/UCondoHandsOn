using Microsoft.EntityFrameworkCore;
using uCondo.HandsOn.Domain.Entities;
using uCondo.HandsOn.Domain.Enums;
using uCondo.HandsOn.Infra.Context;

namespace uCondoHandsOn.Infra.Tests
{
    public sealed class MockDbContextFix : IDisposable
    {
        private static bool _create = false;
        public static readonly object _block = new();

        public ApplicationDbContext Context { get; private set; }

        public MockDbContextFix()
        {
            Context = CreateContext();

            lock (_block)
            {
                if (_create) return;

                _create = true;

                var localContext = CreateContext();

                Initialize(localContext);
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        private static ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "memorydb")
                    .Options;

            return new ApplicationDbContext(options);
        }

        private static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            context.Entry(new Account
            {
                Code = "1",
                Name = "Expense",
                AllowEntries = false,
                Type = AccountType.Expense,
            }).State = EntityState.Added;

            context.Entry(new Account
            {
                Code = "2",
                Name = "Income",
                AllowEntries = true,
                Type = AccountType.Income
            }).State = EntityState.Added;

            context.SaveChanges();
            context.Dispose();
        }
    }
}

