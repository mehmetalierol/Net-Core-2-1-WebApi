using Company.Application.Common.Enums;
using System;

namespace Company.Application.Common.Dto
{
    public class DtoBase
    {
        /// <summary>
        /// Bu sınıfı inherit alacak derived class'larımızın içereceği alanları yazıyoruz
        /// Böylece tekrarlı kod yazmanın önüne geçmiş olacağız. 
        /// </summary>
        public Guid Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? Creator { get; set; }
        public AppStatus? Status { get; set; }
    }
}
