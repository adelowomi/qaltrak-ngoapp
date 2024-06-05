using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class Status : BaseModel<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}

enum Statuses
{
    Active = 1,
    Inactive = 2,
    Deleted = 3,
    Ongoing = 4,
    Completed = 5,
    Upcoming = 6,
    Published = 7,
    Pending = 8,
    Past = 9,
}
