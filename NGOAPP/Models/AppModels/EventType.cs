using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class EventType : BaseModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
}