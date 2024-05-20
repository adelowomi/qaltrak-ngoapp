namespace NGOAPP;

public class SessionView
{
    public string? Topic { get; set; }
    public string? Description { get; set; }
    public int DurationTime { get; set; } // Assuming duration is in minutes or seconds (adjust if needed)
    public Guid ScheduleId { get; set; }
    public Guid EventId { get; set; }
    public Guid LocationId { get; set; }
    public string? YoutubeLiveLink { get; set; }
    public string? YoutubeLink { get; set; }

    // Use DateTime for timestamps
    public DateTime Start { get; set; } = DateTime.Now;
    public DateTime End { get; set; } = DateTime.Now;

    // Use ICollection<T> for a flexible collection type (replace T with actual speaker class)
    public List<SpeakerView> Speakers { get; set; } 
}
