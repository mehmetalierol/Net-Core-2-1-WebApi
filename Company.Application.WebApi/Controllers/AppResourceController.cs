using Company.Application.Common.Api;
using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System.Linq;

namespace Company.Application.WebApi.Controllers
{
    [ApiController]
    [Route("AppResource")]
    public class AppResourceController : ApiBase<AppResource, AppResourceDto, AppResourceController>
    {
        private readonly IMemoryCache _memoryCache;
        public AppResourceController(IServiceProvider service) : base(service)
        {
            _memoryCache = service.GetService<IMemoryCache>();
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
            _uow.SaveChanges(false);
            return result;
        }

        public override ApiResult<AppResourceDto> Update([FromBody] AppResourceDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] AppResourceDto item)
        {
            var result = base.Delete(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> DeleteById(Guid id)
        {
            var result = base.DeleteById(id);
            _uow.SaveChanges(true);
            return result;
        }

        /// <summary>
        /// İstenen dile ait tüm çeviriler 1 gün boyunca sunucu belleğine 
        /// 2 dakika boyunca ise response cache olarak ekleniyor. 
        /// Guid tipindeki dil Id si ise memory cache için anahtar görevi görüyor.
        /// </summary>
        /// <param name="LanguageId">İstenen dil</param>
        /// <returns></returns>
        [HttpGet("GetResourcesByLanguage")]
        //response cache'in bu şekilde uygulanması bir AOP örneğidir, client bazında ve 120 saniye boyunca 
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Client)]
        public ApiResult<List<AppResourceDto>> GetResourcesByLanguage (Guid LanguageId)
        {
            //Önce bellekte bu veri var mı diye anahtar ile kontrol ediyoruz var ise veritabanına hiç gitmeyeceğiz
            if (!_memoryCache.TryGetValue(LanguageId, out List<AppResourceDto> ResourceList))
            {
                //bellekte veri yok ise veritabanında verileri alıp cacheleme yapıyoruz
                ResourceList = GetQueryable().Where(x=>x.LanguageId == LanguageId).ToList().Select(x => Mapper.Map<AppResourceDto>(x)).ToList();
                //cache süresi ve önemi 
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSlidingExpiration(TimeSpan.FromDays(1));
                _memoryCache.Set(LanguageId, ResourceList, cacheEntryOptions);
            }

            return new ApiResult<List<AppResourceDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Resource founded.",
                Data = ResourceList
            };
        }

        /// <summary>
        /// memory cache içinde bulunan dile ait verilerin silinmesi işlemini yapan metot
        /// </summary>
        /// <param name="LanguageId"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult ClearCache(Guid LanguageId)
        {
            _memoryCache.Remove(LanguageId);
            return new ApiResult
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Cache removed from the server.",
                Data = null
            };
        }
    }
}
