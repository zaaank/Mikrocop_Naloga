using Microsoft.AspNetCore.Mvc;
using UserRepo.Application.Common;
using UserRepo.Application.Errors;

namespace UserRepo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return HandleFailure(result);
    }

    protected ActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return HandleFailure(result);
    }

    protected ActionResult HandleFailure(Result result)
    {
        if (result.Error == UserErrors.NotFound)
        {
            return NotFound(result.Error);
        }

        if (result.Error == UserErrors.DuplicateUsername)
        {
            return Conflict(result.Error);
        }

        if (result.Error == UserErrors.InvalidCredentials)
        {
            return Unauthorized(result.Error);
        }

        return BadRequest(result.Error);
    }
}
