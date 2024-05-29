namespace NGOAPP;

public class StatisticsService : IStatisticsService
{
    private readonly IBaseRepository<Event> _eventRepository;
    private readonly IBaseRepository<Ticket> _ticketRepository;

    public StatisticsService(IBaseRepository<Event> eventRepository, IBaseRepository<Ticket> ticketRepository)
    {
        _eventRepository = eventRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<StandardResponse<StatisticsView>> GetStatistics(Guid? eventId = null)
    {
        var statistics = new StatisticsView();
        if (eventId == null)
        {
            statistics.GrossSales = _ticketRepository.Query().Sum(x => x.Price);
            statistics.NetSales = _ticketRepository.Query().Sum(x => x.Price);
            statistics.TotalRegisteredAttendees = _ticketRepository.Query().Count();
        }
        else
        {
            var eventTickets = _ticketRepository.Query().Where(x => x.EventId == eventId);
            statistics.GrossSales = eventTickets.Sum(x => x.Price);
            statistics.NetSales = eventTickets.Sum(x => x.Price);
            statistics.TotalRegisteredAttendees = eventTickets.Count();
        }

        return StandardResponse<StatisticsView>.Create(true, "Statistics retrieved successfully", statistics);
    }

    // public async Task<StandardResponse<PagedCollection<StatisticsEventView>>> ListEventsWithStatistics(PagingOptions pagingOptions)
    // {
    //     var events = _eventRepository.Query();
    //     var statisticsEvents = events.ToPagedCollection<Event, StatisticsEventView>(pagingOptions, Link.ToCollection(nameof(StatisticsController.ListEventsWithStatistics)));
    //     statisticsEvents.Value.ForEach(statisticsEvent =>
    //     {
    //         var eventTickets = _ticketRepository.Query().Where(x => x.EventId == statisticsEvent.Id);
    //         statisticsEvent.TotalRegisteredAttendees = eventTickets.Count();
    //     });
    //     return StandardResponse<PagedCollection<StatisticsEventView>>.Create(true, "Events retrieved successfully", statisticsEvents);
    // }

}
