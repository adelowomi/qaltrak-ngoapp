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
        public string? FullName => $"{FirstName} {LastName}";
        public string? OtherNames { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public int? UserId { get; set; } // Assuming UserId is an integer for simplicity
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string>? DeviceId { get; set; } // List of device IDs as strings
        public string? Gender { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? StateOrProvince { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Department { get; set; }
    }
}

