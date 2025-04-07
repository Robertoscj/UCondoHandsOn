using uCondoHandsOn.Domain.Dto;
using uCondoHandsOn.Domain.Entities;
using uCondoHandsOn.Domain.Enums;
using uCondoHandsOn.Domain.Interfaces;
using uCondoHandsOn.Domain.Mappings;
using uCondoHandsOn.Domain.Services;
using uCondoHandsOn.Domain.Validation;

namespace uCondoHandsOn.Application.Services
{
    public class AccountService : IAccountsService
    {
        private readonly IAccountRepository _accountRepository;
       
       public AccountService(IAccountRepository accountRepository)
       {
         _accountRepository = accountRepository;
       }

        public async ValueTask<ValidationResult<IEnumerable<AccountDto>>> GetAsync(string search, AccountType? type, bool? allowEntries)
        {
            var accounts = await _accountRepository.GetAsync(search, type, allowEntries);
            accounts = FilterWithParents(search, accounts);
            accounts = LoadChildren(accounts);

            var result = ValidationResult<IEnumerable<AccountDto>>.Success(AccountsMapper.Map(accounts));

            return result;
        }


        public async ValueTask<ValidationResult<AccountDto>> CreateAsync(AccountCreateDto dto)
        {
            var parentAccount = await _accountRepository.GetAsync(dto.ParentCode);

            if (!string.IsNullOrEmpty(dto.ParentCode) && parentAccount == null)
                return ValidationResult<AccountDto>.Fail($"A conta pai informada não existe.");

            if (parentAccount != null)
            {
                var checkAllowChildrenResult = CheckAllowChildren(parentAccount);

                if (checkAllowChildrenResult.Invalid)
                    return ValidationResult<AccountDto>.Fail(checkAllowChildrenResult.Message);

                var checkAllowLevelResult = CheckAllowLevel(parentAccount.Code, dto.Code);

                if (checkAllowLevelResult.Invalid)
                    return ValidationResult<AccountDto>.Fail(checkAllowLevelResult.Message);

                if (parentAccount.Type != dto.Type)
                    return ValidationResult<AccountDto>.Fail($"O tipo da conta filha não corresponde ao tipo da conta pai.");
            }

            if (await _accountRepository.IsDuplicatedAsync(dto.Code))
                return ValidationResult<AccountDto>.Fail($"O código {dto.Code} já pertence a outra conta.");

            var entity = await _accountRepository.InsertAsync(new Account
            {
                Code = dto.Code,
                Name = dto.Name,
                Type = dto.Type,
                ParentCode = dto.ParentCode,
                AllowEntries = dto.AllowEntries
            });

            return ValidationResult<AccountDto>.Success(AccountsMapper.Map(entity));
        }
       

        public async ValueTask<ValidationResult> DeleteAsync(string code)
        {
            await _accountRepository.DeleteAsync(code);
            return ValidationResult.Success();
        }

       

       public async ValueTask<ValidationResult<NextAccountCodeDto>> GetNextCodeAsync(string accountCode)
        {
           var parentAccount = await _accountRepository.GetAsync(accountCode);
            var allowChildrenValidation = CheckAllowChildren(parentAccount);

            if (allowChildrenValidation.Invalid)
                return ValidationResult<NextAccountCodeDto>.Fail(allowChildrenValidation.Message);

                var lastChildCode = parentAccount.Children
                    .OrderBy(x => x)
                    .LastOrDefault()?.Code ?? $"{parentAccount.Code}.0";

                var codeParts = lastChildCode.Split('.');
                var lastSegment = codeParts.Last();  
                

            if (!int.TryParse(lastSegment, out var lastNumber))
              return ValidationResult<NextAccountCodeDto>.Fail($"Código inválido: '{lastChildCode}'. Parte final '{lastSegment}' não é um número.");

            if (lastNumber == 999)
            {
                var firstLevelCode = codeParts.First();

                return await GetNextCodeAsync(firstLevelCode);
            }
            var nextCode = $"{parentAccount.Code}.{lastNumber + 1}";
            {
                return ValidationResult<NextAccountCodeDto>.Success(new NextAccountCodeDto
                {
                    GeneratedCode  = parentAccount.Code,
                    ParentCode  = nextCode
                });
            }

        }


    private static IEnumerable<Account> FilterWithParents(string search, IEnumerable<Account> allAccounts)
    {
        if (string.IsNullOrWhiteSpace(search))
            return allAccounts;

        var filtered = allAccounts
            .Where(x => x.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase)
            || x.Code.StartsWith(search, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        var result = new HashSet<Account>(filtered);

        foreach (var account in filtered)
        {
            AddAllAncestors(account, allAccounts, result);
        }

        return result;
    }

    private static void AddAllAncestors(Account account, IEnumerable<Account> allAccounts, HashSet<Account> result)
    {
        var currentParentCode = account.ParentCode;

        while (!string.IsNullOrEmpty(currentParentCode))
        {
            var parent = allAccounts.FirstOrDefault(a => a.Code == currentParentCode);
            if (parent == null || result.Contains(parent))
                break;

            result.Add(parent);
            currentParentCode = parent.ParentCode;
        }
    }

     private static ValidationResult CheckAllowChildren(Account entity)
    {
        if (entity.AllowEntries)
        return ValidationResult.Fail($"A conta pai '{entity.Code} - {entity.Name}' aceita lançamentos, portanto não pode ter contas filhas.");

        return ValidationResult.Success();
     }

       private static IEnumerable<Account> LoadChildren(IEnumerable<Account> entities)
        {
            foreach (var filteredEntity in entities)
                filteredEntity.Children = new List<Account>(entities
                    .Where(x => x.ParentCode == filteredEntity.Code)
                    .OrderBy(x => x));

            return entities
                .OrderBy(x => x)
                .Where(x => x.ParentCode == null);
        }

        private static ValidationResult CheckAllowLevel(string parentCode, string childCode)
        {
            var parentLevels = parentCode.Count(x => x == '.') + 1;
            var childLevels = childCode.Count(x => x == '.') + 1;

            if (parentLevels + 1 != childLevels)
                return ValidationResult.Fail($"A conta filha '{childCode}' deve ter um nível a mais que a conta pai '{parentCode}'.");

            return ValidationResult.Success();
        }

        
    }
}