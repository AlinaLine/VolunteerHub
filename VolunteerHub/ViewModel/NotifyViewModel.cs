using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using VolunteerHub.Model;

namespace VolunteerHub.ViewModel
{
    public static class NotifyViewModel
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient("smtp.mail.ru"))
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("no-reply-volunteerhub@mail.ru"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };
                    mailMessage.To.Add(toEmail);

                    client.Port = 587;
                    client.Credentials = new NetworkCredential("no-reply-volunteerhub@mail.ru", "tpuC4baFSXUMLXziS9CR");
                    client.EnableSsl = true;

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке электронной почты: {ex.Message}");
                throw;  
            }
        }

        public async static Task NotifyNewEvent(string userEmail, Events newEvent)
        {
            string subject = "Новое мероприятие: " + newEvent.Name;
            string body = $"Уважаемый участник, уведомляем вас о новом мероприятии: {newEvent.Name}, которое состоится {newEvent.EventDate}. Описание: {newEvent.Description}.";
            await SendEmailAsync(userEmail, subject, body);
        }

        public async static Task NotifyEventRegistration(string userEmail, Events eventInfo)
        {
            string subject = "Подписка на мероприятие: " + eventInfo.Name;
            string body = $"Вы успешно зарегистрировались на мероприятие '{eventInfo.Name}', которое состоится {eventInfo.EventDate}.";
            await SendEmailAsync(userEmail, subject, body);
        }

        public async static Task NotifyEventUnregistration(string userEmail, Events eventInfo)
        {
            string subject = "Отмена подписки на мероприятие: " + eventInfo.Name;
            string body = $"Вы были отписаны от мероприятия '{eventInfo.Name}', запланированного на {eventInfo.EventDate}.";
            await SendEmailAsync(userEmail, subject, body);
        }

    }
}
