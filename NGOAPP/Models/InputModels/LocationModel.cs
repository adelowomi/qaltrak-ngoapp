namespace NGOAPP;

public class LocationModel
{
    public int LocationTypeId { get; set; }
    public string Name { get; set; }
    public string AddressLine { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Description { get; set; }
}

public class UpdateLocationModel
{
    public Guid EventId { get; set; }
    public Guid? LocationId { get; set; }
    public int LocationTypeId { get; set; }
    public string Name { get; set; }
    public string AddressLine { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Description { get; set; }
}
