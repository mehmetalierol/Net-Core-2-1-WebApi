using Company.Application.Common.Dto;

namespace Company.Application.Dto
{
    /// <summary>
    /// Veritabanımızda bulunan diller üzerinde CRUD (Create, Read, Update, Delete) işlemler için language entity mizi kullanacağız.
    /// Bu Dto ise veritabanı ile etkileşimin son anına kadar verileri saklama ve proje içerisinde kullanma görevlerini icra edecek.
    /// Entity lerimiz ve dto larımız arasında veri transferini ise AutoMapper yardımı ile yapacağız.
    /// Aşağıdaki propertylerin adları language entity içindeki propertyle ile aynı olmalı, yoksa mapper içerisinde profiller oluştururken özel kurallar yazmamız gerekir.
    /// İsimler aynı olursa AutoMapper otomatik olarak hangi property nin Dto daki hangi propertye eşit olduğunu anlayacak ve atamaları yapacak.
    /// </summary>
    public class LanguageDto : DtoBase
    {
        /// <summary>
        /// Dilin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dilin culture kodu (Örnek: TR-tr, EN-us ... vs)
        /// </summary>
        public string Culture { get; set; }
    }
}