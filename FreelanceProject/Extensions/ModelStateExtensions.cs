using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FreelanceProject.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrorList(this ModelStateDictionary modelState, List<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
