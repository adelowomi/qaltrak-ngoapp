﻿namespace NGOAPP;

public class GroupView : BaseViewModel
{
    public string Name { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Email { get; set; }
    public string? Mission { get; set; }
    public string? Commitment { get; set; }
    public DateTime DateCreated { get; set; }
    public int TotalNumberOfFollowers { get; set; }
    public int TotalEvents { get; set; }
    public int TotalSessions { get; set; }
    public bool IsFollowing { get; set; }
}
