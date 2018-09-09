using Microsoft.AspNetCore.Identity;
namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Bu sınıf IdentityRole sınıfından kalıtım alır
    /// Sistemde bulunan tüm roller bu sınıf ile kontrol edilecek
    /// </summary>
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// Mevcut IdentityRole propertylerine ek propertyler eklemek istiyorsak bu alanda istediğimiz gibi tanımlama yapabiliriz.
        /// </summary>
        public string Description { get; set; }
    }
}
