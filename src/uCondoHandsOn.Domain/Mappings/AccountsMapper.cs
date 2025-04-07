using uCondoHandsOn.Domain.Dto;
using uCondoHandsOn.Domain.Entities;

namespace uCondoHandsOn.Domain.Mappings
{
    public static class AccountsMapper
    {
       
        public static AccountDto Map (Account entity)
        {
            var Children = new  List<AccountDto>(entity.Children.Count());
            
            foreach (var child in entity.Children)
            {
                Children.Add(Map(child));
            }

            return new AccountDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Type = entity.Type,
                AllowEntries = entity.AllowEntries,
                Children = Children
            };

        }

        public static IEnumerable<AccountDto> Map(IEnumerable<Account> entities)
        {
            return entities.Select(Map);
        }


    }
}