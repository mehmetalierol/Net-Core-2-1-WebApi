using Microsoft.AspNetCore.Identity;
using System;

namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Kullanıcılar ve rollerin ilişkilendirildiği tablo. Many to many bir ilişki kurmak için 3. bir tabloya ihtiyaç duyuluyor
    /// </summary>
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Kullanıcı bilgisi
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Role bilgisi
        /// </summary>
        public ApplicationRole Role { get; set; }
    }
}
