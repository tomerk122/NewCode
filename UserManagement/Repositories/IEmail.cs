namespace UserManagement.Repositories
{
    public interface IEmail
    {
        string Password { get; set; }
        int Port { get; set; }
        string SmtpServer { get; set; }
        string Username { get; set; }

        void Send(string to, string subject, string body);
    }
}