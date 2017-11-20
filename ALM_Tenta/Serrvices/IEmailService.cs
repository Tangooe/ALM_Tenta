namespace ALM_Tenta.Serrvices
{
    public interface IEmailService
    {
        void SendMail(string sender, string recipient, string topic, string message);
    }
}