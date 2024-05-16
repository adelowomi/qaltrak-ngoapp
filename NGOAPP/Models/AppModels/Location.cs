using NGOAPP.Models.AppModels;

namespace NGOAPP;

public class Location : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }

    // Use List<string> for a flexible collection of image URLs
    public ICollection<Media> Images { get; set; }

    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string AddressLine { get; set; }

    // Can choose one of the following approaches for GeoLocation:

    // Option 1: Separate Longitude and Latitude properties
    // public double GeoLocationLongitude { get; set; }
    // public double GeoLocationLatitude { get; set; }

    // Option 2: Custom class for GeoLocation (if you need more details)
    // public GeoLocation GeoLocation { get; set; }

    // Option 3: Ignore GeoLocation if not used

    public string LocationUrl { get; set; }

    // Use List<string> for a flexible collection of tags
    public List<string> Tags { get; set; } = new List<string>();

    // You can use an enum for specific types (bus, session, taxi)
    public LocationType Type { get; set; }

    public int EventId { get; set; }
}
