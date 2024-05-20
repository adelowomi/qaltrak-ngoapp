namespace NGOAPP;

public class PublishEventModel
{
    public Guid EventId { get; set; }
    public Guid EventTypeId { get; set; }
    public Guid EventCategoryId { get; set; }
    public Guid EventSubCategoryId { get; set; }
    public bool PublishNow { get; set; }
    public DateTime? PublishDate { get; set; }
    public bool IsPrivate { get; set; }
}
