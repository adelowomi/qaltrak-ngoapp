using NGOAPP.Models.AppModels;
using NGOAPP.Models.IdentityModels;

namespace NGOAPP;

public class Group : BaseModel
{
    public string Name { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Email { get; set; }
    public string? Mission { get; set; }
    public string? Commitment { get; set; }
    public ICollection<Event> Events { get; set; }
    public Guid UserId { get; set; }
    public User CreatedBy { get; set; }
    public ICollection<GroupUser> GroupUsers { get; set; }
    public ICollection<GroupFollow> Followers { get; set; }
}
