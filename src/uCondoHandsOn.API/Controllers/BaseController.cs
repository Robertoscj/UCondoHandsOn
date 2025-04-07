using Microsoft.AspNetCore.Mvc;
using uCondoHandsOn.Domain.Validation;

namespace uCondoHandsOn.API.Controllers
{
    public class BaseController : Controller
    {
        protected static IActionResult BadRequest(ValidationResult result)
        {
            return new BadRequestObjectResult(new
            {
                success = false,
                message = result.Message
            });
        }

        protected static IActionResult BadRequest<T>(ValidationResult<T> result)
        {
            return new BadRequestObjectResult(new
            {
                success = false,
                message = result.Message
            });
        }
    }
}
    