using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace uCondoHandsOn.Application.Tests
{
    public class AccountServiceTests
    {
        IAccoutntService _accountService;

        private readonly IAccount _accountParent = new Account
        {
           Code = "1",
           Name = "Incomes",
           Type = AccountType.Income,
        }
    }
}