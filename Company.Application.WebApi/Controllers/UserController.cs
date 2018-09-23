using Company.Application.Common.Api;
using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Company.Application.WebApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Company.Application.WebApi.Controllers
{
    /// <summary>
    /// Identity Alt yapısını kullanarak kullanıcı işlemleri yapacağımız sınıf
    /// </summary>
    [ApiController]
    [Route("User")]
    public class UserController : ApiBase<ApplicationUser, ApplicationUserDto, LanguageController>, IUserController
    {
        #region Variables
        /// <summary>
        /// Identity alt yapısı içerisinde bulunan UserManager sınıfı ile kullanıcı işlemlerini yapacağız.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Contructor
        public UserController(IServiceProvider service) : base(service)
        {
            _userManager = service.GetService<UserManager<ApplicationUser>>();
        }
        #endregion

        /// <summary>
        /// Base de bulunan Add metodu virtual olarak tanımlandığından dolayı override edilebilir 
        /// diğer entitylerden farklı olarak ApplicationUser üzerinde yapılacak ekleme işlemi UserManager sınıf kullanılarak yapılacak
        /// Bu nedenle base de bulunan Add metodunu override ediyoruz.
        /// </summary>
        /// <param name="item">Eklenmesi istenen kullanıcıya ait bilgiler</param>
        /// <returns></returns>
        public override async Task<ApiResult<ApplicationUserDto>> AddAsync(ApplicationUserDto item)
        {
            //UserManager ile yapılacak işlemler geriye IdentityResult tipinde bir sınıf ile değer döndürür, sonucu yakalamak için bir adet IdentityResult tanımlıyoruz.
            var identityResult = new IdentityResult();
            //Çıkacak hataları tutacağım bir string builder tanımladım ve senaryolara göre append işlemi yapacağız
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                //Dto tipinde gelen bilgiler mapper ile entity tipine dönüştürülüyor. Çünkü UserManager IdentityUser'dan kalıtım almış bir sınıf ile işlemler yapabilir
                var user = Mapper.Map<ApplicationUserDto, ApplicationUser>(item);
                //eklenecek kullanıcının create date bilgisini atıyoruz
                user.CreateDate = DateTime.UtcNow;
                //UserManager.CreateAsync metoduna ilk parametre olarak IdentityUser'dan kalıtım almış ApplicationUser sınıfımızın bir instance'ı olan referans tipimizi veriyoruz ve daha sonra ikinci parametre olarak şifresini veriyoruz.
                identityResult = await _userManager.CreateAsync(user, password: user.PasswordHash);
                //Hata var ise String Join yaparak IEnumarable tipinde tutulan Error listesinin içindeki elemanları hata için oluşturudğumuz string builder'ımıza ekliyoruz.
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                //Dönüş yapacağımız ApiResult tipinde modelimizi oluşturuyoruz.
                var result = new ApiResult<ApplicationUserDto>
                {
                    //ekleme işlemi başarılı ise http200 ile başarılı değil ise gelen verilerde bir sorun olduğunuz belirtmek için 406 kabul edilemez yanıtını vereceğiz. Bu yanıtların tipleri size kalmış ben bu ikisini kullanmayı tercih ettim.
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    //Mesaj kısmında başarılı ise bir mesaj gönderiyoruz değil ise hataları içeren stringimizi gönderiyoruz.
                    Message = (identityResult.Succeeded ? "User Added Successfully." : sbErrors.ToString()),
                    //işlem başarılı ise eklenen kullanıcı dil modeli ile birlikte dönüyoruz başarısız ise null dönüyoruz.
                    Data = (identityResult.Succeeded ? Mapper.Map<ApplicationUser, ApplicationUserDto>(GetQueryable().Include(x => x.Language).FirstOrDefault(x=>x.Id == user.Id)) : null)
                };

                //logger ile sonucu logluyoruz.
                _logger.LogInformation($"Add User with userid:{user.Id } mail:{item.Email} username:{item.UserName} result:{result.Message}");
                //hazırladığımız result değişkenimizi dönüyoruz.
                return result;
            }
            catch (Exception ex)
            {
                //Hata çıkarsa logluyoruz.
                _logger.LogError($"User Add : mail:{item.Email} username:{item.UserName} result:Error - {ex.Message}");

                //geriye hatayı içeren bir dönüş yapıyoruz.
                return new ApiResult<ApplicationUserDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        public override async Task<ApiResult<ApplicationUserDto>> UpdateAsync([FromBody] ApplicationUserDto item)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString());

                _logger.LogInformation($"Update User : userid:{user.Id} oldusername:{item.UserName} oldphonenumber:{item.PhoneNumber} oldtitle:{user.Title}");

                user.UserName = item.UserName;
                user.PhoneNumber = item.PhoneNumber;
                user.Title = item.Title;
                user.LanguageId = item.LanguageId;

                identityResult = await _userManager.UpdateAsync(user);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<ApplicationUserDto>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Update User Success" : sbErrors.ToString()),
                    Data = Mapper.Map<ApplicationUser, ApplicationUserDto>(GetQueryable().Include(x => x.Language).Include(x => x.UserRoles).FirstOrDefault(x => x.Id == item.Id))
                };

                _logger.LogInformation($"Update User : userid:{user.Id} newusername:{item.UserName} newphonenumber:{item.PhoneNumber} newtitle:{user.Title} result :{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Update User : newusername:{item.UserName} newphonenumber:{item.PhoneNumber} newtitle:{item.Title} result:{ex.Message}");
                return new ApiResult<ApplicationUserDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }
    }
}
