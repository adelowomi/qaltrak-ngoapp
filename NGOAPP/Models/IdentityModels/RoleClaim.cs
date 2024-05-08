using System;
using Microsoft.AspNetCore.Identity;

namespace NGOAPP.Models.IdentityModels
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public RoleClaim()
        {
        }
    }
}

