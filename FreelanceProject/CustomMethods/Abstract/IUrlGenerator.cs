using FreelanceProject.Data.Entities;

namespace FreelanceProject.CustomMethods.Abstract
{
    public interface IUrlGenerator
    {
        public string GenerateResetPasswordUrl(AppUser user, string token);
        public string GenerateEmailConfirmationUrl(AppUser user, string token);
    }
}