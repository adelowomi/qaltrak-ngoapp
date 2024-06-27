namespace NGOAPP;

public class SessionModel
{
    public Guid EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Location { get; set; }
    public string? LocationDescription { get; set; }
    public List<SpeakerModel> Speakers { get; set; }
    public Guid? ScheduleId { get; set; }
}

public class UpdateSessionModel
{
    public Guid? SessionId { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Location { get; set; }
    public string? LocationDescription { get; set; }
    public List<UpdateSpeakerModel> Speakers { get; set; }
    public Guid? ScheduleId { get; set; }
}
