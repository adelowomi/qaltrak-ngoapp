namespace NGOAPP;

public class EventFilterOptions
{
    public Guid? EventTypeId { get; set; }
    public Guid? EventCategoryId { get; set; }
    public int? EventSubCategoryId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
