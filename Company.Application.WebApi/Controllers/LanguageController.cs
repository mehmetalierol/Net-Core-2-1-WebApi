using Company.Application.Common.Api;
using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Company.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Company.Application.WebApi.Controllers
{
    /// <summary>
    /// diller tablosu ile ilgili işlemleri içeren sınıf
    /// </summary>
    [ApiController]
    [Route("Language")]
    public class LanguageController : ApiBase<Language, LanguageDto, LanguageController>, ILanguageController
    {
        #region Constructor

        public LanguageController(IServiceProvider service) : base(service)
        {
        }

        #endregion Constructor

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
                _uow.SaveChanges(false);
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

        /// <summary>
        /// Unit of work 'ün çalışması ve kayıtların veritabanına ulaşması için Add,Update,Delete metotlarını override ediyoruz
        /// Bu bir zorunluluk değil eğer unitofwork'ü ApiBase içerisinde savechanges yapacak şekilde kullanırsanız bu metotları override etmek zorunda kalmazsınız
        /// Ancak o zaman unit of work mantığı boş yere bu sisteme eklenmiş gibi olacak 
        /// Update metodunda da duplicate kontrolü yapabilirsiniz.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// 
        public override ApiResult<LanguageDto> Update([FromBody] LanguageDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges(true);
            return result;
        }

        public override ApiResult<string> Delete([FromBody] LanguageDto item)
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
    }
}