using Microsoft.AspNetCore.Identity;
using System;

namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Bu sınıf IdentityUser sınıfından kalıtım alır ve bu sayede projemize .Net Core Identity yapısını entegre etmiş oluruz
    /// Sisteme giriş yapacak tüm kullanıcılar bu sınıf ile yönetilecek
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
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


    }
}
