using AutoMapper;
using Company.Application.Common.Paging;
using Company.Application.Common.Paging.Interface;
using Company.Application.Common.Repository;
using Company.Application.Common.UnitofWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Company.Application.Common.Api.Base
{
    public class ApiBase<T, TDto, TController> : ControllerBase where T : class where TDto : class where TController : ControllerBase
    {
        #region Variables
        /// <summary>
        /// Sınıf içerisinde kullanacağımız değişkenlerimizi tanımlıyoruz. Unitofwork ve generic repository private yani sadece bu sınıf için erişilebilirken logger tüm sınıflardan erişilebilsin diye public bırakıyoruz.
        /// Bu sınıfı kalıtım alan tüm sınırlarımızda loglama işlemi yapacağımız için logger public.
        /// </summary>
        private readonly IUnitofWork _uow;
        private readonly IGenericRepository<T> _repository;
        public readonly IServiceProvider _service;
        public readonly ILogger<TController> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Burada dependency injection'ı farklı şekilde kullanmaya karar verdim çünkü diğer projelerimde constructorlar içerisinde çok fazla parametre oluyor ve okunması zor hale geliyordu.
        /// IserviceProvider benim için dependency injection ile resolve işlemi görecek yani bağımlılığı service provider ile projeme enjekte edeceğim.
        /// </summary>
        /// <param name="service">bağımlılıkları eklemek için service provider kullanacağız</param>
        public ApiBase(IServiceProvider service)
        {
            //Get Service diyerek dependencylerimizi ınject ediyoruz
            _logger = service.GetService<ILogger<TController>>();
            _uow = service.GetService<IUnitofWork>();
            _repository = _uow.GetRepository<T>();
            _service = service;
        }
        #endregion

        #region GetMethods
        /// <summary>
        /// Kayıtları veritabanından bulurken bu metodu kullanacağız. Bu method içerisine aranan kaydın Id bilgisini alacak ve geriye ilgili entity ile maplenmiş dto cevabını dönecek.
        /// </summary>
        /// <param name="id">bulunacak kaydın Guid türünde Id bilgisi</param>
        /// <returns></returns>
        [HttpGet("Find")]
        public virtual ApiResult<TDto> Find(Guid id)
        {
            try
            {
                //Logger ile basit bir log yazıyoru. Ben logların mantıklı olması için zaman harcamadım siz senaryonuza göre loglamanıza karar verebilirsiniz.
                //Burada sadece ilgili tablodan hangi id ye sahip kayıt istenmiş ise onu logluyoruz.
                _logger.LogInformation("Find record from the " + typeof(T) + " table with id " + id);

                //Geriye Mapper ile mapleyerek dto dönüyoruz T bizim entitylerimiz için generic type ve TDto ise dto larımız için generic type görevi görüyor.
                return new ApiResult<TDto>
                {
                    Message = "Success",
                    StatusCode = StatusCodes.Status200OK,
                    Data = Mapper.Map<T, TDto>(_repository.Find(id))
                };
            }
            catch (Exception ex)
            {
                //Hata olması durumunda loglama yapıyoruz ve kullanıcıya HTTP durum kodlarından 500 yani internal server error dönüyoruz.
                _logger.LogInformation("Find record error from the " + typeof(T) + " table with id " + id);
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Error : " + ex.Message,
                    Data = null
                };
            }

        }

        /// <summary>
        /// İlgili tabloda bulunan tüm kayıtları seçmek için kullanılacak olan metod
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public virtual ApiResult<List<TDto>> GetAll()
        {
            try
            {
                var entities = _repository.GetAll().ToList();
                _logger.LogInformation("Getall records from the " + typeof(T) + " table");
                return new ApiResult<List<TDto>>
                {
                    Message = "Success",
                    StatusCode = StatusCodes.Status200OK,
                    Data = entities.Select(x => Mapper.Map<TDto>(x)).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Getall records error from the " + typeof(T) + " table");
                return new ApiResult<List<TDto>>
                {
                    Message = "Error : " + ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null
                };
            }

        }

        /// <summary>
        /// Sorgulanabilir ve üzerinde işlemler yapılabilir bir Queryable dönen metodumuz
        /// Burada önemli nokta herhangi bir şekilde mapper ile işimiz yok direk olarak queryable donuyoruz.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetQueryable")]
        public virtual IQueryable<T> GetQueryable()
        {
            try
            {
                _logger.LogInformation("GetQueryable from the " + typeof(T) + " table");
                return _repository.GetAll();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation("GetQueryable error from the " + typeof(T) + " table");
                return null;
            }
            
        }

        /// <summary>
        /// Sayfalanmış listeler dönmek için kullanacağımız metot
        /// </summary>
        /// <param name="pagingParams">Hangi sayfa isteniyor ve sayfada kaç kayıt listelenecek gibi bilgileri parametre olarak bekler</param>
        /// <returns></returns>
        [HttpGet("GetAllWithPaging")]
        public virtual ApiResult GetAllWithPaging(PagingParams pagingParams)
        {
            try
            {
                _logger.LogInformation("GetAllWithPaging from the " + typeof(T) + " table");
                var pagingLinks = _service.GetService<IPagingLinks<T>>();

                var model = new PagedList<T>(
                    GetQueryable(), pagingParams.PageNumber, pagingParams.PageSize);

                Response.Headers.Add("X-Pagination", model.GetHeader().ToJson());

                var outputModel = new OutputModel<T>
                {
                    Paging = model.GetHeader(),
                    Links = pagingLinks.GetLinks(model),
                    Items = model.List.Select(m => m).ToList(),
                };

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = outputModel
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("GetAllWithPaging error from the " + typeof(T) + " table");
                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Error : " + ex.Message,
                    Data = null
                };
            }
        }
        #endregion

        #region PostMethods
        /// <summary>
        /// Id ile Kayıt silmek için kullanacağımız metod
        /// </summary>
        /// <param name="id">Silinecek kaydın Id bilgisi</param>
        /// <returns></returns>
        [HttpPost("DeleteById")]
        public virtual ApiResult<string> DeleteById(Guid id)
        {
            try
            {
                _repository.Delete(id);
                _logger.LogInformation("Record deleted from the " + typeof(T) + " table with id " + id);
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Delete error for the " + typeof(T) + " table with id " + id);
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Error : " + ex.Message,
                    Data = null
                };
            }

        }

        /// <summary>
        /// Id ile Kayıt silmek için kullanacağımız metod
        /// </summary>
        /// <param name="id">Silinecek kaydın Id bilgisi</param>
        /// <returns></returns>
        [HttpPost("DeleteByIdAsync")]
        public virtual async Task<ApiResult<string>> DeleteByIdAsync(Guid id)
        {
            return DeleteById(id);
        }

        /// <summary>
        /// Model ile kayıt silmek için kullanacağımız metod
        /// </summary>
        /// <param name="item">silinecek kayda ait model</param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public virtual ApiResult<string> Delete([FromBody] TDto item)
        {
            try
            {
                _repository.Delete(Mapper.Map<T>(item));
                _logger.LogInformation("Record deleted from the " + typeof(T) + " table. Data:" + item.ToString());
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Delete error for the " + typeof(T) + " table. Data:" + item.ToString());
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Error : " + ex.Message,
                    Data = null
                };
            }

        }

        /// <summary>
        /// Model ile kayıt silmek için kullanacağımız metod
        /// </summary>
        /// <param name="item">silinecek kayda ait model</param>
        /// <returns></returns>
        [HttpPost("DeleteAsync")]
        public virtual async Task<ApiResult<string>> DeleteAsync([FromBody] TDto item)
        {
            return Delete(item);
        }

        /// <summary>
        /// Kayıt eklemek için metod
        /// </summary>
        /// <param name="item">Eklenecek kayıt</param>
        /// <returns></returns>
        [HttpPost("Add")]
        public virtual ApiResult<TDto> Add([FromBody] TDto item)
        {
            try
            {
                var TResult = _repository.Add(Mapper.Map<T>(item));
                _logger.LogInformation("Add record to the " + typeof(T) + " table. Data:" + item.ToString());
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = Mapper.Map<T, TDto>(TResult)
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Add record error to the " + typeof(T) + " table. Data:" + item.ToString());
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Error : " + ex.Message,
                    Data = null
                };
            }

        }

        /// <summary>
        /// Kayıt eklemek için metod
        /// </summary>
        /// <param name="item">Eklenecek kayıt</param>
        /// <returns></returns>
        [HttpPost("AddAsync")]
        public virtual async Task<ApiResult<TDto>> AddAsync([FromBody] TDto item)
        {
            return Add(item);
        }

        /// <summary>
        /// Kayıt güncellemek için metod
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public virtual ApiResult<TDto> Update([FromBody] TDto item)
        {
            try
            {
                var TResult = _repository.Update(Mapper.Map<T>(item));
                _logger.LogInformation("Update record to the " + typeof(T) + " table. Data:" + item.ToString());
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success",
                    Data = Mapper.Map<T, TDto>(TResult)
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Update record error to the " + typeof(T) + " table. Data:" + item.ToString());
                return new ApiResult<TDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Error : " + ex,
                    Data = null
                };
            }

        }

        /// <summary>
        /// Kayıt güncellemek için metod
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost("UpdateAsync")]
        public virtual async Task<ApiResult<TDto>> UpdateAsync([FromBody] TDto item)
        {
            return Update(item);
        }
        #endregion

        #region SaveChanges
        /// <summary>
        /// İşlemleri kaydedecek metot. Bu metot çağrılmaz ise işlemler veritabanına gitmeyecektir.
        /// </summary>
        private void Save()
        {
            _uow.SaveChanges();
        }
        #endregion
    }
}
