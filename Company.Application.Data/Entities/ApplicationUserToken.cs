using Microsoft.AspNetCore.Identity;
using System;

namespace Company.Application.Data.Entities
{
    public class ApplicationUserToken : IdentityUserToken<Guid>
    {
        public Guid Id { get; set; }
    }
}
