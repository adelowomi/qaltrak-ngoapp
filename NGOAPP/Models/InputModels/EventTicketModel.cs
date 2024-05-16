namespace NGOAPP;

public class EventTicketModel
{
    public Guid EvetId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int MaxTicketPerOrder { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsSoldOut { get; set; }
}
