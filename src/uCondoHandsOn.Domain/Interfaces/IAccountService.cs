using uCondoHandsOn.Domain.Dto;
using uCondoHandsOn.Domain.Enums;
using uCondoHandsOn.Domain.Validation;

namespace uCondoHandsOn.Domain.Interfaces
{
    public interface IAccountService
    {
        ValueTask<ValidationResult<IEnumerable<AccountDto>>> GetAsync(string search, AccountType? type, bool? allowEntries);
        ValueTask<ValidationResult<NextAccountCodeDto>> GetNextCodeAsync(string accountCode);
        ValueTask<ValidationResult<AccountDto>> CreateAsync(AccountCreateDto dto);
        ValueTask<ValidationResult> DeleteAsync(string code);
    }
}
