namespace NGOAPP;

public class ScheduleModel
{
    public string Name { get; set; }
    public List<SessionModel> Sessions { get; set; }
}

public class UpdateScheduleModel 
{
    public Guid EventId { get; set; }
    public Guid ScheduleId { get; set; }
    public string Name { get; set; }
    public List<UpdateSessionModel> Sessions { get; set; }
}
