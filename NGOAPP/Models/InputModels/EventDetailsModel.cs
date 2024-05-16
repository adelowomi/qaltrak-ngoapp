using System.ComponentModel.DataAnnotations;

namespace NGOAPP;

public class EventDetailsModel
{
    [Required]
    public Guid EventId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<LocationModel> Locations { get; set; }
    public List<ScheduleModel> Schedules { get; set; }
    public List<ContactModel> Contacts { get; set; }
}
