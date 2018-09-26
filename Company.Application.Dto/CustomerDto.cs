using Company.Application.Common.Dto;
using System;

namespace Company.Application.Dto
{
    /// <summary>
    /// Veritabanımızda bulunan müşteriler üzerinde CRUD (Create, Read, Update, Delete) işlemler için customer entity mizi kullanacağız.
    /// Bu Dto ise veritabanı ile etkileşimin son anına kadar verileri saklama ve proje içerisinde kullanma görevlerini icra edecek.
    /// Entity lerimiz ve dto larımız arasında veri transferini ise AutoMapper yardımı ile yapacağız.
    /// Aşağıdaki propertylerin adları customer entity içindeki propertyle ile aynı olmalı, yoksa mapper içerisinde profiller oluştururken özel kurallar yazmamız gerekir.
    /// İsimler aynı olursa AutoMapper otomatik olarak hangi property nin Dto daki hangi propertye eşit olduğunu anlayacak ve atamaları yapacak.
    /// </summary>
    public class CustomerDto : DtoBase
    {
        /// <summary>
        /// Müşteri adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Müşterinin soyadı
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Müşterinin bağlı olduğu firmanın Id bilgisi
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Müşterinin bağlı olduğu firmanın bilgileri
        /// </summary>
        public OrganizationDto Organization { get; set; }

        /// <summary>
        /// Müşterinin Mail adresi
        /// </summary>
        public string MailAdress { get; set; }

        /// <summary>
        /// Müşterinin Telefon numarası
        /// </summary>
        public string Phone { get; set; }
    }
}