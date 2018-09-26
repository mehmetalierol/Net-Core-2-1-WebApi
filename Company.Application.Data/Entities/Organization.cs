using Company.Application.Common.Data;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Firmaların bilgilerinin tutalacağı sınıf
    /// </summary>
    public class Organization : EntityBase
    {
        /// <summary>
        /// Firmanın adı. Sadece Name property sini zorunlu tututyoruz diğerleri optional olarak kalacak.
        /// </summary>
        [Required(ErrorMessage = "Key is required")]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Name Should be minimum 3 characters and a maximum 100 characters")]
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
        /// Firmanın adresi
        /// </summary>
        public string Address { get; set; }
    }
}