using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NGOAPP.Models.IdentityModels
{
    public class User : IdentityUser<Guid>
    {
        [MaxLength(60)]
        public string? FirstName { get; set; }
        [MaxLength(60)]
        public string? LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        public string? OtherNames { get; set; }

    }
}

