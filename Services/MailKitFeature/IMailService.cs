namespace Company.PL.Helper.MailKitFeature
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(Email email);
    }
}
