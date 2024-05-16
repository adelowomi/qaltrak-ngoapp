using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class LocationType : BaseModel<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}

enum LocationTypes
{
    InPerson = 1,
    Virtual = 2,
    Other
}