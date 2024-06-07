using NGOAPP.Models.AppModels;
using NGOAPP.Models.IdentityModels;

namespace NGOAPP;

public class EventVolunteer : BaseModel
{
    public Guid EventId { get; set; }
    public Event Event { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

}
