using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserRepo.Application.Interfaces;
using UserRepo.Contracts.Requests;
using UserRepo.Contracts.Responses;

namespace UserRepo.API.Controllers
{
    /// <summary>
    /// Exposes endpoints for managing Users.
    /// Inherits from ApiController for consistent Result handling.
    /// </summary>
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateUserAsync(request);
            
            if (result.IsSuccess)
            {
                // Returns 201 Created with the location of the new resource.
                return CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value);
            }

            // Maps the error result to a proper HTTP status code (Conflict, BadRequest, etc.)
            return HandleFailure(result);
        }

        /// <summary>
        /// Retrieves user details by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> GetUser(Guid id)
        {
            // HandleResult automatically returns 200 OK or 404 Not Found.
            return HandleResult(await _userService.GetUserAsync(id));
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            return HandleResult(await _userService.UpdateUserAsync(id, request));
        }

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return HandleFailure(result);
        }

        /// <summary>
        /// Checks if the provided username and password match.
        /// </summary>
        [HttpPost("validate-password")]
        public async Task<IActionResult> ValidatePassword([FromBody] ValidatePasswordRequest request)
        {
            var result = await _userService.ValidatePasswordAsync(request.UserName, request.Password);
            
            if (result.IsSuccess)
            {
               return Ok("Password is valid.");
            }

            return HandleFailure(result);
        }
    }
}
