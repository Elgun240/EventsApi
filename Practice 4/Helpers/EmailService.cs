using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MimeKit;
using Practice_4.Helpers;
using SendGrid;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using Humanizer;
using Practice_4.ViewModels;


public class EmailService:IEmailService
{
    
    public async Task SendEmailAsync(string email, string subject, string message1)
    {

        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
        client.UseDefaultCredentials = false;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("elgunpheonix@gmail.com", "lorf fqhf icvn gftt"); // Замените на ваш адрес Gmail и пароль приложения
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        MailMessage message = new MailMessage("elgunpheonix@gmail.com", email);
        message.Subject = subject;
        message.Body = message1;
        message.BodyEncoding = System.Text.Encoding.UTF8;
        message.IsBodyHtml = true;
        await client.SendMailAsync(message);
         
    }
   
}

