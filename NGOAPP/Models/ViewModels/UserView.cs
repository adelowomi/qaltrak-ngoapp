namespace NGOAPP;

public class UserView
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? FullName => $"{FirstName} {LastName}";
    public string? OtherNames { get; set; }
    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
    public List<string>? DeviceId { get; set; } // List of device IDs as strings
    public string? Gender { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? StateOrProvince { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Department { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string> Roles { get; set; }
    public DateTime DateCreated { get; set; }
}
