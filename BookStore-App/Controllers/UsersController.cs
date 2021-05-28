
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
        public UsersController(IConfiguration configuration, ILoggerService loggerService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _loggerService = loggerService;
            _config = configuration;
        }
        ///<summary>
        ///User Login Endpoint
        /// </summary>
        /// <param name="dto"></param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserDto dto)
        {
            var location = GetControllerActionName();
            try
            {


               _loggerService.LogInfo($"{location}:Login Attempt for user :{dto.UserName}");
                var result = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);

                if (result.Succeeded)
                {
                    _loggerService.LogInfo($"{location}-{dto.UserName} Successfully Authenticated");
                    var user = await _userManager.FindByNameAsync(dto.UserName);
                    var tokenstring = await GenerateJsonToken(user);
                    return Ok(new { token= tokenstring });
                }
                _loggerService.LogInfo($"{location}:{dto.UserName} Not Authenticated");
                return Unauthorized(dto);
            }
            catch (Exception ex)
            {

                return InternalError($"{ex.Message}-{ex.InnerException}");
            }
        }
        public async Task<string> GenerateJsonToken(IdentityUser user)
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
                ,_config["Jwt:Issuer"],
                claims,null,
                expires:DateTime.Now.AddMinutes(5),
                signingCredentials:credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GetControllerActionName()
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
