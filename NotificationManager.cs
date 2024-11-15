using System;
using System.Net;
using System.Net.Mail;

class NotificationManager
{
    private const string AdminEmail = "admin@example.com";
    private const string SenderEmail = "notifier@example.com";
    private const string SenderPassword = "password";

    public static void NotifyAdmin(string subject, string message)
    {
        try
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(SenderEmail);
                mail.To.Add(AdminEmail);
                mail.Subject = subject;
                mail.Body = message;

                using (SmtpClient smtp = new SmtpClient("smtp.example.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            Console.WriteLine($"[СПОВІЩЕННЯ] Повідомлення надіслано адміністратору: {subject}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ПОМИЛКА СПОВІЩЕННЯ] Неможливо надіслати повідомлення: {ex.Message}");
        }
    }
}
