using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class Media : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public Guid? EventId { get; set; }
    public Guid? UserId { get; set; }
}
