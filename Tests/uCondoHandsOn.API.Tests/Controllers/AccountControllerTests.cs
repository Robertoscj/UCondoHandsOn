using Microsoft.AspNetCore.Mvc;
using Moq;
using uCondoHandsOn.API.Controllers;
using uCondoHandsOn.Domain.Dtos;
using uCondoHandsOn.Domain.Enums;
using uCondoHandsOn.Domain.Interfaces.Services;
using uCondoHandsOn.Domain.Validation;

namespace uCondoHandsOn.API.Tests.Controllers
{
 
    public class AccountControllerTests 
    {
        AccountController _controller;

        [Fact]
        public async Task GetResult()
        {
            // Given
            var mock = new Mock<IAccountService>();
             mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => ValidationResult<IEnumerable<AccountDto>>.Success(null));
            // When
            _controller = new AccountController(mock.Object);
            var result = await _controller.GetAsync(null, null, null);

            // Then
             Assert.IsType<Result>(result.Result);
        
        }

        [Fact]
        public async Task GetNextCode_OkResult()
        {
            var mock = new Mock<IAccountsService>();

            mock.Setup(x => x.GetNextCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(() => ValidationResult<NextAccountCodeDto>.Success(null));

            _controller = new AccountsController(mock.Object);

            var result = await _controller.GetNextCodeAsync("1");

            Assert.IsType<OkResult>(result);
        }

        

    }
}