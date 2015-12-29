namespace Logbook.Server.Contracts.Emails
{
    public class Email
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Receiver { get; set; }
        public string Sender { get; set; }
    }
}