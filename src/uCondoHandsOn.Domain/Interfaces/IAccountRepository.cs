using uCondoHandsOn.Domain.Entities;
using uCondoHandsOn.Domain.Enums;

namespace uCondoHandsOn.Domain.Interfaces
{
    public interface IAccountRepository
    {
        ValueTask<IEnumerable<Account>> GetAsync(string search, AccountType? type, bool? allowEntries);
        ValueTask<Account> GetAsync(string code);
        ValueTask<Account> InsertAsync(Account entity);
		Task DeleteAsync(string code);
        ValueTask<bool> IsDuplicatedAsync(string code);
    }
}