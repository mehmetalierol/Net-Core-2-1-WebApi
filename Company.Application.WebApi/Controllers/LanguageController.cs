using Company.Application.Common.Api;
using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Company.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Company.Application.WebApi.Controllers
{
    /// <summary>
    /// diller tablosu ile ilgili işlemleri içeren sınıf
    /// </summary>
    [Route("Language")]
    public class LanguageController : ApiBase<Language, LanguageDto, LanguageController>, ILanguageController
    {
        #region Constructor
        public LanguageController(IServiceProvider service): base(service)
        {
            
        }
        #endregion

        /// <summary>
        /// Add metodunu override ederek çift kayıt kontrolü yaptık
        /// dikkat edilmesi gerekn nokta savechanges diyerek işlemleri kaydetmiş olmamız. 
        /// Eğer savechanges yapmazsanız geriye eklendi dönseniz bile veritabanına kayıt eklenmeyecektir.
        /// Begin new transaction işlemi yapmadık çünkü savechanges içerisinde transaction yoksa yeni oluştur kuralı eklemiştik.
        /// </summary>
        /// <param name="item">eklenecek dile ait bilgiler</param>
        /// <returns></returns>
        public override ApiResult<LanguageDto> Add([FromBody] LanguageDto item)
        {
            //çift kayıt kontrolü yapan metot false dönerse işlemler yapılacak.
            if (!CheckDublicateLanguage(item.Culture))
            {
                //base deki ekleme işlemi yapılıyor
                var result = base.Add(item);
                //sıralı işlemler olmadığı için save changes diyerek veritabanında değişiklikler yapılıyor
                base._uow.SaveChanges();
                //geriye base de bulunan add metodundan dönen değer döndürülüyor
                return result;
            }
            else
            {
                //çift kayıt varsa ekleme işlemi yapılmıyor ve kullanıcıya cevap dönülüyor
                return new ApiResult<LanguageDto>
                {
                    StatusCode = StatusCodes.Status406NotAcceptable,
                    Message = "This culture is exist!",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Çift kayıt kontrolü yapan metodumuz ben burada culture property'sini kontrol etmeyi tercih ettim.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public bool CheckDublicateLanguage(string culture)
        {
            var result = GetQueryable().Where(x => x.Culture == culture).ToList().Count;
            return result > 0;
        }
    }
}
