using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Company.Application.Common.Api;
using System.Threading.Tasks;
using Company.Application.WebApi.VM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Company.Application.Data.Entities;
using System.Security.Claims;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Company.Application.Dto;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Company.Application.WebApi.Controllers
{
    [Route("Authentication")]
    public class TokenController : ControllerBase
    {
        #region Variables
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public TokenController(IServiceProvider service)
        {
            _userManager = service.GetService<UserManager<ApplicationUser>>();
            _signinManager = service.GetService<SignInManager<ApplicationUser>>();
            _config = service.GetService<IConfiguration>();
        }
        #endregion

        #region BusinessSection
        [HttpPost("LoginAsync")]
        [AllowAnonymous]
        public async Task<ApiResult> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var StatusCode = StatusCodes.Status204NoContent;
                var ResultMessage = "";
                object ResultData = "";
                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                if (user != null)
                {
                    var checkPwd = await _signinManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
                    if (checkPwd.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, "Token");
                        claimsIdentity.AddClaims(roles.Select(role => new Claim("roles", role)));

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                        _config["Tokens:Issuer"],
                        claimsIdentity.Claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);

                        var tokenHandler = new { token = new JwtSecurityTokenHandler().WriteToken(token) };

                        var dto = new TokenModel
                        {
                            Token = tokenHandler.token,
                            UserDto = Mapper.Map<ApplicationUser, ApplicationUserDto>(user)
                        };

                        ResultData = dto;
                        ResultMessage = "Token created successfully";
                        StatusCode = StatusCodes.Status200OK;
                    }
                    else
                    {
                        ResultData = null;
                        ResultMessage = "Password is not correct!";
                        StatusCode = StatusCodes.Status406NotAcceptable;
                    }
                }
                else
                {
                    ResultData = null;
                    ResultMessage = "No such user!";
                    StatusCode = StatusCodes.Status406NotAcceptable;
                }

                return new ApiResult
                {
                    StatusCode = StatusCode,
                    Message = ResultMessage,
                    Data = ResultData
                };
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }
        #endregion
    }
}
