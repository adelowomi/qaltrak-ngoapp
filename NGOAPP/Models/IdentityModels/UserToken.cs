using System;
using Microsoft.AspNetCore.Identity;

namespace NGOAPP.Models.IdentityModels
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public UserToken()
        {
        }
    }
}

