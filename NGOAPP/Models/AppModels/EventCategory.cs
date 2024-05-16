using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class EventCategory : BaseModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<EventSubCategory> SubCategories { get; set; }
}
