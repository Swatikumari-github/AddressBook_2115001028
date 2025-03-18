using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    using System;
    using System.Net;
    using System.Net.Mail;

    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // SMTP server for Gmail
        private readonly int _smtpPort = 587; // Port for TLS
        private readonly string _smtpUser = "kumariswati7889@gmail.com"; // Your email address
        private readonly string _smtpPassword = "wkfq cjux jdra izlr"; // Your email password or app-specific password

        // Method to send the password reset email
        public void SendPasswordResetEmail(string recipientEmail, string token)
        {
            var resetUrl = $"https://yourapp.com/reset-password?token={token}";  // Your reset password URL
            var subject = "Password Reset Request";
            var body = $"Click here to reset your password: {resetUrl}";

            try
            {
                var client = new SmtpClient(_smtpServer)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_smtpUser, _smtpPassword),
                    EnableSsl = true  // Enable SSL for secure connection
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // Set to true if you're sending HTML content
                };

                mailMessage.To.Add(recipientEmail); // Recipient email

                // Send the email
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }

}
