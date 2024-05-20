namespace NGOAPP;

public class ContactModel
{
    public string Name { get; set; }
    public string Description { get; set; } 
    public string Department { get; set; }
    public string? Phone { get; set; }
    public string AltPhoneNumber1 { get; set; }
    public string AltPhoneNumber2 { get; set; }
    public string? Email { get; set; }
    public Guid? UserId { get; set; }
    public string Image { get; set; }
}
