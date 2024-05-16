namespace NGOAPP;

public class LocationTypeSeeder
{
    private readonly Context _context;

    public LocationTypeSeeder(Context context)
    {
        _context = context;
    }

    public void SeedData()
    {
        foreach (int app in Enum.GetValues(typeof(LocationTypes)))
        {
            if (!_context.LocationTypes.Any(sp => sp.Name == Enum.GetName(typeof(LocationTypes), app)))
            {

                var locationType = new LocationType
                {
                    Name = Enum.GetName(typeof(LocationTypes), app)
                };
                _context.LocationTypes.Add(locationType);
            }
        }
        _context.SaveChanges();
    }
}
