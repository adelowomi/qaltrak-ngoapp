using Newtonsoft.Json;

namespace NGOAPP;

public class EventTypeSeeder
{
    private readonly Context _context;

    public EventTypeSeeder(Context context)
    {
        _context = context;
    }


    public void SeedData()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var json = File.ReadAllText($"{currentDirectory}/seeddata.json");
        var data = JsonConvert.DeserializeObject<JsonStructure>(json);
        foreach (var eventType in data.EventTypes)
        {
            if (!_context.EventTypes.Any(sp => sp.Name == eventType))
            {
                var newEventType = new EventType
                {
                    Name = eventType
                };
                _context.EventTypes.Add(newEventType);
            }
        }
        _context.SaveChanges();
    }
}
