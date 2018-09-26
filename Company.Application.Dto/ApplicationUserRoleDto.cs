using Company.Application.Data.Entities;
using System;

namespace Company.Application.Dto
{
    public class ApplicationUserRoleDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Kullanıcı bilgisi
        /// </summary>

        public Guid UserId { get; set; }

        /// <summary>
        /// Role bilgisi
        /// </summary>
        public ApplicationRole Role { get; set; }

        public Guid RoleId { get; set; }
    }
}