namespace EComm.Web.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(string email, string body);
    }
}
