namespace NGOAPP;

public class LocationView : BaseViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Media> Images { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string AddressLine { get; set; }
    public string? LocationUrl { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public int? LocationTypeId { get; set; }
    public BaseViewModelI Type { get; set; }
    public Guid EventId { get; set; }
}
