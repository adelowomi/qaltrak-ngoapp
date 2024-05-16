﻿namespace NGOAPP;

public class SessionModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Location { get; set; }
    public string LocationDescription { get; set; }
    public List<SpeakerModel> Speakers { get; set; }
}
