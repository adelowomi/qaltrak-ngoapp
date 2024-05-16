namespace NGOAPP;

public class TicketTypeSeeder
{
    private readonly Context _context;

    public TicketTypeSeeder(Context context)
    {
        _context = context;
    }

    public void SeedData()
    {
        foreach (int app in Enum.GetValues(typeof(TicketTypes)))
        {
            if (!_context.TicketTypes.Any(sp => sp.Name == Enum.GetName(typeof(TicketTypes), app)))
            {

                var ticketType = new TicketType
                {
                    Name = Enum.GetName(typeof(TicketTypes), app)
                };
                _context.TicketTypes.Add(ticketType);
            }
        }
        _context.SaveChanges();
    }
}
