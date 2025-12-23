using Microsoft.AspNetCore.Mvc;
using UserRepo.Application.Common;
using UserRepo.Application.Errors;

namespace UserRepo.API.Controllers;

/// <summary>
/// Base class for all API Controllers.
/// Provides helper methods to map Application "Result" objects to HTTP ActionResults.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    /// Helper to handle results that have a value (e.g., GetUser return a UserResponse).
    /// </summary>
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return HandleFailure(result);
    }

    /// <summary>
    /// Helper to handle results that don't have a value (e.g., DeleteUser return bool).
    /// </summary>
    protected ActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return HandleFailure(result);
    }

    /// <summary>
    /// Centralized mapping of Domain/Application errors to HTTP Status Codes.
    /// </summary>
    protected ActionResult HandleFailure(Result result)
    {
        // 404 Not Found
        if (result.Error == UserErrors.NotFound)
        {
            return NotFound(result.Error);
        }

        // 409 Conflict
        if (result.Error == UserErrors.DuplicateUsername)
        {
            return Conflict(result.Error);
        }

        // 401 Unauthorized
        if (result.Error == UserErrors.InvalidCredentials)
        {
            return Unauthorized(result.Error);
        }

        // Default to 400 Bad Request
        return BadRequest(result.Error);
    }
}
