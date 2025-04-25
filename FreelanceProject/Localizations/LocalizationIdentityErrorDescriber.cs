using Microsoft.AspNetCore.Identity;

namespace FreelanceProject.Localizations
{
    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "The email you entered has already been registered."
            };
        }
    }
}
