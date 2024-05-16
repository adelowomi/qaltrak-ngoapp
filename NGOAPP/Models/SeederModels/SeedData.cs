namespace NGOAPP;

public class SeedData
    {
        private readonly Context _context;

        public SeedData(Context context)
        {
            _context = context;
        }

        public void SeedInitialData()
        {
            new StatusSeeder(_context).SeedData();
            new EventCategorySeeder(_context).SeedData();
            new EventTypeSeeder(_context).SeedData();
            new LocationTypeSeeder(_context).SeedData();
            new TicketTypeSeeder(_context).SeedData();
        }
    }
