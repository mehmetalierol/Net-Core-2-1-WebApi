using Company.Application.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Bu sınıf IdentityUser sınıfından kalıtım alır ve bu sayede projemize .Net Core Identity yapısını entegre etmiş oluruz
    /// Sisteme giriş yapacak tüm kullanıcılar bu sınıf ile yönetilecek
    /// Generic tip olarak Guid veriyoruz bu tip ile Identity Id bilgisinin hangi formatta saklanacağını belirlemiş oluyor
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        private AppStatus status;
        private DateTime createdDate;

        /// <summary>
        /// Mevcut IdentityUser base i içerisinde bulunan property lere ek olarak bu alanda çeşitli propertyler yazabiliriz
        /// Normal şartlar title için ayrı bir sınıf oluşturmak ve burada o sınıfı çağırmak daha mantıklı olur ancak biz örnek olması açsıından string tipinde ekledik
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Bu kullanıcının hangi dil ile sistemi kullandığını gösteren property
        /// </summary>
        public Guid LanguageId { get; set; }

        /// <summary>
        /// Kullanıcının sistemi kullandığı dilin detayları ve aynı zamanda database ilişkisi kurmak için kullanıyoruz
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Identity alt yapısında bulunan usermanager bize istediğimiz userın sadece role isimlerini veriyor
        /// biz entityframework ile kullanıcının rolüne dair tüm verileri almak için aşağıdaki tanımlama ile ilişki oluşturuyoruz
        /// daha sonra include ederek bu ilişkisel yapı üzerinden (ki zaten veritabanında ilişkiler tanımlı) select işlemleri yapacağız
        /// </summary>
        public List<ApplicationUserRole> UserRoles { get; set; }

        public DateTime? CreateDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                createdDate = value ?? DateTime.UtcNow;
            }
        }

        public Guid? Creator { get; set; }

        public AppStatus? Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value ?? AppStatus.Aktif;
            }
        }
    }
}
