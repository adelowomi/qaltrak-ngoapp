using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class EventTicket : BaseModel
{
    public Guid EventId { get; set; }
    public Guid TicketTypeId { get; set; }
    public int Quantity { get; set; }
    public int MaxQuantityPerOrder { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsSoldOut { get; set; }
    public bool IsDeleted { get; set; }
    public Event Event { get; set; }
    public TicketType TicketType { get; set; }
}
