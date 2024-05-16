using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class EventSubCategory : BaseModel<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid EventCategoryId { get; set; }
    public EventCategory EventCategory { get; set; }
}
