using AutoMapper;
using Company.Application.Common.Api;
using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Company.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Company.Application.WebApi.Controllers
{
    [ApiController]
    [Route("Role")]
    public class RoleController : ApiBase<ApplicationRole, ApplicationRoleDto, RoleController>, IRoleController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(IServiceProvider service) : base(service)
        {
            _roleManager = service.GetService<RoleManager<ApplicationRole>>();
        }

        public override ApiResult<ApplicationRoleDto> Add([FromBody] ApplicationRoleDto item)
        {
            return new ApiResult<ApplicationRoleDto>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to Add a user to database",
                Data = null
            };
        }

        public override async Task<ApiResult<ApplicationRoleDto>> AddAsync([FromBody] ApplicationRoleDto item)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var role = Mapper.Map<ApplicationRoleDto, ApplicationRole>(item);
                identityResult = await _roleManager.CreateAsync(role).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<ApplicationRoleDto>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Role Added Successfully." : sbErrors.ToString()),
                    Data = identityResult.Succeeded ? Mapper.Map<ApplicationRole, ApplicationRoleDto>(role) : null
                };

                _logger.LogInformation($"Role Added with roleid:{role.Id } rolename:{role.Name} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Role Add : rolename:{item.Name} result:Error - {ex.Message}");

                return new ApiResult<ApplicationRoleDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        public override ApiResult<ApplicationRoleDto> Update([FromBody] ApplicationRoleDto item)
        {
            return new ApiResult<ApplicationRoleDto>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to update a user",
                Data = null
            };
        }

        public override async Task<ApiResult<ApplicationRoleDto>> UpdateAsync([FromBody] ApplicationRoleDto item)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var role = await _roleManager.FindByIdAsync(item.Id.ToString()).ConfigureAwait(false);

                _logger.LogInformation($"Role Update with roleid:{role.Id} oldname:{role.Name} newname:{item.Name}");

                role.Name = item.Name;

                identityResult = await _roleManager.UpdateAsync(role).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<ApplicationRoleDto>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Role Updated Successfully." : sbErrors.ToString()),
                    Data = AutoMapper.Mapper.Map<ApplicationRole, ApplicationRoleDto>(role)
                };

                _logger.LogInformation($"Role Updated roleid:{role.Id } rolename:{role.Name} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Role Update : rolename:{item.Name} result:Error - {ex.Message}");

                return new ApiResult<ApplicationRoleDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        public override ApiResult<string> Delete([FromBody] ApplicationRoleDto item)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to delete a user from database",
                Data = null
            };
        }

        public override Task<ApiResult<string>> DeleteAsync([FromBody] ApplicationRoleDto item)
        {
            return DeleteByIdAsync(item.Id);
        }

        public override ApiResult<string> DeleteById(Guid id)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to delete a user from database",
                Data = null
            };
        }

        public override async Task<ApiResult<string>> DeleteByIdAsync(Guid id)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
                identityResult = await _roleManager.DeleteAsync(role).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<string>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Role Deleted Successfully." : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"Role deleted. roleid:{id} rolename:{role.Name}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Delete role error. roleid:{id} error:{ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }
    }
}