namespace Practice_4.Helpers
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
