namespace NGOAPP;

public class ScheduleView : BaseViewModel
{
    public string Name { get; set; }
    public List<SessionView> Sessions { get; set; }
    public Guid EventId { get; set; }
}
