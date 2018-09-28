using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Company.Application.WebApi.Controllers
{
    [ApiController]
    [Route("AppResource")]
    public class AppResourceController : ApiBase<AppResource, AppResourceDto, AppResourceController>
    {
        public AppResourceController(IServiceProvider service) : base(service)
        {
        }
    }
}
