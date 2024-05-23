﻿namespace NGOAPP;

public interface IEventService
{
    Task<StandardResponse<EventView>> CreateEvent(CreateEventModel model);
    Task<StandardResponse<EventView>> AddEventDetails(EventDetailsModel model);
    Task<StandardResponse<EventView>> AddEVentTickets(EventTicketModel model);
    Task<StandardResponse<EventView>> AddEventOrderFormDetails(EventOrderFormModel model);
    Task<StandardResponse<EventView>> PublishEvent(PublishEventModel model);
    Task<StandardResponse<EventView>> GetEventById(Guid eventId);
    Task<StandardResponse<PagedCollection<EventView>>> ListEvents(PagingOptions _options, EventFilterOptions filterOptions);

}