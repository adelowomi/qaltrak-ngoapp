using System;
using Microsoft.AspNetCore.Identity;

namespace NGOAPP.Models.IdentityModels
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public UserLogin()
        {
        }
    }
}

