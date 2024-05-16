namespace NGOAPP;
using Mapster;

public class UtilityService : IUtilityService
{
    private readonly IBaseRepository<LocationType> _locationTypeRepository;
    private readonly IBaseRepository<TicketType> _ticketTypeRepository;
    private readonly IBaseRepository<EventType> _eventTypeRepository;
    private readonly IBaseRepository<EventCategory> _eventCategoryRepository;
    private readonly IBaseRepository<EventSubCategory> _eventSubcategoryRepository;

    public UtilityService(IBaseRepository<LocationType> locationTypeRepository, IBaseRepository<TicketType> ticketTypeRepository, IBaseRepository<EventType> eventTypeRepository, IBaseRepository<EventCategory> eventCategoryRepository, IBaseRepository<EventSubCategory> eventSubcategoryRepository)
    {
        _locationTypeRepository = locationTypeRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _eventTypeRepository = eventTypeRepository;
        _eventCategoryRepository = eventCategoryRepository;
        _eventSubcategoryRepository = eventSubcategoryRepository;
    }

    public async Task<StandardResponse<List<BaseViewModelI>>> GetLocationTypes()
    {
        var locationTypes =  _locationTypeRepository.GetAll().ToList();
        //use mapster to map to view model
        var response  = locationTypes.Adapt<List<BaseViewModelI>>();
        return StandardResponse<List<BaseViewModelI>>.Ok(response);
    }

    public async Task<StandardResponse<List<BaseViewModelI>>> GetTicketTypes()
    {
        var ticketTypes =  _ticketTypeRepository.GetAll().ToList();
        //use mapster to map to view model
        var response  = ticketTypes.Adapt<List<BaseViewModelI>>();
        return StandardResponse<List<BaseViewModelI>>.Ok(response);
    }

    public async Task<StandardResponse<List<BaseViewModel>>> GetEventTypes()
    {
        var eventTypes =  _eventTypeRepository.GetAll().ToList();
        //use mapster to map to view model
        var response  = eventTypes.Adapt<List<BaseViewModel>>();
        return StandardResponse<List<BaseViewModel>>.Ok(response);
    }

    public async Task<StandardResponse<List<BaseViewModel>>> GetEventCategories()
    {
        var eventCategories =  _eventCategoryRepository.GetAll().ToList();
        //use mapster to map to view model
        var response  = eventCategories.Adapt<List<BaseViewModel>>();
        return StandardResponse<List<BaseViewModel>>.Ok(response);
    }

    public async Task<StandardResponse<List<BaseViewModelI>>> GetEventSubCategories(Guid eventCategoryId)
    {
        var eventCategories = _eventSubcategoryRepository.GetAll().Where(x => x.EventCategoryId == eventCategoryId).ToList();
        //use mapster to map to view model
        var response  = eventCategories.Adapt<List<BaseViewModelI>>();
        return StandardResponse<List<BaseViewModelI>>.Ok(response);
    }
}
