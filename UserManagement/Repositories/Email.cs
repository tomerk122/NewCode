namespace UserManagement.Repositories
{
    public class Email
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Email()
        {
            Console.WriteLine(  "Im buildng");
        }

        public void Send(string to, string subject, string body)
        {
            // Simulate sending an email
            Console.WriteLine($"Sending email to {to} with subject '{subject}' and body '{body}'");
        }

    }
}
