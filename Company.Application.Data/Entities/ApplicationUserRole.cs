using Company.Application.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Company.Application.Data.Entities
{
    /// <summary>
    /// Kullanıcılar ve rollerin ilişkilendirildiği tablo. Many to many bir ilişki kurmak için 3. bir tabloya ihtiyaç duyuluyor
    /// </summary>
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        private AppStatus status;
        private DateTime createdDate;

        public Guid Id { get; set; }

        /// <summary>
        /// Kullanıcı bilgisi
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Role bilgisi
        /// </summary>
        public ApplicationRole Role { get; set; }

        public DateTime? CreateDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                createdDate = value ?? DateTime.UtcNow;
            }
        }

        public Guid? Creator { get; set; }

        public AppStatus? Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value ?? AppStatus.Aktif;
            }
        }
    }
}
