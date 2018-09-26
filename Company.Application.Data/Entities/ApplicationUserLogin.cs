using Microsoft.AspNetCore.Identity;
using System;

namespace Company.Application.Data.Entities
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public Guid Id { get; set; }
    }
}