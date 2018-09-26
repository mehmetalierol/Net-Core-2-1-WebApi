using Company.Application.Common.Dto;

namespace Company.Application.Dto
{
    /// <summary>
    /// Identity alt yapısını kullanarak oluşturduğumuz ApplicationRole entity miz ile gerekli CRUD işlemleri yapacağız
    /// Bu Dto ise veritabanı ile etkileşimin son anına kadar verileri saklama ve proje içerisinde kullanma görevlerini icra edecek.
    /// Ben IdentityRole base sınıfını açarak içinde bulunan propertyleri ve bizim sonradan eklediğimiz propertyleri bu dto içerisine aynı isimler ile oluşturdum
    /// Diğer dto ların her birine yazdığım gibi burada da property isimleri ApplicationUser ımız ile aynı olmalı.
    /// Burada dikkat edilmesi gereken husus şu, ApplicationRole sınıfımız IdentityRole sınıfından kalıtım aldığı için EntityBase sınıfımızdan kalıtım alamaz
    /// Ancak dto için DtoBase sınıfını kullanabiliriz.
    /// </summary>
    public class ApplicationRoleDto : DtoBase
    {
        /// <summary>
        /// IdentityRole base sınıfından gelen name propertysini aynı iismle dto ya ekledik
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Bizim sonradan eklediğimiz açıklama property si
        /// </summary>
        public string Description { get; set; }
    }
}