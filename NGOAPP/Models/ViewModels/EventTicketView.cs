namespace NGOAPP;

public class EventTicketView
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public int TicketTypeId { get; set; }
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
    public BaseViewModelI TicketType { get; set; }
}
