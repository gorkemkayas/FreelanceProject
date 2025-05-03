using FreelanceProject.Areas.Admin.Models;
using FreelanceProject.Data.Entities;
using FreelanceProject.Utilities;

namespace FreelanceProject.Services.Abstract
{
    public interface IRoleService
    {
        Task<ServiceResult<AppRole>> AddRoleAsync(RoleAddViewModel model);

        Task<ItemPagination<RoleViewModel>> GetPagedRolesAsync(int page, int pageSize, bool includeDeleted = false);
    }
}
