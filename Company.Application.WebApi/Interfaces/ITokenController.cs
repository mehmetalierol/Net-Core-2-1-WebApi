using Company.Application.Common.Api;
using Company.Application.WebApi.VM;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.Application.WebApi.Interfaces
{
    public interface ITokenController
    {
        Task<ApiResult> LoginAsync([FromBody] LoginModel loginModel);
    }
}
