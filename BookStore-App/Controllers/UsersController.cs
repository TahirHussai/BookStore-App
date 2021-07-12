
using BookStore_App.Contracts;
using BookStore_App.Data.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILoggerService _loggerService;
        private readonly IConfiguration _config;
        public UsersController(IConfiguration configuration,  ILoggerService loggerService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loggerService = loggerService;
            _config = configuration;


        }
        /// <summary>
        /// User End Point
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
       [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]UserDto userDTO)
        {
           var location = GetControllerActionName();
            try
            {
                var username = userDTO.EmailAddress;
                var password = userDTO.Password;
                _loggerService.LogInfo($"Registration Attempt for {username} ");
                var user = new IdentityUser { Email = username, UserName = username };
                var result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _loggerService.LogError($"{error.Code} {error.Description}");
                    }
                    return InternalError($"{location}:{username} User Registration Attempt Failed");
                }
                await _userManager.AddToRoleAsync(user, "Customer");
                return Created("login", new { result.Succeeded });
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex.ToString());
                return StatusCode(500, "Some thing went wrong please contact to administrator");
            }
        }
        //<summary>
       //User Login Endpoint
        // </summary>
        // <param name = "dto" ></ param >

       [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserDto dto)
        {
            var location = GetControllerActionName();
            try
            {
                var username = dto.EmailAddress;
                var password = dto.Password;
                _loggerService.LogInfo($"{location}: Login Attempt from user {username} ");
                var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

                if (result.Succeeded)
                {
                    _loggerService.LogInfo($"{location}: {username} Successfully Authenticated");
                    var user = await _userManager.FindByEmailAsync(username);
                    _loggerService.LogInfo($"{location}: Generating Token");
                    var tokenString = await GenerateJsonToken(user);
                    return Ok(new { token = tokenString });
                }
                _loggerService.LogInfo($"{location}: {username} Not Authenticated");
                return Unauthorized(dto);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        private async Task<string> GenerateJsonToken(IdentityUser user)
        {
            var securtyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securtyKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(a => new Claim(ClaimsIdentity.DefaultRoleClaimType, a)));
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(_config["Jwt:Issuer"]
                , _config["Jwt:Issuer"],
                claims, null,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GetControllerActionName()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller}-{action}";
        }
        private ObjectResult InternalError(string message)
        {
            _loggerService.LogError(message);

            return StatusCode(500, "Some thing went wrong please contact to administrator");
        }
    }
}
