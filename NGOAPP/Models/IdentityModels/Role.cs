using System;
using Microsoft.AspNetCore.Identity;

namespace NGOAPP.Models.IdentityModels
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
        }
    }
}

