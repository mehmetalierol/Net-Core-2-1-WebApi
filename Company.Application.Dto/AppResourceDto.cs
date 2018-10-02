using Company.Application.Common.Dto;
using System;

namespace Company.Application.Dto
{
    /// <summary>
    /// Veritabanımızda bulunan çeviriler üzerinde CRUD (Create, Read, Update, Delete) işlemler için AppResource entity mizi kullanacağız.
    /// Bu Dto ise veritabanı ile etkileşimin son anına kadar verileri saklama ve proje içerisinde kullanma görevlerini icra edecek.
    /// Entity lerimiz ve dto larımız arasında veri transferini ise AutoMapper yardımı ile yapacağız.
    /// Aşağıdaki propertylerin adları AppResource entity içindeki propertyle ile aynı olmalı, yoksa mapper içerisinde profiller oluştururken özel kurallar yazmamız gerekir.
    /// İsimler aynı olursa AutoMapper otomatik olarak hangi property nin Dto daki hangi propertye eşit olduğunu anlayacak ve atamaları yapacak.
    /// </summary>
    public class AppResourceDto : DtoBase
    {
        /// <summary>
        /// Kelime için referans key bilgisi
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// bu çevirinin hangi dilde olduğu
        /// </summary>
        public Guid LanguageId { get; set; }

        /// <summary>
        /// dile ait bilgiler
        /// </summary>
        public LanguageDto Language { get; set; }

        /// <summary>
        /// referans keyi verilen kelimenin o dildeki çevirisi
        /// </summary>
        public string Value { get; set; }
    }
}