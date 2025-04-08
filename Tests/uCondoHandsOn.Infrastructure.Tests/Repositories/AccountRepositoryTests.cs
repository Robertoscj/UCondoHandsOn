using uCondoHandsOn.Domain.Entities;
using uCondoHandsOn.Domain.Enums;
using uCondoHandsOn.Domain.Interfaces.Repositories;
using uCondoHandsOn.Infra.Repositories;

namespace uCondoHandsOn.Infrastructure.Tests.Repositories
{
    public class MockDbContextFix : IClassFixture<MockDbContextFix>
    {
        public AccountsRepositoryTests(MockDbContextFix fix)
        {
            _repository = new AccountsRepository(fix.Context);
        }

        [Fact]
        public async Task GetNotEmpty()
        {
            var entitie = await _repository.GetAsync(null, null, null);

            Assert.NotEmpty(entitie);
        }

        [Fact]
        public async Task GetFilterExpense()
        {
            var entitie = await _repository.GetAsync(null, AccountType.Expense, null);

            Assert.All(entitie, x => Assert.Equal(AccountType.Expense, x.Type));
        }



        [Fact]
        public async Task GetAllowEntriesTrue()
        {
            var entitie = await _repository.GetAsync(null, null, true);

            Assert.All(entitie, x => Assert.True(x.AllowEntries));
        }
    }
}