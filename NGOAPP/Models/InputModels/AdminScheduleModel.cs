namespace NGOAPP;

public class AdminScheduleModel
{
    public string Title { get; set; }
    public int NotificationIntervalInMinutes { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Image { get; set; }
    public bool IsPrivate { get; set; }
    public string Color { get; set; }
}
