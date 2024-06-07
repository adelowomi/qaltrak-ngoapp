using System.Formats.Asn1;
using System.Net;
using System.Runtime.CompilerServices;
using AutoMapper;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace NGOAPP;

public class EventService : IEventService
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

    public EventService(IBaseRepository<Event> eventRepository, IBaseRepository<Location> locationRepository, IBaseRepository<Schedule> scheduleRepository, IBaseRepository<Contact> contactRepository, IBaseRepository<Session> sessionRepository, IBaseRepository<Speaker> speakerRepository, IBaseRepository<EventTicket> eventTicketRepository, IMapper mapper, IBaseRepository<Ticket> ticketRepository, IHttpContextAccessor httpContextAccessor, IBaseRepository<EventVolunteer> eventVolunteerRepository)
    {
        _eventRepository = eventRepository;
        _locationRepository = locationRepository;
        _scheduleRepository = scheduleRepository;
        _contactRepository = contactRepository;
        _sessionRepository = sessionRepository;
        _speakerRepository = speakerRepository;
        _eventTicketRepository = eventTicketRepository;
        _mapper = mapper;
        _ticketRepository = ticketRepository;
        _httpContextAccessor = httpContextAccessor;
        _eventVolunteerRepository = eventVolunteerRepository;
    }

    public async Task<StandardResponse<EventView>> CreateEvent(CreateEventModel model)
    {
        var newEvent = model.Adapt<Event>();
        newEvent.StatusId = (int)Statuses.Pending;
        newEvent = _eventRepository.CreateAndReturn(newEvent);
        newEvent = _eventRepository.Query().Include(x => x.Status).FirstOrDefault(x => x.Id == newEvent.Id);
        var eventView = _mapper.Map<EventView>(newEvent);
        return StandardResponse<EventView>.Create(true, "Event created successfully", eventView);
    }

    public async Task<StandardResponse<EventView>> AddEventDetails(EventDetailsModel model)
    {
        var existingEvent = _eventRepository.GetById(model.EventId);
        if (existingEvent == null)
            return StandardResponse<EventView>.Error("Event not found", HttpStatusCode.NotFound);

        // process list of schedules, locations, and contacts
        if (model.Locations != null && model.Locations.Any())
            AddEventLocations(model.Locations, model.EventId);

        if (model.Schedules != null && model.Schedules.Any())
            AddEventSchedules(model.Schedules, model.EventId);

        if (model.Contacts != null && model.Contacts.Any())
            AddEventContacts(model.Contacts, model.EventId);

        existingEvent.StartDate = model.StartDate;
        existingEvent.EndDate = model.EndDate;
        _eventRepository.Update(existingEvent);
        existingEvent = _eventRepository.Query().Include(x => x.Status).FirstOrDefault(x => x.Id == existingEvent.Id);
        var eventView = _mapper.Map<EventView>(existingEvent);
        return StandardResponse<EventView>.Create(true, "Event details updated successfully", eventView);
    }

    public async Task<StandardResponse<EventView>> AddEVentTickets(CreateEventTicketModel model)
    {
        var existingEvent = _eventRepository.GetById(model.EventId);
        if (existingEvent == null)
            return StandardResponse<EventView>.Error("Event not found", HttpStatusCode.NotFound);

        var newEventTickets = model.Adapt<List<EventTicket>>();
        newEventTickets = _eventTicketRepository.CreateMultiple(newEventTickets);
        existingEvent = _eventRepository.Query().Include(x => x.Status).FirstOrDefault(x => x.Id == existingEvent.Id);
        var eventView = _mapper.Map<EventView>(existingEvent);
        return StandardResponse<EventView>.Create(true, "Event tickets updated successfully", eventView);
    }

    public async Task<StandardResponse<EventView>> AddEventOrderFormDetails(EventOrderFormModel model)
    {
        var existingEvent = _eventRepository.GetById(model.EventId);
        if (existingEvent == null)
            return StandardResponse<EventView>.Error("Event not found", HttpStatusCode.NotFound);

        existingEvent.NumberOfVolunteersNeeded = model.NumberOfVolunteersNeeded;
        existingEvent.AttendeesCanVolunteer = model.AttendeesCanVolunteer;
        existingEvent.QuestionsForAttendees = model.QuestionsForAttendees;
        _eventRepository.Update(existingEvent);
        existingEvent = _eventRepository.Query().Include(x => x.Status).FirstOrDefault(x => x.Id == existingEvent.Id);
        var eventView = _mapper.Map<EventView>(existingEvent);
        return StandardResponse<EventView>.Create(true, "Event order form details updated successfully", eventView);
    }

    public async Task<StandardResponse<EventView>> PublishEvent(PublishEventModel model)
    {
        var existingEvent = _eventRepository.GetById(model.EventId);
        if (existingEvent == null)
            return StandardResponse<EventView>.Error("Event not found", HttpStatusCode.NotFound);

        existingEvent.EventTypeId = model.EventTypeId;
        existingEvent.EventCategoryId = model.EventCategoryId;
        existingEvent.EventSubCategoryId = model.EventSubCategoryId;
        existingEvent.PublishDate = model.PublishDate;
        existingEvent.IsPrivate = model.IsPrivate;
        existingEvent.IsPublished = model.PublishNow;
        if(model.PublishNow)
            existingEvent.StatusId = (int)Statuses.Published;
        _eventRepository.Update(existingEvent);
        existingEvent = _eventRepository.Query().Include(x => x.Status).FirstOrDefault(x => x.Id == existingEvent.Id);
        var eventView = _mapper.Map<EventView>(existingEvent);
        return StandardResponse<EventView>.Create(true, "Event published successfully", eventView);
    }

    public async Task<StandardResponse<EventView>> GetEventById(Guid eventId)
    {
        var existingEvent = _eventRepository.Query().Include(x => x.Locations).Include(x => x.Schedules).Include(x => x.Contacts).Include(x => x.Status).FirstOrDefault(x => x.Id == eventId);
        if (existingEvent == null)
            return StandardResponse<EventView>.Error("Event not found", HttpStatusCode.NotFound);
            
        var loggedInUserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var existingTicket = _ticketRepository.Query().FirstOrDefault(x => x.EventId == eventId && x.UserId == loggedInUserId);

        var eventView = _mapper.Map<EventView>(existingEvent);

        if (existingTicket != null)
            eventView.Registered = true;
        return StandardResponse<EventView>.Create(true, "Event retrieved successfully", eventView);
    }

    //TODO: add search and filter options to the list events method
    public async Task<StandardResponse<PagedCollection<EventView>>> ListEvents(PagingOptions _options, EventFilterOptions filterOptions)
    {
        var events = _eventRepository.Query()
                            .Include(x => x.Locations)
                            .Include(x => x.Schedules)
                            .Include(x => x.Contacts)
                            .OrderByDescending(x => x.DateCreated)
                            .AsQueryable()
                            .ApplySort(_options.SortDirection, _options.SortField);

        events = SearchEvents(events,_options.SearchQuery);
        events = FilterEvents(events, filterOptions);
        var pagedEvents = events.ToPagedCollection<Event, EventView>(_options, Link.ToCollection(nameof(EventController.ListEvents)));
        return StandardResponse<PagedCollection<EventView>>.Create(true, "Events retrieved successfully", pagedEvents);
    }

    public async Task<StandardResponse<List<SessionView>>> ListEventSessions(Guid eventId)
    {
        var eventSessions = _sessionRepository.Query().Where(x => x.EventId == eventId).ToList();
        var sessionViews = eventSessions.Adapt<List<SessionView>>();
        return StandardResponse<List<SessionView>>.Create(true, "Event sessions retrieved successfully", sessionViews);
    }

    public async Task<StandardResponse<List<SpeakerView>>> ListEventSpeakers(Guid eventId)
    {
        var eventSpeakers = _speakerRepository.Query().Where(x => x.EventId == eventId).ToList();
        var speakerViews = eventSpeakers.Adapt<List<SpeakerView>>();
        return StandardResponse<List<SpeakerView>>.Create(true, "Event speakers retrieved successfully", speakerViews);
    }

    public async Task<StandardResponse<bool>> RegisterToAttendEventOrVolunteer(EventRegistrationModel model)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var existingTicket = _ticketRepository.Query().FirstOrDefault(x => x.EventId == model.EventId && x.UserId == userId);

        if (existingTicket !=   null &&  model.IsAttending)
            return StandardResponse<bool>.Error("You have already registered for this event", HttpStatusCode.BadRequest);
        
        var existingVolunteer = _eventVolunteerRepository.Query().FirstOrDefault(x => x.EventId == model.EventId && x.UserId == userId);

        if (existingVolunteer != null && model.IsVolunteering)
            return StandardResponse<bool>.Error("You have already volunteered for this event", HttpStatusCode.BadRequest);

        var eventTicket = _eventTicketRepository.GetById(model.EventTicketId);

        if (eventTicket == null)
            return StandardResponse<bool>.Error("Event ticket not found", HttpStatusCode.NotFound);

        if(model.IsAttending)
        {
            var newTicket = new Ticket
            {
                EventId = model.EventId,
                UserId = userId,
                EventTicketId = model.EventTicketId,
                TicketTypeId = eventTicket.TicketTypeId,
                Price = eventTicket.Price,
                Free = eventTicket.Price == 0 ? true : false,
            };

            newTicket = _ticketRepository.CreateAndReturn(newTicket);
        }
        
        if(model.IsVolunteering)
        {
            var newVolunteer = new EventVolunteer
            {
                EventId = model.EventId,
                UserId = userId
            };

            newVolunteer = _eventVolunteerRepository.CreateAndReturn(newVolunteer);
        }

        return StandardResponse<bool>.Ok(true);
    }
    // update event details, tickets, order form details


    #region Private Methods
    private IQueryable<Event> FilterEvents(IQueryable<Event> events, EventFilterOptions filterOptions)
    {
        if (filterOptions.GroupId.HasValue)
            events = events.Where(x => x.GroupId == filterOptions.GroupId);
        
        if (filterOptions.EventTypeId.HasValue)
            events = events.Where(x => x.EventTypeId == filterOptions.EventTypeId);

        if (filterOptions.EventCategoryId.HasValue)
            events = events.Where(x => x.EventCategoryId == filterOptions.EventCategoryId);

        if (filterOptions.EventSubCategoryId.HasValue)
            events = events.Where(x => x.EventSubCategoryId == filterOptions.EventSubCategoryId);

        if (filterOptions.StartDate.HasValue)
            events = events.Where(x => x.StartDate >= filterOptions.StartDate);

        if (filterOptions.EndDate.HasValue)
            events = events.Where(x => x.EndDate <= filterOptions.EndDate);

        return events;
    }

    private IQueryable<Event> SearchEvents(IQueryable<Event> events, string searchQuery)
    {
        if (string.IsNullOrEmpty(searchQuery))
            return events;

        var query = searchQuery.ToLower();
        return events.Where(x => x.Title.ToLower().Contains(query) || x.Description.ToLower().Contains(query));
    }
    #endregion


    // private methods for processing schedules, locations, contacts, sessions, speakers and other related entities
    #region Private Methods
    private List<Location> AddEventLocations(List<LocationModel> locations, Guid eventId)
    {
        var eventLocations = locations.Adapt<List<Location>>();
        eventLocations.ForEach(x => x.EventId = eventId);
        return _locationRepository.CreateMultiple(eventLocations);
    }

    private List<Schedule> AddEventSchedules(List<ScheduleModel> schedules, Guid eventId)
    {
        var eventSchedules = schedules.Adapt<List<Schedule>>();
        eventSchedules.ForEach(x => x.EventId = eventId);
        return _scheduleRepository.CreateMultiple(eventSchedules);
    }

    private List<Session> AddEventSessions(List<SessionModel> sessions)
    {
        var eventSessions = sessions.Adapt<List<Session>>();
        return _sessionRepository.CreateMultiple(eventSessions);
    }

    private List<Speaker> AddEventSpeakers(List<SpeakerModel> speakers)
    {
        var eventSpeakers = speakers.Adapt<List<Speaker>>();
        return _speakerRepository.CreateMultiple(eventSpeakers);
    }

    private List<Contact> AddEventContacts(List<ContactModel> contacts, Guid eventId)
    {
        var eventContacts = contacts.Adapt<List<Contact>>();
        eventContacts.ForEach(x => x.EventId = eventId);
        return _contactRepository.CreateMultiple(eventContacts);
    }
    #endregion
}
