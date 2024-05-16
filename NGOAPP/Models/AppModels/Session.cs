namespace NGOAPP;

public class Session
{
    public string Topic { get; set; }
    public string Description { get; set; }
    public int DurationTime { get; set; } // Assuming duration is in minutes or seconds (adjust if needed)
    public Guid ScheduleId { get; set; }
    public Guid EventId { get; set; }
    public Guid LocationId { get; set; }
    public string YoutubeLiveLink { get; set; }
    public string YoutubeLink { get; set; }

    // Use DateTime for timestamps
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    // Use ICollection<T> for a flexible collection type (replace T with actual speaker class)
    public ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();
}
