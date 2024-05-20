namespace NGOAPP;

public class ContactView : BaseViewModel
{
    public string Name { get; set; }
    public string? Description { get; set; } // Description field might not be applicable here, adjust based on your data
    public string? Department { get; set; }

    // Allow phone numbers to be nullable strings
    public string? Phone { get; set; }
    public string? AltPhoneNumber1 { get; set; }
    public string? AltPhoneNumber2 { get; set; }

    // Allow email to be a nullable string
    public string? Email { get; set; }

    public string Image { get; set; }
    public Guid EventId { get; set; }
    public Event Event { get; set; }
    public Guid? UserId { get; set; }
    public UserView User { get; set; }
}
