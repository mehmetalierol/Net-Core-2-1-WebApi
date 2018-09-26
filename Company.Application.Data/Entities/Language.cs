using Company.Application.Common.Data;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Uygulamamızda desteklediğimiz dilleri tutan sınıfımız
    /// </summary>
    public class Language : EntityBase
    {
        /// <summary>
        /// Dil adı
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        /// <summary>
        /// Dil kodu. Örneğin TR-tr, EN-us .. vs
        /// </summary>
        [Required(ErrorMessage = "Culture is required")]
        public string Culture { get; set; }
    }
}