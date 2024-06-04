namespace NGOAPP;

public interface IAdminService
{
    Task<StandardResponse<UserView>> CreateUser(AdminModel model);
    Task<StandardResponse<List<RolesView>>> ListRoles();
    Task<StandardResponse<UserView>> AddUserToGroup(Guid userId, Guid groupId);
    Task<StandardResponse<PagedCollection<UserView>>> ListGroupUsers(PagingOptions pagingOptions, string searchQuery);
    Task<StandardResponse<AdminScheduleModel>> CreateAdminSchedule(AdminScheduleModel model);
    Task<StandardResponse<List<AdminScheduleModel>>> ListAdminSchedulesForCurrentMonth(int currentMonth = 0);
    Task<StandardResponse<AdminScheduleModel>> GetAdminSchedule(Guid scheduleId);
    Task<StandardResponse<UserView>> RemoveUserFromGroup(Guid userId, Guid groupId);
}
