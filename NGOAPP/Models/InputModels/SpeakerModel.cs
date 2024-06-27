namespace NGOAPP;

public class SpeakerModel
{
    public string Name { get; set; }
    public string Bio { get; set; }
    public string ImageUrl { get; set; }
    public Guid? SessionId { get; set; }
}

public class UpdateSpeakerModel
{
    public Guid? SpeakerId { get; set;}
    public string Name { get; set; }
    public string Bio { get; set; }
    public string ImageUrl { get; set; }
}