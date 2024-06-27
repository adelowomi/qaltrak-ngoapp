namespace NGOAPP;

public class EventOrderFormModel
{
    public Guid EventId { get; set; }
    public int? NumberOfVolunteersNeeded { get; set; }
    public bool AttendeesCanVolunteer { get; set; }
    public string? QuestionsForAttendees { get; set; }
}

public class UpdateEventOrderFormModel
{
    public Guid EventId { get; set; }
    public int? NumberOfVolunteersNeeded { get; set; }
    public bool AttendeesCanVolunteer { get; set; }
    public string? QuestionsForAttendees { get; set; }
}
