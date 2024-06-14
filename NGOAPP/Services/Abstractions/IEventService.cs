namespace NGOAPP;

public interface IEventService
{
    Task<StandardResponse<EventView>> CreateEvent(CreateEventModel model);
    Task<StandardResponse<EventView>> AddEventDetails(EventDetailsModel model);
    Task<StandardResponse<EventView>> AddEVentTickets(CreateEventTicketModel model);
    Task<StandardResponse<EventView>> AddEventOrderFormDetails(EventOrderFormModel model);
    Task<StandardResponse<EventView>> PublishEvent(PublishEventModel model);
    Task<StandardResponse<EventView>> GetEventById(Guid eventId);
    Task<StandardResponse<PagedCollection<EventView>>> ListEvents(PagingOptions _options, EventFilterOptions filterOptions);
    Task<StandardResponse<bool>> RegisterToAttendEventOrVolunteer(EventRegistrationModel model);
    Task<StandardResponse<bool>> UnregisterFromEventOrVolunteer(EventRegistrationModel model);
    Task<StandardResponse<PagedCollection<TicketView>>> ListUserTickets(PagingOptions _options);

}
