using ResApi.Services.Interface;
using ResApi.DataResponse;
using ResApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ResApi.Controllers
{
    [ApiController]
    [Route("[api/user]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly ResApi.Services.Interface.ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ITokenService tokenService, ILogger<UserController> logger)
        {
            _service = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserDto model)
        {
            try
            {
                var response = await _service.Authenticate(model);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Authenticate POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error while authenticating user"

                };
                return BadRequest(errRet);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            try
            {
                var response = await _service.Register(model);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
        }

        [HttpPost("updateCustomer")]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerDto model)
        {
            try
            {
                var response = await _service.UpdateCustomer(model);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdateCustomer POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error while updating customer"
                };
                return BadRequest(errRet);
            }
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            try
            {
                var response = await _service.ChangePassword(model);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ChangePassword POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error while changing password for customer"
                };
                return BadRequest(errRet);
            }
        }

        [HttpPost("logoutUser")]
        public async Task<IActionResult> LogoutUser(string token)
        {
            await _tokenService.DeactivateAsync(token);
            return NoContent();
        }

        [HttpGet("checkActiveSession")]
        public async Task<IActionResult> CheckActiveSession(string token)
        {
            var result = await _tokenService.IsActiveAsync(token);
            var response = new DataResponse<bool>
            {
                Data = result,
                Succeeded = true
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login(string email)
        {
            try
            {
                var response = await _service.Login(email);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Authenticate POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error while authenticating user"

                };
                return BadRequest(errRet);
            }
        }

    }
}