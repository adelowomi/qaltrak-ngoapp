﻿using System.ComponentModel.DataAnnotations.Schema;
using NGOAPP.Models.AppModels;
using NGOAPP.Models.IdentityModels;

namespace NGOAPP;

public class Event : BaseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? CoverImage { get; set; }

    // Option 1: Separate Longitude and Latitude properties
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

    // Option 2: Custom class for GeoLocation (if you need more details)
    // public GeoLocation GeoLocation { get; set; }

    // Use List<string> for a flexible collection of image URLs
    public List<string> Images { get; set; } = new List<string>();

    public decimal EventPrice { get; set; } // Use decimal for precise monetary values
    public string? EventContact { get; set; }
    public int? TotalCapacity { get; set; } // Use int? for nullable values
    public ICollection<EventTicket> EventTicket { get; set; } 
    // // Use int? for nullable foreign key references (assuming Country and Continent tables)
    // public int? CountryId { get; set; }
    // public int? ContinentId { get; set; }
    public int? NumberOfVolunteersNeeded { get; set; }
    public bool AttendeesCanVolunteer { get; set; }
    public string? QuestionsForAttendees { get; set; }
    public List<string>? Tags { get; set; }
    public Guid? EventTypeId { get; set; }
    public EventType EventType { get; set; }
    public Guid? EventCategoryId { get; set; }
    public EventCategory EventCategory { get; set; }
    public int? EventSubCategoryId { get; set; }
    public EventSubCategory EventSubCategory { get; set; }
    public DateTime? PublishDate { get; set; } = DateTime.Now;
    public bool? IsPrivate { get; set; }
    public bool? IsPublished { get; set; }
    public ICollection<Location> Locations { get; set; }
    public ICollection<Schedule> Schedules { get; set; }
    public ICollection<Contact> Contacts { get; set; }
    // the user who created the event
    public Guid? UserId { get; set; }
    public User User { get; set; }
    public Guid? GroupId { get; set; }
    public Group Group { get; set; }
    public int? StatusId { get; set; } = (int)Statuses.Upcoming;
    [ForeignKey(nameof(StatusId))]
    public Status Status { get; set; }
}
