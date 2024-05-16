using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class Ticket : BaseModel
{
    public int EventId { get; set; }
    public decimal Price { get; set; } // Use decimal for precise monetary values
    public string TicketId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Free { get; set; }
    public Guid? UserId { get; set; }
    public int? TicketTypeId { get; set; }
    public TicketType TicketType { get; set; }

    // External payment integration is complex and data structure depends on the specific integration
    // You might need a separate class or interface to represent it
    // public object ExternalPaymentIntegration { get; set; } // Placeholder, adjust based on actual integration

}
