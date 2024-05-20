namespace NGOAPP;

public class SpeakerView : BaseViewModel
{
    public string Name { get; set; }
    public string Bio { get; set; }
    public List<Media> Images { get; set; } 
    public int EventId { get; set; }
}
