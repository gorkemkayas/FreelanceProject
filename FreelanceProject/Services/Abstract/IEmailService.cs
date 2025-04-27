namespace FreelanceProject.Services.Abstract
{
    public interface IEmailService
    {
        Task SendForgetPasswordEmailAsync(string receiverEmail, string resetPasswordLink);
        Task SendEmailConfirmationEmailAsync(string receiverEmail, string confirmationLink);
    }
}
