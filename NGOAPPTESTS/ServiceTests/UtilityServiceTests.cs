namespace NGOAppTests.ServiceTests;

public class UtilityServiceTests
{
    private readonly IBaseRepository<LocationType> _locationTypeRepository;
    private readonly IBaseRepository<TicketType> _ticketTypeRepository;
    private readonly IBaseRepository<EventType> _eventTypeRepository;
    private readonly IBaseRepository<EventCategory> _eventCategoryRepository;
    private readonly IBaseRepository<EventSubCategory> _eventSubcategoryRepository;
    private readonly UtilityService _utilityService;

    public UtilityServiceTests()
    {
        _locationTypeRepository = Substitute.For<IBaseRepository<LocationType>>();
        _ticketTypeRepository = Substitute.For<IBaseRepository<TicketType>>();
        _eventTypeRepository = Substitute.For<IBaseRepository<EventType>>();
        _eventCategoryRepository = Substitute.For<IBaseRepository<EventCategory>>();
        _eventSubcategoryRepository = Substitute.For<IBaseRepository<EventSubCategory>>();

        _utilityService = new UtilityService(
            _locationTypeRepository,
            _ticketTypeRepository,
            _eventTypeRepository,
            _eventCategoryRepository,
            _eventSubcategoryRepository
        );
    }

    [Fact]
    public async Task GetLocationTypes_ShouldReturnListOfLocationTypes()
    {
        // Arrange
        var locationTypes = new List<LocationType>
        {
            new LocationType { Id = 1, Name = "Location Type 1" },
            new LocationType { Id = 2, Name = "Location Type 2" }
        };
        _locationTypeRepository.GetAll().Returns(locationTypes);

        // Act
        var result = await _utilityService.GetLocationTypes();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(locationTypes.Count, result.Data.Count);
        Assert.Equal(locationTypes.Select(l => l.Name), result.Data.Select(d => d.Name));
    }

    [Fact]
    public async Task GetTicketTypes_ShouldReturnListOfTicketTypes()
    {
        // Arrange
        var ticketTypes = new List<TicketType>
        {
            new TicketType { Id = 1, Name = "Ticket Type 1" },
            new TicketType { Id = 2, Name = "Ticket Type 2" }
        };
        _ticketTypeRepository.GetAll().Returns(ticketTypes);

        // Act
        var result = await _utilityService.GetTicketTypes();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ticketTypes.Count, result.Data.Count);
        Assert.Equal(ticketTypes.Select(t => t.Name), result.Data.Select(d => d.Name));
    }

    [Fact]
    public async Task GetEventTypes_ShouldReturnListOfEventTypes()
    {
        // Arrange
        var eventTypes = new List<EventType>
        {
            new EventType { Id = Guid.NewGuid(), Name = "Event Type 1" },
            new EventType { Id = Guid.NewGuid(), Name = "Event Type 2" }
        };
        _eventTypeRepository.GetAll().Returns(eventTypes);

        // Act
        var result = await _utilityService.GetEventTypes();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventTypes.Count, result.Data.Count);
        Assert.Equal(eventTypes.Select(e => e.Name), result.Data.Select(d => d.Name));
    }

    [Fact]
    public async Task GetEventCategories_ShouldReturnListOfEventCategories()
    {
        // Arrange
        var eventCategories = new List<EventCategory>
        {
            new EventCategory { Id = Guid.NewGuid(), Name = "Event Category 1" },
            new EventCategory { Id = Guid.NewGuid(), Name = "Event Category 2" }
        };
        _eventCategoryRepository.GetAll().Returns(eventCategories);

        // Act
        var result = await _utilityService.GetEventCategories();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventCategories.Count, result.Data.Count);
        Assert.Equal(eventCategories.Select(e => e.Name), result.Data.Select(d => d.Name));
    }

    [Fact]
    public async Task GetEventSubCategories_ShouldReturnListOfEventSubCategories()
    {
        // Arrange
        var eventCategoryId = Guid.NewGuid();
        var eventSubCategories = new List<EventSubCategory>
        {
            new EventSubCategory { Id = 1, Name = "Event SubCategory 1", EventCategoryId = eventCategoryId },
            new EventSubCategory { Id = 2, Name = "Event SubCategory 2", EventCategoryId = eventCategoryId }
        };
        _eventSubcategoryRepository.GetAll().Returns(eventSubCategories);

        // Act
        var result = await _utilityService.GetEventSubCategories(eventCategoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventSubCategories.Count, result.Data.Count);
        Assert.Equal(eventSubCategories.Select(e => e.Name), result.Data.Select(d => d.Name));
    }
}