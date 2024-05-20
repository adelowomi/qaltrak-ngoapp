using Microsoft.AspNetCore.Mvc;

namespace NGOAPP;

[ApiController]
[Route("api/[controller]")]
public class EventController : StandardControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
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
    public async Task<ActionResult<StandardResponse<EventView>>> AddEventTickets([FromBody] EventTicketModel model)
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
    public async Task<ActionResult<StandardResponse<PagedCollection<EventView>>>> ListEvents([FromQuery] PagingOptions _options)
    {
        return Result(await _eventService.ListEvents(_options));
    }


}
