using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NGOAPP;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventController : StandardControllerBase
{
    private readonly IEventService _eventService;
    private readonly PagingOptions _defaultPagingOptions;

    public EventController(IEventService eventService, IOptions<PagingOptions> defaultPagingOptions)
    {
        _eventService = eventService;
        _defaultPagingOptions = defaultPagingOptions.Value;
    }

    [HttpPost("create", Name = nameof(CreateEvent))]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 500)]
    public async Task<ActionResult<StandardResponse<EventView>>> CreateEvent([FromBody] CreateEventModel model)
    {
        return Result(await _eventService.CreateEvent(model));
    }

    [HttpPost("details", Name = nameof(AddEventDetails))]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 500)]
    public async Task<ActionResult<StandardResponse<EventView>>> AddEventDetails([FromBody] EventDetailsModel model)
    {
        return Result(await _eventService.AddEventDetails(model));
    }

    [HttpPost("tickets", Name = nameof(AddEventTickets))]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 500)]
    public async Task<ActionResult<StandardResponse<EventView>>> AddEventTickets([FromBody] CreateEventTicketModel model)
    {
        return Result(await _eventService.AddEVentTickets(model));
    }

    [HttpPost("orderform", Name = nameof(AddEventOrderFormDetails))]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 500)]
    public async Task<ActionResult<StandardResponse<EventView>>> AddEventOrderFormDetails([FromBody] EventOrderFormModel model)
    {
        return Result(await _eventService.AddEventOrderFormDetails(model));
    }

    [HttpPost("publish", Name = nameof(PublishEvent))]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 500)]
    public async Task<ActionResult<StandardResponse<EventView>>> PublishEvent([FromBody] PublishEventModel model)
    {
        return Result(await _eventService.PublishEvent(model));
    }

    [HttpGet("{eventId}", Name = nameof(GetEventById))]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 500)]
    public async Task<ActionResult<StandardResponse<EventView>>> GetEventById(Guid eventId)
    {
        return Result(await _eventService.GetEventById(eventId));
    }

    [HttpGet("list",Name = nameof(ListEvents))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<EventView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<EventView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<EventView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<EventView>>>> ListEvents([FromQuery] PagingOptions _options, [FromQuery] EventFilterOptions filterOptions)
    {
        _options.Replace(_defaultPagingOptions);
        return Result(await _eventService.ListEvents(_options, filterOptions));
    }

    [HttpPost("register/{eventId}/{eventTicketId}", Name = nameof(RegisterToAttendEvent))]
    [ProducesResponseType(typeof(StandardResponse<TicketView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<TicketView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<TicketView>), 500)]
    public async Task<ActionResult<StandardResponse<TicketView>>> RegisterToAttendEvent(EventRegistrationModel model)
    {
        return Result(await _eventService.RegisterToAttendEventOrVolunteer(model));
    }

    [HttpPost("unregister/{eventId}/{eventTicketId}", Name = nameof(UnregisterFromEvent))]
    [ProducesResponseType(typeof(StandardResponse<TicketView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<TicketView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<TicketView>), 500)]
    public async Task<ActionResult<StandardResponse<TicketView>>> UnregisterFromEvent(EventRegistrationModel model)
    {
        return Result(await _eventService.UnregisterFromEventOrVolunteer(model));
    }

    [HttpGet("tickets", Name = nameof(ListUserTickets))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<TicketView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<TicketView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<TicketView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<TicketView>>>> ListUserTickets([FromQuery] PagingOptions _options)
    {
        _options.Replace(_defaultPagingOptions);
        return Result(await _eventService.ListUserTickets(_options));
    }

    [HttpPut("update", Name = nameof(UpdateEvent))]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 500)]
    public async Task<ActionResult<StandardResponse<EventView>>> UpdateEvent([FromBody] UpdateEventModel model)
    {
        return Result(await _eventService.UpdateEvent(model));
    }

    [HttpPut("details", Name = nameof(UpdateEventDetails))]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 200)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 400)]
    [ProducesResponseType(typeof(StandardResponse<EventView>), 500)]
    public async Task<ActionResult<StandardResponse<EventView>>> UpdateEventDetails([FromBody] UpdateEventDetailsModel model)
    {
        return Result(await _eventService.UpdateEventDetails(model));
    }

    [HttpPut("schedule", Name = nameof(UpdateEventSchedule))]
    [ProducesResponseType(typeof(StandardResponse<bool>), 200)]
    [ProducesResponseType(typeof(StandardResponse<bool>), 400)]
    [ProducesResponseType(typeof(StandardResponse<bool>), 500)]
    public async Task<ActionResult<StandardResponse<bool>>> UpdateEventSchedule([FromBody] List<UpdateScheduleModel> model)
    {
        return Result(await _eventService.UpdateEventSchedule(model));
    }

    [HttpPut("contacts", Name = nameof(UpdateEventContacts))]
    [ProducesResponseType(typeof(StandardResponse<bool>), 200)]
    [ProducesResponseType(typeof(StandardResponse<bool>), 400)]
    [ProducesResponseType(typeof(StandardResponse<bool>), 500)]
    public async Task<ActionResult<StandardResponse<bool>>> UpdateEventContacts([FromBody] List<UpdateContactModel> model)
    {
        return Result(await _eventService.UpdateEventContacts(model));
    }

    [HttpPut("orderform", Name = nameof(UpdateEventOrderFormDetails))]
    [ProducesResponseType(typeof(StandardResponse<bool>), 200)]
    [ProducesResponseType(typeof(StandardResponse<bool>), 400)]
    [ProducesResponseType(typeof(StandardResponse<bool>), 500)]
    public async Task<ActionResult<StandardResponse<bool>>> UpdateEventOrderFormDetails([FromBody] UpdateEventOrderFormModel model)
    {
        return Result(await _eventService.UpdateEventOrderFormDetails(model));
    }

    [HttpPut("locations", Name = nameof(UpdateEventLocations))]
    [ProducesResponseType(typeof(StandardResponse<bool>), 200)]
    [ProducesResponseType(typeof(StandardResponse<bool>), 400)]
    [ProducesResponseType(typeof(StandardResponse<bool>), 500)]
    public async Task<ActionResult<StandardResponse<bool>>> UpdateEventLocations([FromBody] List<UpdateLocationModel> model)
    {
        return Result(await _eventService.UpdateEventLocations(model));
    }

    [HttpGet("{eventId}/speakers", Name = nameof(ListEventSpeakers))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<SpeakerView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<SpeakerView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<SpeakerView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<SpeakerView>>>> ListEventSpeakers(Guid eventId, [FromQuery] PagingOptions _options)
    {
        _options.Replace(_defaultPagingOptions);
        return Result(await _eventService.ListEventSpeaker(eventId, _options));
    }

    [HttpGet("{eventId}/contacts", Name = nameof(ListEventContacts))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<ContactView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<ContactView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<ContactView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<ContactView>>>> ListEventContacts(Guid eventId, [FromQuery] PagingOptions _options)
    {
        _options.Replace(_defaultPagingOptions);
        return Result(await _eventService.ListEventContacts(eventId, _options));
    }

    [HttpGet("{eventId}/locations", Name = nameof(ListEventLocations))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<LocationView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<LocationView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<LocationView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<LocationView>>>> ListEventLocations(Guid eventId, [FromQuery] PagingOptions _options)
    {
        _options.Replace(_defaultPagingOptions);
        return Result(await _eventService.ListEventLocations(eventId, _options));
    }

    [HttpGet("{eventId}/schedules", Name = nameof(ListEventSchedules))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<ScheduleView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<ScheduleView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<ScheduleView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<ScheduleView>>>> ListEventSchedules(Guid eventId, [FromQuery] PagingOptions _options)
    {
        _options.Replace(_defaultPagingOptions);
        return Result(await _eventService.ListEventSchedules(eventId, _options));
    }

    [HttpGet("{eventId}/sessions", Name = nameof(ListEventSessions))]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<SessionView>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<SessionView>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<PagedCollection<SessionView>>), 500)]
    public async Task<ActionResult<StandardResponse<PagedCollection<SessionView>>>> ListEventSessions(Guid eventId, [FromQuery] PagingOptions _options)
    {
        _options.Replace(_defaultPagingOptions);
        return Result(await _eventService.ListEventSessions(eventId, _options));
    }
}
