
using uCondoHandsOn.Domain.Enums;

namespace uCondoHandsOn.Domain.Dto
{
    public class AccountDto
    {
        public string ? Code { get; set; }
        public string ? Name { get; set; }
        public AccountType Type { get; set; }
        public bool AllowEntries { get; set; }
        public IEnumerable<AccountDto>? Children { get; set; }
    }
}