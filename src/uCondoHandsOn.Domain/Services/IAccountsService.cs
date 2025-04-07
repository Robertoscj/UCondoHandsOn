using uCondoHandsOn.Domain.Dto;
using uCondoHandsOn.Domain.Enums;
using uCondoHandsOn.Domain.Validation;

namespace uCondoHandsOn.Domain.Services
{
    public interface IAccountsService
    {
        ValueTask<uCondoHandsOn.Domain.Validation.ValidationResult<IEnumerable<AccountDto>>> GetAsync(string search, AccountType? type, bool? allowEntries);
        ValueTask<uCondoHandsOn.Domain.Validation.ValidationResult<NextAccountCodeDto>> GetNextCodeAsync(string accountCode);
        ValueTask<uCondoHandsOn.Domain.Validation.ValidationResult<AccountDto>> CreateAsync(AccountCreateDto dto);
        ValueTask<uCondoHandsOn.Domain.Validation.ValidationResult> DeleteAsync(string code);
    }
}