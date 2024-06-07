namespace NGOAPP;

public class EventRegistrationModel
{
    public Guid EventId { get; set; }
    public Guid EventTicketId { get; set; }
    public bool IsAttending { get; set; }
    public bool IsVolunteering { get; set; }
}
