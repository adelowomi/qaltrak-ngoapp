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

    public async Task<StandardResponse<EventView>> UpdateEvent(UpdateEventModel model)
    {
        var existingEvent = _eventRepository.GetById(model.EventId);
        if (existingEvent == null)
            return StandardResponse<EventView>.Error("Event not found", HttpStatusCode.NotFound);

        existingEvent.Title = model.Title;
        existingEvent.Description = model.Description;
        existingEvent.CoverImage = model.CoverImage;
        _eventRepository.Update(existingEvent);
        existingEvent = _eventRepository.Query().Include(x => x.Status).FirstOrDefault(x => x.Id == existingEvent.Id);
        var eventView = _mapper.Map<EventView>(existingEvent);
        return StandardResponse<EventView>.Create(true, "Event updated successfully", eventView);
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

    public async Task<StandardResponse<EventView>> UpdateEventDetails(UpdateEventDetailsModel model)
    {
        var existingEvent = _eventRepository.GetById(model.EventId);
        if (existingEvent == null)
            return StandardResponse<EventView>.Error("Event not found", HttpStatusCode.NotFound);

        existingEvent.StartDate = model.StartDate;
        existingEvent.EndDate = model.EndDate;
        _eventRepository.Update(existingEvent);
        existingEvent = _eventRepository.Query().Include(x => x.Status).FirstOrDefault(x => x.Id == existingEvent.Id);
        var eventView = _mapper.Map<EventView>(existingEvent);
        return StandardResponse<EventView>.Create(true, "Event details updated successfully", eventView);
    }

    public async Task<StandardResponse<bool>> UpdateEventSchedule(List<UpdateScheduleModel> model)
    {
        foreach (var schedule in model)
        {
            if (schedule.ScheduleId == null || schedule.ScheduleId == Guid.Empty)
            {
                var newSchedule = schedule.Adapt<ScheduleModel>();
                AddEventSchedules(new List<ScheduleModel> { newSchedule }, schedule.EventId);
            }
            if (schedule.ScheduleId != null || schedule.ScheduleId != Guid.Empty)
            {
                var existingSchedule = _scheduleRepository.Query().Include(x => x.Sessions).FirstOrDefault(x => x.Id == (Guid)schedule.ScheduleId);
                if (existingSchedule == null)
                    return StandardResponse<bool>.Error("Schedule not found", HttpStatusCode.NotFound);

                existingSchedule.Name = schedule.Name;
                if (schedule.Sessions != null && schedule.Sessions.Any())
                {
                    foreach (var session in schedule.Sessions)
                    {
                        if (session.SessionId == null || session.SessionId == Guid.Empty)
                        {
                            var newSession = session.Adapt<SessionModel>();
                            newSession.ScheduleId = existingSchedule.Id;
                            AddEventSessions(new List<SessionModel> { newSession });
                        }
                        if (session.SessionId != null || session.SessionId != Guid.Empty)
                        {
                            var existingSession = _sessionRepository.GetById((Guid)session.SessionId);
                            if (existingSession == null)
                                return StandardResponse<bool>.Error("Session not found", HttpStatusCode.NotFound);

                            existingSession.DateModified = DateTime.Now;
                            existingSession.Description = session.Description;
                            existingSession.Start = session.StartDateTime;
                            existingSession.End = session.EndDateTime;

                            if (session.Speakers != null && session.Speakers.Any())
                            {
                                foreach (var speaker in session.Speakers)
                                {
                                    if (speaker.SpeakerId == null || speaker.SpeakerId == Guid.Empty)
                                    {
                                        var newSpeaker = speaker.Adapt<SpeakerModel>();
                                        newSpeaker.SessionId = existingSession.Id;
                                        AddEventSpeakers(new List<SpeakerModel> { newSpeaker });
                                    }
                                    if (speaker.SpeakerId != null || speaker.SpeakerId != Guid.Empty)
                                    {
                                        var existingSpeaker = _speakerRepository.Query().Include(x => x.Images).FirstOrDefault(x => x.Id ==(Guid)speaker.SpeakerId);
                                        if (existingSpeaker == null)
                                            return StandardResponse<bool>.Error("Speaker not found", HttpStatusCode.NotFound);

                                        existingSpeaker.Name = speaker.Name;
                                        existingSpeaker.Bio = speaker.Bio;
                                        _speakerRepository.Update(existingSpeaker);
                                    }
                                }
                            }


                            _sessionRepository.Update(existingSession);
                        }
                    }
                }
                _scheduleRepository.Update(existingSchedule);
            }
        }

        return StandardResponse<bool>.Create(true, "Event schedule updated successfully");
    }

    public async Task<StandardResponse<bool>> UpdateEventContacts(List<UpdateContactModel> model)
    {
        foreach (var contact in model)
        {
            if (contact.ContactId == null || contact.ContactId == Guid.Empty)
            {
                var newContact = contact.Adapt<ContactModel>();
                AddEventContacts(new List<ContactModel> { newContact }, contact.EventId);
            }
            if (contact.ContactId != null || contact.ContactId != Guid.Empty)
            {
                var existingContact = _contactRepository.GetById((Guid)contact.ContactId);
                if (existingContact == null)
                    return StandardResponse<bool>.Error("Contact not found", HttpStatusCode.NotFound);

                existingContact.Name = contact.Name;
                existingContact.Email = contact.Email;
                existingContact.Phone = contact.Phone;
                _contactRepository.Update(existingContact);
            }
        }

        return StandardResponse<bool>.Create(true, "Event contacts updated successfully");
    }
    public async Task<StandardResponse<bool>> UpdateEventOrderFormDetails(UpdateEventOrderFormModel model)
    {
        var existingEvent = _eventRepository.GetById(model.EventId);
        if (existingEvent == null)
            return StandardResponse<bool>.Error("Event not found", HttpStatusCode.NotFound);

        existingEvent.NumberOfVolunteersNeeded = model.NumberOfVolunteersNeeded;
        existingEvent.AttendeesCanVolunteer = model.AttendeesCanVolunteer;
        existingEvent.QuestionsForAttendees = model.QuestionsForAttendees;
        _eventRepository.Update(existingEvent);
        return StandardResponse<bool>.Create(true, "Event order form details updated successfully");
    }

    public async Task<StandardResponse<bool>> UpdateEventLocations(List<UpdateLocationModel> model)
    {
        foreach (var location in model)
        {
            if (location.LocationId == null || location.LocationId == Guid.Empty)
            {
                var newLocation = location.Adapt<LocationModel>();
                AddEventLocations(new List<LocationModel> { newLocation }, location.EventId);
            }
            if (location.LocationId != null || location.LocationId != Guid.Empty)
            {
                var existingLocation = _locationRepository.GetById((Guid)location.LocationId);
                if (existingLocation == null)
                    return StandardResponse<bool>.Error("Location not found", HttpStatusCode.NotFound);

                existingLocation.Name = location.Name;
                existingLocation.AddressLine = location.AddressLine;
                existingLocation.Latitude = location.Latitude;
                existingLocation.Longitude = location.Longitude;
                existingLocation.Description = location.Description;
                _locationRepository.Update(existingLocation);
            }
        }

        return StandardResponse<bool>.Create(true, "Event location updated successfully");
    }

    public async Task<StandardResponse<EventView>> AddEVentTickets(CreateEventTicketModel model)
    {
        var existingEvent = _eventRepository.GetById(model.EventId);
        if (existingEvent == null)
            return StandardResponse<EventView>.Error("Event not found", HttpStatusCode.NotFound);

        var newEventTickets = model.Tickets.Adapt<List<EventTicket>>();
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
        if (model.PublishNow)
            existingEvent.StatusId = (int)Statuses.Published;
        _eventRepository.Update(existingEvent);
        existingEvent = _eventRepository.Query().Include(x => x.Status).FirstOrDefault(x => x.Id == existingEvent.Id);
        var eventView = _mapper.Map<EventView>(existingEvent);
        return StandardResponse<EventView>.Create(true, "Event published successfully", eventView);
    }


    public async Task<StandardResponse<EventView>> GetEventById(Guid eventId)
    {
        var existingEvent = _eventRepository.Query().Include(x => x.Locations).Include(x => x.EventTicket).Include(x => x.Schedules).Include(x => x.Contacts).Include(x => x.Status).FirstOrDefault(x => x.Id == eventId);
        if (existingEvent == null)
            return StandardResponse<EventView>.Error("Event not found", HttpStatusCode.NotFound);

        var loggedInUserId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var existingTicket = _ticketRepository.Query().FirstOrDefault(x => x.EventId == eventId && x.UserId == loggedInUserId);
        var existingVolunteer = _eventVolunteerRepository.Query().FirstOrDefault(x => x.EventId == eventId && x.UserId == loggedInUserId);

        var eventView = _mapper.Map<EventView>(existingEvent);

        if (existingTicket != null)
            eventView.Registered = true;

        if (existingVolunteer != null)
            eventView.Volunteered = true;
        return StandardResponse<EventView>.Create(true, "Event retrieved successfully", eventView);
    }

    //TODO: add search and filter options to the list events method
    public async Task<StandardResponse<PagedCollection<EventView>>> ListEvents(PagingOptions _options, EventFilterOptions filterOptions)
    {
        var events = _eventRepository.Query()
                            .Include(x => x.Locations)
                            .Include(x => x.Schedules)
                            .Include(x => x.Contacts)
                            .OrderByDescending(x => x.DateCreated.Date.DayOfYear)
                            .AsQueryable()
                            .ApplySort(_options.SortDirection, _options.SortField);

        events = SearchEvents(events, _options.SearchQuery);
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

        if (existingTicket != null && model.IsAttending)
            return StandardResponse<bool>.Error("You have already registered for this event", HttpStatusCode.BadRequest);

        var existingVolunteer = _eventVolunteerRepository.Query().FirstOrDefault(x => x.EventId == model.EventId && x.UserId == userId);

        if (existingVolunteer != null && model.IsVolunteering)
            return StandardResponse<bool>.Error("You have already volunteered for this event", HttpStatusCode.BadRequest);

        var eventTicket = _eventTicketRepository.GetById(model.EventTicketId);

        if (eventTicket == null)
            return StandardResponse<bool>.Error("Event ticket not found", HttpStatusCode.NotFound);

        if (model.IsAttending)
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

        if (model.IsVolunteering)
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

    public async Task<StandardResponse<bool>> UnregisterFromEventOrVolunteer(EventRegistrationModel model)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var existingTicket = _ticketRepository.Query().FirstOrDefault(x => x.EventId == model.EventId && x.UserId == userId);

        if (existingTicket != null && model.IsAttending)
            _ticketRepository.Delete(existingTicket);

        var existingVolunteer = _eventVolunteerRepository.Query().FirstOrDefault(x => x.EventId == model.EventId && x.UserId == userId);

        if (existingVolunteer != null && model.IsVolunteering)
            _eventVolunteerRepository.Delete(existingVolunteer);

        return StandardResponse<bool>.Ok(true);
    }

    public async Task<StandardResponse<PagedCollection<TicketView>>> ListUserTickets(PagingOptions _options)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId<Guid>();
        var tickets = _ticketRepository.Query().Where(x => x.UserId == userId).Include(x => x.Event).Include(x => x.EventTicket).AsQueryable().ApplySort(_options.SortDirection, _options.SortField);
        var pagedTickets = tickets.ToPagedCollection<Ticket, TicketView>(_options, Link.ToCollection(nameof(EventController.ListUserTickets)));
        return StandardResponse<PagedCollection<TicketView>>.Create(true, "Tickets retrieved successfully", pagedTickets);
    }
   
    public async Task<StandardResponse<PagedCollection<SpeakerView>>> ListEventSpeaker(Guid eventId, PagingOptions _options)
    {
        var speakers = _speakerRepository.Query().Where(x => x.EventId == eventId).AsQueryable();
        var pagedSpeakers = speakers.ToPagedCollection<Speaker, SpeakerView>(_options, Link.ToCollection(nameof(EventController.ListEventSpeakers)));
        return StandardResponse<PagedCollection<SpeakerView>>.Create(true, "Speakers retrieved successfully", pagedSpeakers);
    }

    public async Task<StandardResponse<PagedCollection<ContactView>>> ListEventContacts(Guid eventId, PagingOptions _options)
    {
        var contacts = _contactRepository.Query().Where(x => x.EventId == eventId).AsQueryable();
        var pagedContacts = contacts.ToPagedCollection<Contact, ContactView>(_options, Link.ToCollection(nameof(EventController.ListEventContacts)));
        return StandardResponse<PagedCollection<ContactView>>.Create(true, "Contacts retrieved successfully", pagedContacts);
    }

    public async Task<StandardResponse<PagedCollection<LocationView>>> ListEventLocations(Guid eventId, PagingOptions _options)
    {
        var locations = _locationRepository.Query().Where(x => x.EventId == eventId).AsQueryable();
        var pagedLocations = locations.ToPagedCollection<Location, LocationView>(_options, Link.ToCollection(nameof(EventController.ListEventLocations)));
        return StandardResponse<PagedCollection<LocationView>>.Create(true, "Locations retrieved successfully", pagedLocations);
    }

    public async Task<StandardResponse<PagedCollection<ScheduleView>>> ListEventSchedules(Guid eventId, PagingOptions _options)
    {
        var schedules = _scheduleRepository.Query().Where(x => x.EventId == eventId).AsQueryable();
        var pagedSchedules = schedules.ToPagedCollection<Schedule, ScheduleView>(_options, Link.ToCollection(nameof(EventController.ListEventSchedules)));
        return StandardResponse<PagedCollection<ScheduleView>>.Create(true, "Schedules retrieved successfully", pagedSchedules);
    }

    public async Task<StandardResponse<PagedCollection<SessionView>>> ListEventSessions(Guid eventId, PagingOptions _options)
    {
        var sessions = _sessionRepository.Query().Where(x => x.EventId == eventId).AsQueryable();
        var pagedSessions = sessions.ToPagedCollection<Session, SessionView>(_options, Link.ToCollection(nameof(EventController.ListEventSessions)));
        return StandardResponse<PagedCollection<SessionView>>.Create(true, "Sessions retrieved successfully", pagedSessions);
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
