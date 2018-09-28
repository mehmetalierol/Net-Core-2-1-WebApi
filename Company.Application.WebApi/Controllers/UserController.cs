using AutoMapper;
using Company.Application.Common.Api;
using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using Company.Application.WebApi.Interfaces;
using Company.Application.WebApi.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Application.WebApi.Controllers
{
    /// <summary>
    /// Identity Alt yapısını kullanarak kullanıcı işlemleri yapacağımız sınıf
    /// </summary>
    [ApiController]
    [Route("User")]
    [Authorize(Roles = "Admin")]
    public class UserController : ApiBase<ApplicationUser, ApplicationUserDto, UserController>, IUserController
    {
        #region Variables

        /// <summary>
        /// Identity alt yapısı içerisinde bulunan UserManager sınıfı ile kullanıcı işlemlerini yapacağız.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion Variables

        #region Contructor

        public UserController(IServiceProvider service) : base(service)
        {
            _userManager = service.GetService<UserManager<ApplicationUser>>();
        }

        #endregion Contructor

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
                identityResult = await _userManager.CreateAsync(user, password: user.PasswordHash).ConfigureAwait(false);
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
                    Data = (identityResult.Succeeded ? Mapper.Map<ApplicationUser, ApplicationUserDto>(GetQueryable().Include(x => x.Language).FirstOrDefault(x => x.Id == user.Id)) : null)
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

        /// <summary>
        /// Asenkron olan ekleme metodunun kullanılması gerektiğini bildiriyoruz.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override ApiResult<ApplicationUserDto> Add([FromBody] ApplicationUserDto item)
        {
            return new ApiResult<ApplicationUserDto>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to Add a user to database",
                Data = null
            };
        }

        /// <summary>
        /// Güncelleme işlemini async metot ile yapıyoru. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override async Task<ApiResult<ApplicationUserDto>> UpdateAsync([FromBody] ApplicationUserDto item)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString()).ConfigureAwait(false);

                _logger.LogInformation($"Update User : userid:{user.Id} oldusername:{item.UserName} oldphonenumber:{item.PhoneNumber} oldtitle:{user.Title}");

                user.UserName = item.UserName;
                user.PhoneNumber = item.PhoneNumber;
                user.Title = item.Title;
                user.LanguageId = item.LanguageId;
                user.Email = item.Email;

                identityResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
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

        /// <summary>
        /// Update işlemi için asenkron metodun kullanılması gerektiğini bildiriyoruz.
        /// </summary>
        /// <param name="item">Update edilecek kullanıcı</param>
        /// <returns></returns>
        public override ApiResult<ApplicationUserDto> Update([FromBody] ApplicationUserDto item)
        {
            return new ApiResult<ApplicationUserDto>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to Update a user",
                Data = null
            };
        }

        /// <summary>
        /// Asenkron silme işleminin kullanılması gerektiğini belirtiyoruz.
        /// </summary>
        /// <param name="id">Silinmek istenen kaydın Id bilgisi</param>
        /// <returns></returns>
        public override ApiResult<string> DeleteById(Guid id)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to delete a user from database",
                Data = null
            };
        }

        /// <summary>
        /// Silme işlemini yapmak için Id gönderilir ise bu metoddan yararlanıyoruz.
        /// </summary>
        /// <param name="id">Silinecek kaydın Id bilgisi</param>
        /// <returns></returns>
        public override async Task<ApiResult<string>> DeleteByIdAsync(Guid id)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
                identityResult = await _userManager.DeleteAsync(user).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));

                var result = new ApiResult<string>
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Deleted Successfully." : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"User deleted. userid:{id} email:{user.Email} username:{user.UserName}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Delete user error. userid:{id} error:{ex}");
                return new ApiResult<string>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }

        /// <summary>
        /// Asenkron silme işleminin kullanılması gerektiğini belirtiyoruz.
        /// </summary>
        /// <param name="item">Silinecek kayıt</param>
        /// <returns></returns>
        public override ApiResult<string> Delete([FromBody] ApplicationUserDto item)
        {
            return new ApiResult<string>
            {
                StatusCode = StatusCodes.Status406NotAcceptable,
                Message = "Please use Async methods to delete a user from database",
                Data = null
            };
        }

        /// <summary>
        /// Silme işlemini async metot ile yapıyoruz.
        /// </summary>
        /// <param name="item">Silinmek istenen kayıt</param>
        /// <returns></returns>
        public override Task<ApiResult<string>> DeleteAsync([FromBody] ApplicationUserDto item)
        {
            return DeleteByIdAsync(item.Id);
        }

        /// <summary>
        /// Kullanıcının dilini ve rollerini yüklemek için include işlemi yapıyoruz.
        /// </summary>
        /// <param name="id">İstenen kaydın Id bilgisi</param>
        /// <returns></returns>
        public override ApiResult<ApplicationUserDto> Find(Guid id)
        {
            try
            {
                var a = GetQueryable().Include(x => x.UserRoles).ThenInclude(t => t.Role).Include(y => y.Language)
                    .FirstOrDefault(x => x.Id == id);
                var dto = Mapper.Map<ApplicationUser, ApplicationUserDto>(a);

                var result = new ApiResult<ApplicationUserDto>
                {
                    StatusCode = (dto != null ? StatusCodes.Status200OK : StatusCodes.Status404NotFound),
                    Message = (dto != null ? "User Founded Successfully." : "No such user!"),
                    Data = dto
                };

                _logger.LogInformation($"Find user success username:{dto.UserName} email:{dto.Email}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Find user error! Code:{ex}");
                return new ApiResult<ApplicationUserDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Kullanıcı listesini gerekli include işlemleri yaparak döndürüyoruz. 
        /// </summary>
        /// <returns></returns>
        public override ApiResult<List<ApplicationUserDto>> GetAll()
        {
            try
            {
                var list = _userManager.Users.Include(x => x.UserRoles).ThenInclude(t => t.Role).Include(x => x.Language).Select(x => Mapper.Map<ApplicationUserDto>(x)).ToList();

                var result = new ApiResult<List<ApplicationUserDto>>
                {
                    StatusCode = (list.Count >= 1 ? StatusCodes.Status200OK : StatusCodes.Status404NotFound),
                    Message = (list.Count >= 1 ? "Users Founded Successfully." : "There is no user!"),
                    Data = list
                };

                _logger.LogInformation($"Getall users success Total user count:{list.Count}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Getall users error! Code:{ex}");
                return new ApiResult<List<ApplicationUserDto>>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// Admin yetkisine sahip kişiler kullanıcıların önceki şifrelerini bilmeseler bile şifrelerini resetleyip değiştirebilir. 
        /// Bu metot eski şifreyi sormadan şifre resetleme işlemi yapar
        /// </summary>
        /// <param name="model">Şifre değiştirme için gerekli bilgileri içeren model</param>
        /// <returns></returns>
        [HttpPost("ChangePasswordAsAdminAsync")]
        public async Task<ApiResult> ChangePasswordAsAdminAsync([FromBody] ChangePasswordModel model)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString()).ConfigureAwait(false);

                var validator = new PasswordValidator<ApplicationUser>();

                var validatorResult = await validator.ValidateAsync(_userManager, user, model.NewPassword).ConfigureAwait(false);
                sbErrors.Append(String.Join(",", validatorResult.Errors.Select(x => x.Code).ToList()));

                if (validatorResult.Succeeded)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                    identityResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Change Password Success" : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"Change Password : userid:{user.Id } mail:{user.Email} username:{user.UserName} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Change Password : id:{model.UserId} result:Error - {ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }

        /// <summary>
        /// Kullanıcıların kendi şifrelerini değiştirmeleri için bu metot kullanılır. Bu metot şifrenin değiştirilebilmesi için eski şifreyi ister.
        /// </summary>
        /// <param name="model">Şifre değiştirme için gerekli bilgileri içeren model</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("ChangePasswordAsync")]
        public async Task<ApiResult> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString()).ConfigureAwait(false);

                var oldPasswordCheck = await _userManager.CheckPasswordAsync(user, model.OldPassword).ConfigureAwait(false);

                if (oldPasswordCheck)
                {
                    var validatorResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", validatorResult.Errors.Select(x => x.Code).ToList()));
                }
                else
                {
                    sbErrors.Append("Old Password Invalid");
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "Change Password Success" : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"Change Password : userid:{user.Id } mail:{user.Email} username:{user.UserName} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Change Password : UserId:{model.UserId} Error:{ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }

        /// <summary>
        /// Kullanıcıya rol eklemek için kullanılan metot
        /// </summary>
        /// <param name="userid">Rolün ekleneceği kullanıcı Id si</param>
        /// <param name="roleid">Kullanıcıya eklenecek rolün Id bilgisi</param>
        /// <returns></returns>
        [HttpPost("AddUserRoleAsync")]
        public async Task<ApiResult> AddUserRoleAsync(Guid userid, Guid roleid)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            var roleManager = _service.GetService<RoleManager<ApplicationRole>>();
            try
            {
                var user = await _userManager.FindByIdAsync(userid.ToString()).ConfigureAwait(false);
                var role = await roleManager.FindByIdAsync(roleid.ToString()).ConfigureAwait(false);

                if (role != null)
                {
                    identityResult = await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));
                }
                else
                {
                    sbErrors.Append("No Such Role!");
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Role Added Successfully." : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"User Role Add to userid:{userid} role:{roleid} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"User Role Add to userid:{userid} role:{roleid} result:Error - {ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }

        /// <summary>
        /// Kullanıcının rollerini silmek için kullanılır
        /// </summary>
        /// <param name="userid">Rolü silinecek kullanıcı Id si</param>
        /// <param name="roleid">Kullanıcıdan silinecek rol Id si</param>
        /// <returns></returns>
        [HttpPost("DeleteUserRoleAsync")]
        public async Task<ApiResult> DeleteUserRoleAsync(Guid userid, Guid roleid)
        {
            var identityResult = new IdentityResult();
            var sbErrors = new StringBuilder("Errors:");
            var roleManager = _service.GetService<RoleManager<ApplicationRole>>();
            try
            {
                var user = await _userManager.FindByIdAsync(userid.ToString()).ConfigureAwait(false);
                var role = await roleManager.FindByIdAsync(roleid.ToString()).ConfigureAwait(false);

                if (role != null)
                {
                    identityResult = await _userManager.RemoveFromRoleAsync(user, role.Name).ConfigureAwait(false);
                    sbErrors.Append(String.Join(",", identityResult.Errors.Select(x => x.Code).ToList()));
                }
                else
                {
                    sbErrors.Append("No Such Role!");
                }

                var result = new ApiResult
                {
                    StatusCode = (identityResult.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status406NotAcceptable),
                    Message = (identityResult.Succeeded ? "User Role Deleted Successfully." : sbErrors.ToString()),
                    Data = $"IsSucceeded:{identityResult.Succeeded}"
                };

                _logger.LogInformation($"User Role Delete to userid:{userid} role:{roleid} result:{result.Message}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"User Role Delete to userid:{userid} role:{roleid} result:Error - {ex.Message}");

                return new ApiResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Error:{ex.Message}",
                    Data = sbErrors.ToString()
                };
            }
        }
    }
}