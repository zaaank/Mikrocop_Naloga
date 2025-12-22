using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserRepo.Application.Interfaces;
using UserRepo.Contracts.Requests;
using UserRepo.Contracts.Responses;

namespace UserRepo.API.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateUserAsync(request);
            
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value);
            }

            return HandleFailure(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> GetUser(Guid id)
        {
            return HandleResult(await _userService.GetUserAsync(id));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            return HandleResult(await _userService.UpdateUserAsync(id, request));
        }

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
