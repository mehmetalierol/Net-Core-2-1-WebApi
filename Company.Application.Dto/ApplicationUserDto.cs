using Company.Application.Common.Dto;
using System;
using System.Collections.Generic;

namespace Company.Application.Dto
{
    /// <summary>
    /// Identity alt yapısını kullanarak oluşturduğumuz ApplicationUser entity miz ile gerekli CRUD işlemleri yapacağız
    /// Bu Dto ise veritabanı ile etkileşimin son anına kadar verileri saklama ve proje içerisinde kullanma görevlerini icra edecek.
    /// Ben IdentityUser base sınıfını açarak içinde bulunan propertyleri ve bizim sonradan eklediğimiz propertyleri bu dto içerisine aynı isimler ile oluşturdum
    /// Ek olarak Kullanıcı rollerinin isimlerini de 
    /// Diğer dto ların her birine yazdığım gibi burada da property isimleri ApplicationUser ımız ile aynı olmalı.
    /// Burada dikkat edilmesi gereken husus şu, ApplicationUser sınıfımız IdentityUser sınıfından kalıtım aldığı için EntityBase sınıfımızdan kalıtım alamaz
    /// Ancak dto için DtoBase sınıfını kullanabiliriz. 
    /// </summary>
    public class ApplicationUserDto : DtoBase
    {
        /// <summary>
        /// Kullanıcının email adresi onaylandımı bilgisi
        /// Bu alan mail adresi üzerinden doğrulama yaparken kullanılır
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Kullanıcının email adresi
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password dto ile aldığımız string formattaki parolayı UserManager sınıfı sayesinde şifreleyerek veritabanına yazacağız
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Kullanıcıya ait telefon numarası
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Telefon numarası onaylı mı değil mi kontrolü, sms ile doğrulama yapmak için kullanılır
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Bizim sonradan eklediğimiz unvan propertysi
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Sonradan eklediğimiz dil Id si
        /// </summary>
        public Guid LanguageId { get; set; }

        /// <summary>
        /// Kullanıcının diline ait detaylar
        /// </summary>
        public LanguageDto Language { get; set; }

        /// <summary>
        /// Kullanıcının rollerinin listesi
        /// </summary>
        public List<ApplicationRoleDto> UserRoles { get; set; }
    }
}
