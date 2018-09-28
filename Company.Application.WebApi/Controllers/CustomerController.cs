using Company.Application.Common.Api;
using Company.Application.Common.Api.Base;
using Company.Application.Data.Entities;
using Company.Application.Dto;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Company.Application.WebApi.Controllers
{
    /// <summary>
    /// Müşteri tablosu işlemleri için kullanılacak sınıf.
    /// </summary>
    [ApiController]
    [Route("Customer")]
    public class CustomerController : ApiBase<Customer, CustomerDto, CustomerController>
    {
        public CustomerController(IServiceProvider service) : base(service)
        {
        }

        /// <summary>
        /// Include işlemi olduğu için Find metodunu override ediyoruz.
        /// </summary>
        /// <param name="id">İstenen kaydın Id bilgisi</param>
        /// <returns></returns>
        public override ApiResult<CustomerDto> Find(Guid id)
        {
            return new ApiResult<CustomerDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "User founded",
                Data = Mapper.Map<Customer, CustomerDto>(GetQueryable().Include(x => x.Organization).FirstOrDefault(x => x.Id == id))
            };
        }

        /// <summary>
        /// Include işlemi olduğu için getall metodunu override ediyoruz.
        /// </summary>
        /// <returns></returns>
        public override ApiResult<List<CustomerDto>> GetAll()
        {
            return new ApiResult<List<CustomerDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "User founded",
                Data = GetQueryable().Include(x=>x.Organization).ToList().Select(x => Mapper.Map<CustomerDto>(x)).ToList()
            };
        }

        /// <summary>
        /// Unit of work 'ün çalışması ve kayıtların veritabanına ulaşması için Add,Update,Delete metotlarını override ediyoruz
        /// Bu bir zorunluluk değil eğer unitofwork'ü ApiBase içerisinde savechanges yapacak şekilde kullanırsanız bu metotları override etmek zorunda kalmazsınız
        /// Ancak o zaman unit of work mantığı boş yere bu sisteme eklenmiş gibi olacak 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override ApiResult<CustomerDto> Add([FromBody] CustomerDto item)
        {
            var result = base.Add(item);
            _uow.SaveChanges();
            return result;
        }

        public override ApiResult<CustomerDto> Update([FromBody] CustomerDto item)
        {
            var result = base.Update(item);
            _uow.SaveChanges();
            return result;
        }

        public override ApiResult<string> Delete([FromBody] CustomerDto item)
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
