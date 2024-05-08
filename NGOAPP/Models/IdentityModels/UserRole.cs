using System;
using Microsoft.AspNetCore.Identity;

namespace NGOAPP.Models.IdentityModels
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public UserRole()
        {
        }
    }
}

