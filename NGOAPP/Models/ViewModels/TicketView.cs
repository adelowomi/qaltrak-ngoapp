namespace NGOAPP;

public class TicketView
{
    public Guid? EventId { get; set; }
    public EventView Event { get; set; }
    public decimal Price { get; set; } // Use decimal for precise monetary values
    public Guid? EventTicketId { get; set; }
    public EventTicketView EventTicket { get; set; }
    public bool Free { get; set; }
    public Guid? UserId { get; set; }
    public UserView User { get; set; }
}
