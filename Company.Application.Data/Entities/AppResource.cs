using Company.Application.Common.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Çoklu dil alt yapısına sahip projemizin sözlüğü görevini görecek olan sınıfımız.
    /// Kelimelerin hangi diller ne anlama geldiği bu sınıf üzerinde tutulacak
    /// </summary>
    public class AppResource : EntityBase
    {
        /// <summary>
        /// Key propertysi her kelime için tanımlanacak bir referans ismi tutacak
        /// </summary>
        [Required(ErrorMessage = "Key is required")]
        public string Key { get; set; }

        /// <summary>
        /// LangId bu kelimenin hangi dil için kayıt edildiğini tutacak
        /// </summary>
        public Guid LanguageId { get; set; }

        /// <summary>
        /// Dile ait detaylar
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Value ise verilen key in belirlenen dil için çevirisini tutacak
        /// </summary>
        [Required(ErrorMessage = "Value is required")]
        public string Value { get; set; }
    }
}
