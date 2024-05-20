using NGOAPP.Models.AppModels;
using NGOAPP.Models.IdentityModels;

namespace NGOAPP;

public class Ticket : BaseModel
{
    public Guid? EventId { get; set; }
    public Event Event { get; set; }
    public decimal Price { get; set; } // Use decimal for precise monetary values
    public Guid? EventTicketId { get; set; }
    public EventTicket EventTicket { get; set; }
    public bool Free { get; set; }
    public Guid? UserId { get; set; }
    public User User { get; set; }
    public int? TicketTypeId { get; set; }
    public TicketType TicketType { get; set; }
}
