namespace NGOAPP;

public class GroupDashboardView
{
    public int TotalEventsCreated { get; set; }
    public int TotalPastEvents { get; set; }
    public int TotalUpcomingEvents { get; set; }
    public int TotalAttendeeRegistered { get; set; }
    public int TotalFollowers { get; set; }
    public List<EventView> UpcomingEvents { get; set; }
    public List<EventView> TotalPerformingEventsPerResgistration { get; set; }
}
