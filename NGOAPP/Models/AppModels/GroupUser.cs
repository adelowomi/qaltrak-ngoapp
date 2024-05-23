using NGOAPP.Models.AppModels;
using NGOAPP.Models.IdentityModels;

namespace NGOAPP;

public class GroupUser : BaseModel
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string? Role { get; set; }
}
