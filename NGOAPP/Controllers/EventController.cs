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
}
