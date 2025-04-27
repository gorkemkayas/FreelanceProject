using FreelanceProject.CustomMethods.Abstract;
using FreelanceProject.Data.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.CustomMethods.Concrete
{
    public class UrlGenerator : IUrlGenerator
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UrlGenerator(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, IHttpContextAccessor httpContextAccessor)
            {
                _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
                _httpContextAccessor = httpContextAccessor;
            }

        public string GenerateEmailConfirmationUrl(AppUser user, string token)
        {
            var emailConfirmationLink = _urlHelper.Action("ConfirmEmail", "User", new { userId = user.Id, token = token }, _httpContextAccessor.HttpContext.Request.Scheme);

            return emailConfirmationLink!;
        }

        public string GenerateResetPasswordUrl(AppUser user, string token)
            {
                var passwordResetLink = _urlHelper.Action("ResetPassword", "User", new { userId = user.Id, token = token }, _httpContextAccessor.HttpContext.Request.Scheme);

                return passwordResetLink!;
            }
        }
}
