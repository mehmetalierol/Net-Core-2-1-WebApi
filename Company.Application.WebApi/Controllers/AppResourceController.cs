using Company.Application.Common.Api;
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

        /// <summary>
        /// Unit of work 'ün çalışması ve kayıtların veritabanına ulaşması için Add,Update,Delete metotlarını override ediyoruz
        /// Bu bir zorunluluk değil eğer unitofwork'ü ApiBase içerisinde savechanges yapacak şekilde kullanırsanız bu metotları override etmek zorunda kalmazsınız
        /// Ancak o zaman unit of work mantığı boş yere bu sisteme eklenmiş gibi olacak 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override ApiResult<AppResourceDto> Add([FromBody] AppResourceDto item)
        {
            var result = base.Add(item);
            _uow.SaveChanges();
            return result;
        }

        public override ApiResult<AppResourceDto> Update([FromBody] AppResourceDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges();
            return result;
        }

        public override ApiResult<string> Delete([FromBody] AppResourceDto item)
        {
            var result = base.Delete(item);
            _uow.SaveChanges();
            return result;
        }

        public override ApiResult<string> DeleteById(Guid id)
        {
            var result = base.DeleteById(id);
            _uow.SaveChanges();
            return result;
        }
    }
}
