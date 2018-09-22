using Company.Application.Common.Api;
using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Company.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Company.Application.WebApi.Controllers
{
    [Route("Language")]
    public class LanguageController : ApiBase<Language, LanguageDto, LanguageController>, ILanguageController
    {
        public LanguageController(IServiceProvider service): base(service)
        {
            
        }
    }
}
