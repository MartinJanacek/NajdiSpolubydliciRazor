namespace NajdiSpolubydliciRazor.Services.Interfaces
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string to, string subject, string body); 
    }
}
