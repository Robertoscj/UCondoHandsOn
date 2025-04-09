using Microsoft.AspNetCore.Mvc;
using uCondoHandsOn.Domain.Dto;
using uCondoHandsOn.Domain.Enums;
using uCondoHandsOn.Domain.Interfaces;
using uCondoHandsOn.Domain.Validation;

namespace uCondoHandsOn.API.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : BaseController
    {
       private readonly IAccountService _accountService;

        
       public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        /// <summary>
        /// Retrieves a list of accounts based on optional filters
        /// </summary>
        /// <param name="search">Text to search in account code or name</param>
        /// <param name="type">Filter by account type</param>
        /// <param name="allowEntries">Filter by accounts that allow entries</param>
        /// <returns>List of accounts matching the criteria</returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string search, [FromQuery] AccountType? type, [FromQuery] bool? allowEntries)
        {
            var result = await _accountService.GetAsync(search, type, allowEntries);

            if (result.Invalid)
                return new NotFoundResult();

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Gets the next available account code based on a parent account
        /// </summary>
        /// <param name="code">Parent account code</param>
        /// <returns>Next available account code</returns>
        [HttpGet("{code}/next")]
        public async Task<IActionResult> GetNextCodeAsync(string code)
        {
            var result = await _accountService.GetNextCodeAsync(code);

            if (result == null)
                return new NotFoundResult();
            
            if (result.Invalid)
                return BadRequest(result);

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Creates a new account in the system
        /// </summary>
        /// <param name="dto">Account creation data</param>
        /// <returns>Created account information</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AccountCreateDto dto)
        {
            if (dto is IValidationDto dtoValidation)
            {
                var validationResult = dtoValidation.IsValid();

                if (validationResult.Invalid)
                    return BadRequest(validationResult);
            }

            var result = await _accountService.CreateAsync(dto);

            if (result.Invalid)
                return BadRequest(result);

            return new CreatedResult("api/accounts", result);
        }

        /// <summary>
        /// Deletes an account from the system
        /// </summary>
        /// <param name="code">Account code to delete</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteAsync(string code)
        {
            var result = await _accountService.DeleteAsync(code);

            if (result.Invalid)
                return BadRequest(result);

            return new NoContentResult();
        }
    }
}