using Company.Application.Common.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Müşterilerin bilgilerin, tutan sınıf
    /// </summary>
    public class Customer : EntityBase
    {
        /// <summary>
        /// Müşteri adı
        /// </summary>
        /// String ValueType larda required eklemezsek o property nin null olabileceği anlamına gelir bu nedenle required ekledik
        /// Aynı zamanda StringLenght ile girilmesi gereken maximum ve minimum karakter aralığını beliriyoruz
        /// Bu şartlar sağlanmazsa ModelState içerisinde bulunan Error listesine aşağıdaki ErrorMessage lar eklenecektir.
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Name Should be minimum 3 characters and a maximum 100 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Müşteri soyadı
        /// </summary>
        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Surname Should be minimum 3 characters and a maximum 100 characters")]
        public string Surname { get; set; }

        /// <summary>
        /// Müşterinin hangi firmaya bağlı olduğu bilgisi
        /// </summary>
        [Required(ErrorMessage = "Organization is required")]
        [DisplayName(nameof(Organization))]
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Müşterinin bağlı olduğu firmanın tüm bilgileri
        /// Aynı zamanda veritabanında relation oluşmasını sağlayacak.
        /// Relationları Fluent API vasıtasıyla ayrı bir mapping dosyası oluşturarak da tanımlayabiliriz. 
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// Müşterinin mail adresi
        /// </summary>
        [Required(ErrorMessage = "Mail Address is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Mail Address Should be minimum 3 characters and a maximum 100 characters")]
        [DataType(DataType.EmailAddress)]
        public string MailAdress { get; set; }

        /// <summary>
        /// Müşterinin telefon numarası
        /// </summary>
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        /// <summary>
        /// Müşteriye projenin ilerleyen dönemlerinde bir dashboard vs vermek istersek diye birde şifre alanı bırakıyoruz
        /// </summary>
        public string PasswordHash { get; set; }
    }
}
