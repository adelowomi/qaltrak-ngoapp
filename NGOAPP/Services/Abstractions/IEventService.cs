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
    Task<StandardResponse<EventView>> UpdateEvent(UpdateEventModel model);
    Task<StandardResponse<EventView>> UpdateEventDetails(UpdateEventDetailsModel model);
    Task<StandardResponse<bool>> UpdateEventSchedule(List<UpdateScheduleModel> model);
    Task<StandardResponse<bool>> UpdateEventContacts(List<UpdateContactModel> model);
    Task<StandardResponse<bool>> UpdateEventOrderFormDetails(UpdateEventOrderFormModel model);
    Task<StandardResponse<bool>> UpdateEventLocations(List<UpdateLocationModel> model);
    Task<StandardResponse<PagedCollection<SpeakerView>>> ListEventSpeaker(Guid eventId, PagingOptions _options);
    Task<StandardResponse<PagedCollection<ContactView>>> ListEventContacts(Guid eventId, PagingOptions _options);
    Task<StandardResponse<PagedCollection<LocationView>>> ListEventLocations(Guid eventId, PagingOptions _options);
    Task<StandardResponse<PagedCollection<ScheduleView>>> ListEventSchedules(Guid eventId, PagingOptions _options);
    Task<StandardResponse<PagedCollection<SessionView>>> ListEventSessions(Guid eventId, PagingOptions _options);

}
