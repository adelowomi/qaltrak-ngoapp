using Microsoft.AspNetCore.Mvc;

namespace NGOAPP;

[Route("api/[controller]")]
[ApiController]
public class UtilityController : StandardControllerBase
{
    private readonly IUtilityService _utilityService;

    public UtilityController(IUtilityService utilityService)
    {
        _utilityService = utilityService;
    }

    [HttpGet("location/types", Name = nameof(GetLocationTypes))]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModelI>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModelI>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModelI>>), 500)]
    public async Task<ActionResult<StandardResponse<List<BaseViewModelI>>>> GetLocationTypes()
    {
        return Ok(await _utilityService.GetLocationTypes());
    }

    [HttpGet("ticket/types", Name = nameof(GetTicketTypes))]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModelI>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModelI>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModelI>>), 500)]
    public async Task<ActionResult<StandardResponse<List<BaseViewModelI>>>> GetTicketTypes()
    {
        return Ok(await _utilityService.GetTicketTypes());
    }

    [HttpGet("event/types", Name = nameof(GetEventTypes))]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModel>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModel>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModel>>), 500)]
    public async Task<ActionResult<StandardResponse<List<BaseViewModel>>>> GetEventTypes()
    {
        return Ok(await _utilityService.GetEventTypes());
    }

    [HttpGet("event/categories", Name = nameof(GetEventCategories))]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModel>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModel>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModel>>), 500)]
    public async Task<ActionResult<StandardResponse<List<BaseViewModel>>>> GetEventCategories()
    {
        return Ok(await _utilityService.GetEventCategories());
    }

    [HttpGet("event/subcategories/{eventCategoryId}", Name = nameof(GetEventSubCategories))]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModelI>>), 200)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModelI>>), 400)]
    [ProducesResponseType(typeof(StandardResponse<List<BaseViewModelI>>), 500)]
    public async Task<ActionResult<StandardResponse<List<BaseViewModelI>>>> GetEventSubCategories(Guid eventCategoryId)
    {
        return Ok(await _utilityService.GetEventSubCategories(eventCategoryId));
    }
}
