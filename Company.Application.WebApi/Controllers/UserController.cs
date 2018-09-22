using Company.Application.Common.Api;
using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Company.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Company.Application.WebApi.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ApiBase<ApplicationUser, ApplicationUserDto, LanguageController>, IUserController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IServiceProvider service) : base(service)
        {
            _userManager = service.GetService<UserManager<ApplicationUser>>();
        }

        public override async Task<ApiResult<ApplicationUserDto>> AddAsync(ApplicationUserDto item)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = Mapper.Map<ApplicationUserDto, ApplicationUser>(item);
                user.CreateDate = DateTime.UtcNow;
                //user.Language = null;
                identityResult = await _userManager.CreateAsync(user, password: user.PasswordHash);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<ApplicationUserDto>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Added Successfully." : sbErrors.ToString()),
                    Data = (identityResult.Succeeded ? Mapper.Map<ApplicationUser, ApplicationUserDto>(GetQueryable().Include(x => x.Language).FirstOrDefault(x=>x.Id == user.Id)) : null)
                };

                _logger.LogInformation($"Add User with userid:{user.Id } mail:{item.Email} username:{item.UserName} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"User Add : mail:{item.Email} username:{item.UserName} result:Error - {ex.Message}");

                return new ApiResult<ApplicationUserDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }
    }
}
