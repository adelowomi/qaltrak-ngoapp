namespace NGOAPP;

public interface IUtilityService
{
    Task<StandardResponse<List<BaseViewModelI>>> GetLocationTypes();
    Task<StandardResponse<List<BaseViewModelI>>> GetTicketTypes();
    Task<StandardResponse<List<BaseViewModel>>> GetEventTypes();
    Task<StandardResponse<List<BaseViewModel>>> GetEventCategories();
    Task<StandardResponse<List<BaseViewModelI>>> GetEventSubCategories(Guid eventCategoryId);
}
