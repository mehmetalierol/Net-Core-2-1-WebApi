using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;

namespace Company.Application.WebApi.Interfaces
{
    public interface ILanguageController : IApiController<LanguageDto, Language>
    {
    
    }
}
