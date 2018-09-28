using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Company.Application.WebApi.Controllers
{
    [ApiController]
    [Route("Organization")]
    public class OrganizationController : ApiBase<Organization, OrganizationDto, OrganizationController>
    {
        public OrganizationController(IServiceProvider service) : base(service)
        {
        }
    }
}
