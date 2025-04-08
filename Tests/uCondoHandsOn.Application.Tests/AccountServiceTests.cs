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
    };

    private readonly AccountEntity _RootParent = new AccountEntity
    {
        Code = "2",
        Name = "Expenses",
        Type = AccountType.Expense
    };


    private readonly AccountEntity _incomeChild = new AccountEntity
    {
        Code = "1.2",
        ParentCode = "1",
        Name = "Incomes Child",
        Type = AccountType.Income
    };

    private readonly AccountEntity _incomeChilds = new AccountEntity
    {
        Code = "1.10",
        ParentCode = "1",
        Name = "Incomes Childs 10",
        Type = AccountType.Income
 };



    }
}