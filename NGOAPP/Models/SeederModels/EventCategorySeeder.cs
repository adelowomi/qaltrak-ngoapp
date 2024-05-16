using Newtonsoft.Json;

namespace NGOAPP;

public class EventCategorySeeder
{
    private readonly Context _context;

    public EventCategorySeeder(Context context)
    {
        _context = context;
    }

    public void SeedData()
    {
        // Get current directory
        var currentDirectory = Directory.GetCurrentDirectory();
        var json = File.ReadAllText($"{currentDirectory}/seeddata.json");
        var data = JsonConvert.DeserializeObject<JsonStructure>(json);

        foreach (var eventCategory in data.EventCategoriesAndSubCategories)
        {
            CreateEventCategory(eventCategory);
            CreateEventSubCategories(eventCategory);
        }

        _context.SaveChanges();
    }

    private void CreateEventCategory(EventCategoryJson eventCategoryJson)
    {
        if (!_context.EventCategories.Any(sp => sp.Name == eventCategoryJson.Category))
        {
            var newEventCategory = new EventCategory
            {
                Name = eventCategoryJson.Category
            };
            _context.EventCategories.Add(newEventCategory);
        }
        _context.SaveChanges();
    }

    private void CreateEventSubCategories(EventCategoryJson eventCategoryJson)
    {
        var category = _context.EventCategories.FirstOrDefault(sp => sp.Name == eventCategoryJson.Category);

        foreach (var subCategory in eventCategoryJson.SubCategories)
        {
            if (!_context.EventSubCategories.Any(sp => sp.Name == subCategory))
            {
                var newEventSubCategory = new EventSubCategory
                {
                    Name = subCategory,
                    EventCategoryId = category.Id
                };
                _context.EventSubCategories.Add(newEventSubCategory);
            }
        }
    }
}

public class JsonStructure
{
    public List<string> EventTypes { get; set; }
    public List<EventCategoryJson> EventCategoriesAndSubCategories { get; set; }
}

public class EventCategoryJson
{
    public string Category { get; set; }
    public List<string> SubCategories { get; set; }
}

