using Company.Application.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Bu sınıf IdentityRole sınıfından kalıtım alır
    /// Sistemde bulunan tüm roller bu sınıf ile kontrol edilecek
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>
    {
        /// <summary>
        /// Mevcut IdentityRole propertylerine ek propertyler eklemek istiyorsak bu alanda istediğimiz gibi tanımlama yapabiliriz.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Bu rolü kimin oluşturduğu bilgisi
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// Rolün oluşturulma tarihi
        /// </summary>
        public DateTime CreateDate { get; set; }

        public AppStatus Status { get; set; }
    }
}
