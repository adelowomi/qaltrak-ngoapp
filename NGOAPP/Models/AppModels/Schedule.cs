using System.ComponentModel.DataAnnotations.Schema;
using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class Schedule : BaseModel
{
    public string Name { get; set; }
    public ICollection<Session> Sessions { get; set; }
    public Guid EventId { get; set; }
    // [ForeignKey(nameof(EventId))]
    // public Event Event { get; set; }
}
