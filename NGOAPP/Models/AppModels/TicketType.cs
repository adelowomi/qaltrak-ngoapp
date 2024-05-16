using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class TicketType : BaseModel<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}

enum TicketTypes
{
    Free = 1,
    Paid = 2,
}
