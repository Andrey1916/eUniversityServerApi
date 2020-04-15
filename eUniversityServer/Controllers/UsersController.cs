using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eUniversityServer.Models.BindingModels;
using eUniversityServer.Models.ViewModels;
using eUniversityServer.Services.Exceptions;
using eUniversityServer.Services.Interfaces;
using eUniversityServer.Utils;
using eUniversityServer.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using UserDto = eUniversityServer.Services.Dtos.User;
using Dtos = eUniversityServer.Services.Dtos;
using eUniversityServer.Utils.Auth;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace eUniversityServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly Logger _logger;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;


        public UsersController(IMapper mapper, IUserService userService, Logger logger)
        {
            _mapper      = mapper ?? throw new NullReferenceException(nameof(mapper));
            _userService = userService ?? throw new NullReferenceException(nameof(userService));
            _logger      = logger ?? throw new NullReferenceException(nameof(logger));
        }


        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody]SignInBindingModel userParam)
        {
            try
            {
                var user = await _userService.SignInAsync(userParam.Email, userParam.Password);

                if (user == null)
                    return BadRequest("Username or password is incorrect");

                var signedUser = _mapper.Map<Dtos.UserWithToken, SignInViewModel>(user);

                await _logger.Info($"SignIn user: { user.FirstNameEng } { user.LastNameEng }", user.Id);

                return Ok(signedUser);
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]SignUpBindingModel userParam)
        {
            try
            {
                var user = _mapper.Map<UserDto>(userParam);
                Guid userId = await _userService.SignUpAsync(user, userParam.Password);

                await _logger.Info($"SignUp new user: { user.FirstNameEng } { user.LastNameEng }", userId);

                var absoluteUri = string.Concat(
                            HttpContext.Request.Scheme,
                            "://",
                            HttpContext.Request.Host.ToUriComponent(),
                            "/api/users/confirm-email"
                            );


                await _userService.SendConfirmationEmailAsync(absoluteUri, userParam.Email);

                return Ok();
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }


        [AllowAnonymous]
        [HttpPost("sent-confirm-email/{email}")]
        public async Task<IActionResult> SentConfirmationEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return BadRequest("Email is empty");

            try
            {
                var absoluteUri = string.Concat(
                            HttpContext.Request.Scheme,
                            "://",
                            HttpContext.Request.Host.ToUriComponent(),
                            "/api/users/confirm-email"
                            );


                await _userService.SendConfirmationEmailAsync(absoluteUri, email);

                return Ok();
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [AllowAnonymous]
        [HttpGet("confirm-email/{token}")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest("Token is empty");

            try
            {
                await _userService.ConfirmEmailAsync(token);

                return View("~/Views/EmailConfirmed.cshtml");
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }


        [AllowAnonymous]
        [HttpPost("sent-recovery-email/{email}")]
        public async Task<IActionResult> SentPasswordRecoveryEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                return BadRequest("Email is empty");

            try
            {
                var uri = string.Concat(
                            HttpContext.Request.Scheme,
                            "://",
                            HttpContext.Request.Host.ToUriComponent(),
                            "/api/users/recovery/"
                            );


                await _userService.SendPasswordRecoveryEmailAsync(uri, email);

                return Ok();
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [AllowAnonymous]
        [HttpGet("recovery/{token}")]
        public async Task<IActionResult> RecoveryPassword(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest("Token is empty");

            try
            {
                ViewBag.Token = token;
                return View("~/Views/ResetPassword.cshtml");
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
        
        [AllowAnonymous]
        [HttpPost("recovery")]
        public async Task<IActionResult> RecoveryPassword([FromBody] RecoveryPasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(ModelState);
            }

            try
            {
                await _userService.ResetPasswordAsync(model.Token, model.NewPassword);

                return Ok();
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
        

        [Authorize]
        [HttpPost("signout")]
        public IActionResult SignOut()
        {
            try
            {
                var userIdClaim = this.HttpContext.User.FindFirst(ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    return this.NotFound("User not fount");
                }

                _userService.SignOut(Guid.Parse(userIdClaim.Value));

                return this.Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message).GetAwaiter().GetResult();
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Users)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usersDtos = await _userService.GetAllAsync();
                var users = _mapper.Map<IEnumerable<UserDto>, IEnumerable<UserViewModel>>(usersDtos);

                return Ok(users);
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("search")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Users)]
        public async Task<IActionResult> Get([FromQuery]SieveModel sieveModel)
        {
            try 
            {
                var usersDtos = await _userService.GetSomeAsync(sieveModel);
                var users = _mapper.Map<IEnumerable<UserDto>, IEnumerable<UserViewModel>>(usersDtos.Result);

                return Ok(new SieveResponseModel<UserViewModel> { 
                    Result     = users,
                    TotalCount = usersDtos.TotalCount
                });
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("{id}")]
        [AuthorizePermission(DAL.Enums.AccessModifier.CanRead, DAL.Enums.TargetModifier.Users)]
        public async Task<ActionResult<UserViewModel>> Get(Guid id)
        {
            if (id.Equals(Guid.Empty))
                return BadRequest("Id is empty");

            var usersDto = await _userService.GetByIdAsync(id);
            if (usersDto == null)
                return NotFound();

            var user = _mapper.Map<UserDto, UserViewModel>(usersDto);
            return Ok(user);
        }

        [HttpGet("whoami")]
        [Authorize]
        public async Task<IActionResult> WhoAmI()
        {
            try 
            {
                var userIdClaim = this.HttpContext.User.FindFirst(ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    return this.NotFound("User not fount");
                }

                var currentUser = await _userService.GetByIdAsync(Guid.Parse(userIdClaim.Value));

                if (currentUser == null)
                {
                    return this.NotFound("User not found");
                }

                var userDto = _mapper.Map<UserDto, UserViewModel>(currentUser);

                return Ok(userDto);
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromQuery]string token)
        {
            try
            {
                if (token == null || string.IsNullOrWhiteSpace(token))
                {
                    return this.BadRequest("Invalid token");
                }

                var tokenViewModel = _mapper.Map<Dtos.TokenInfo, TokenInfoViewModel>(await _userService.RefreshTokenAsync(token));

                return Ok(tokenViewModel);
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AppPolicies.AdministratorsOnly)]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserBindingModel model)
        {
            if (id.Equals(Guid.Empty) || !id.Equals(model.Id))
                return BadRequest("Id is empty or not equals to the id in the model");

            try 
            {
                var user = _mapper.Map<UserBindingModel, UserDto>(model);
                await _userService.UpdateAsync(user);

                return Ok();
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [Authorize(Policy = AppPolicies.AdministratorsOnly)]
        [HttpPatch("{id}/add-role/{roleId}")]
        public async Task<IActionResult> AddRole(Guid id, Guid roleId)
        {
            if (id.Equals(Guid.Empty) || roleId.Equals(Guid.Empty))
                return BadRequest("Id is empty or role name is empty");

            try
            {
                await _userService.AddUserToRoleAsync(id, roleId);

                return Ok();
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [Authorize(Policy = AppPolicies.AdministratorsOnly)]
        [HttpPatch("{id}/remove-role/{roleId}")]
        public async Task<IActionResult> RemoveRole(Guid id, Guid roleId)
        {
            if (id.Equals(Guid.Empty) || roleId.Equals(Guid.Empty))
                return NotFound("Id is empty or role name is empty");

            try
            {
                await _userService.RemoveUserFromRoleAsync(id, roleId);

                return Ok();
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AppPolicies.AdministratorsOnly)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id.Equals(Guid.Empty))
                return BadRequest("Id is empty");

            try
            {
                await _userService.RemoveAsync(id);

                await _logger.Info($"Deleted user with id: { id.ToString() }");

                return Ok();
            }
            catch (ServiceException aex)
            {
                return this.StatusCode((int)aex.ErrorCode, aex.Message);
            }
            catch (Exception ex)
            {
                await _logger.Error(ex.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
