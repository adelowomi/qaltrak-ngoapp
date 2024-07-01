

using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace NGOAPPTESTS;

public class EventServiceTests
{
    private readonly IBaseRepository<Event> _eventRepository;
    private readonly IBaseRepository<Location> _locationRepository;
    private readonly IBaseRepository<Schedule> _scheduleRepository;
    private readonly IBaseRepository<Contact> _contactRepository;
    private readonly IBaseRepository<Session> _sessionRepository;
    private readonly IBaseRepository<Speaker> _speakerRepository;
    private readonly IBaseRepository<EventTicket> _eventTicketRepository;
    private readonly IBaseRepository<Ticket> _ticketRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBaseRepository<EventVolunteer> _eventVolunteerRepository;
    private readonly IPostmarkHelper _postmarkHelper;
    private readonly IBaseRepository<User> _userRepository;
    private readonly EventService _eventService;
    public EventServiceTests()
    {
        _eventRepository = Substitute.For<IBaseRepository<Event>>();
        _locationRepository = Substitute.For<IBaseRepository<Location>>();
        _scheduleRepository = Substitute.For<IBaseRepository<Schedule>>();
        _contactRepository = Substitute.For<IBaseRepository<Contact>>();
        _sessionRepository = Substitute.For<IBaseRepository<Session>>();
        _speakerRepository = Substitute.For<IBaseRepository<Speaker>>();
        _eventTicketRepository = Substitute.For<IBaseRepository<EventTicket>>();
        _mapper = Substitute.For<IMapper>();
        _ticketRepository = Substitute.For<IBaseRepository<Ticket>>();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _eventVolunteerRepository = Substitute.For<IBaseRepository<EventVolunteer>>();
        _postmarkHelper = Substitute.For<IPostmarkHelper>();
        _userRepository = Substitute.For<IBaseRepository<User>>();

        _eventService = new EventService(
            _eventRepository,
            _locationRepository,
            _scheduleRepository,
            _contactRepository,
            _sessionRepository,
            _speakerRepository,
            _eventTicketRepository,
            _mapper,
            _ticketRepository,
            _httpContextAccessor,
            _eventVolunteerRepository,
            _postmarkHelper,
            _userRepository
        );
    }

    [Fact]
    public async Task CreateEvent_ValidInput_ReturnsSuccessResponse()
    {
        // Arrange
        var model = new CreateEventModel { /* Set valid properties for the model */ };
        var expectedEvent = new Event { /* Set expected properties for the event */ };
        var expectedEventView = new EventView { /* Set expected properties for the event view */ };

        _eventRepository.CreateAndReturn(Arg.Any<Event>()).Returns(expectedEvent);
        _eventRepository.Query().Include(Arg.Any<Expression<Func<Event, object>>>()).FirstOrDefault(Arg.Any<Expression<Func<Event, bool>>>()).Returns(expectedEvent);
        _mapper.Map<EventView>(Arg.Any<Event>()).Returns(expectedEventView);

        // Act
        var result = await _eventService.CreateEvent(model);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("Event created successfully", result.Message);
        Assert.Equal(expectedEventView, result.Data);
    }

    [Fact]
    public async Task CreateEvent_InvalidInput_ReturnsErrorResponse()
    {
        // Arrange
        var model = new CreateEventModel
        {
        };

        // Act
        var result = await _eventService.CreateEvent(model);

        // Assert
        Assert.False(result.Status);
        Assert.Equal("Invalid input", result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task UpdateEvent_ExistingEvent_ReturnsUpdatedEvent()
    {
        // Arrang

        var existingEvent = new Event { Id = Guid.NewGuid(), Title = "Old Title", Description = "Old Description" };
        var model = new UpdateEventModel { EventId = existingEvent.Id, Title = "New Title", Description = "New Description" };

        _eventRepository.GetById(existingEvent.Id).Returns(existingEvent);


        // Act
        var result = await _eventService.UpdateEvent(model);

        // Assert
        Assert.True(result.Status);
        Assert.Equal("Event updated successfully", result.Message);
        Assert.Equal(model.Title, result.Data.Title);
        Assert.Equal(model.Description, result.Data.Description);
    }

    [Fact]
    public async Task UpdateEvent_NonExistingEvent_ReturnsError()
    {
        // Arrange
        var model = new UpdateEventModel { EventId = Guid.NewGuid(), Title = "New Title", Description = "New Description" };

        _eventRepository.GetById(model.EventId).Returns((Event)null);

        // Act
        var result = await _eventService.UpdateEvent(model);

        // Assert
        Assert.False(result.Status);
        Assert.Equal("Event not found", result.Message);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

}

