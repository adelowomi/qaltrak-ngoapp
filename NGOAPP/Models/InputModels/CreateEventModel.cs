﻿using System.ComponentModel.DataAnnotations;

namespace NGOAPP;

public class CreateEventModel
{
    public List<string> Images { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } 
    [Required]
    public Guid GroupId { get; set; }
}
