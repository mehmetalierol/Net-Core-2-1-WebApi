using Company.Application.Common.Dto;

namespace Company.Application.Dto
{
    /// <summary>
    /// Veritabanımızda bulunan firmaların üzerinde CRUD (Create, Read, Update, Delete) işlemler için organization entity mizi kullanacağız.
    /// Bu Dto ise veritabanı ile etkileşimin son anına kadar verileri saklama ve proje içerisinde kullanma görevlerini icra edecek.
    /// Entity lerimiz ve dto larımız arasında veri transferini ise AutoMapper yardımı ile yapacağız.
    /// Aşağıdaki propertylerin adları organization entity içindeki propertyle ile aynı olmalı, yoksa mapper içerisinde profiller oluştururken özel kurallar yazmamız gerekir.
    /// İsimler aynı olursa AutoMapper otomatik olarak hangi property nin Dto daki hangi propertye eşit olduğunu anlayacak ve atamaları yapacak.
    /// </summary>
    public class OrganizationDto : DtoBase
    {
        /// <summary>
        /// Firmanın Adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Firmanın vergi numarası
        /// </summary>
        public string TaxNumber { get; set; }

        /// <summary>
        /// Firmanın vergi dairesi
        /// </summary>
        public string TaxOffice { get; set; }

        /// <summary>
        /// Firmanın adres bilgisi
        /// </summary>
        public string Address { get; set; }
    }
}