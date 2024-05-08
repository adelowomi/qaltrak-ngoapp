using System;
using Microsoft.AspNetCore.Identity;

namespace NGOAPP.Models.IdentityModels
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public UserClaim()
        {
        }
    }
}

