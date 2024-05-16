namespace NGOAPP;

public class StatusSeeder
{
    private readonly Context _context;

    public StatusSeeder(Context context)
    {
        _context = context;
    }

    public void SeedData()
    {
        foreach (int app in Enum.GetValues(typeof(Statuses)))
        {
            if (!_context.Statuses.Any(sp => sp.Name == Enum.GetName(typeof(Statuses), app)))
            {

                var status = new Status
                {
                    Name = Enum.GetName(typeof(Statuses), app)
                };
                _context.Statuses.Add(status);
            }
        }
        _context.SaveChanges();
    }
}
