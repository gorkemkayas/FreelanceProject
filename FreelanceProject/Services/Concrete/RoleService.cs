using FreelanceProject.Areas.Admin.Models;
using FreelanceProject.Data.Entities;
using FreelanceProject.Services.Abstract;
using FreelanceProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FreelanceProject.Services.Concrete
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ServiceResult<AppRole>> AddRoleAsync(RoleAddViewModel model)
        {

            var result = await _roleManager.CreateAsync(new AppRole
            {
                Name = model.Name,
                CreatedBy = model.CreatedBy
            });

            if(!result.Succeeded)
            {
                return new ServiceResult<AppRole>()
                {
                    IsSuccess = false,
                    Errors = result.Errors.ToList()
                };
            }
            return new ServiceResult<AppRole>()
            {
                IsSuccess = true
            };
        }
        public async Task<ItemPagination<RoleViewModel>> GetPagedRolesAsync(int page, int pageSize, bool includeDeleted = false)
        {
            var itemsQuery = _roleManager.Roles;
            if (!includeDeleted)
            {
                itemsQuery = itemsQuery.Where(p => p.IsDeleted == false);
            }

            var pagedRoles = new ItemPagination<RoleViewModel>()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = (includeDeleted is true) ? _roleManager.Roles.Count() : _roleManager.Roles.Where(p => p.IsDeleted == false).Count(),
                Items = await itemsQuery
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(role => new RoleViewModel
                                    {
                                        Id = role.Id,
                                        CreatedBy = role.CreatedBy!,
                                        EditedBy = role.EditedBy,
                                        Name = role.Name!,
                                        IsDeleted = role.IsDeleted
                                    }).ToListAsync()
            };

            return pagedRoles;
        }
    }
}
