/***************************************************************************************

 *    Title: <Implement Contact Us Form in ASP.Net Core>
 *    Author: <Mudassar Khan>
 *    Date Published: <15 Dec 2021>
 *    Date Retrieved: <27 October 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://www.aspsnippets.com/Articles/Implement-Contact-Us-Form-in-ASPNet-Core.aspx>

***************************************************************************************/


/***************************************************************************************

 *    Title: <MVC Contact Form with Email>
 *    Author: <iggyweb>
 *    Date Published: <6 May 2014>
 *    Date Retrieved: <26 October 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://stackoverflow.com/questions/23502407/mvc-contact-form-with-email>

***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

//references: for logger :
//https://www.linkedin.com/advice/0/how-do-you-communicate-collaborate-other-developers#:~:text=Error%20handling%20and%20logging%20are,are%20not%20only%20technical%20tasks.

//timer
//reference: https://www.codeproject.com/Questions/1243773/How-to-use-one-timer-to-check-database-and-another
//https://www.c-sharpcorner.com/article/timer-in-C-Sharp/

namespace Helpful_Hackers._XBCAD7319._POE.Models
{
    public class TicketStatusCheckService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TicketStatusCheckService> _logger;

        public TicketStatusCheckService(IServiceProvider serviceProvider, ILogger<TicketStatusCheckService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Run the status check every 2 days
            _timer = new Timer(DoStatusCheck, null, TimeSpan.Zero, TimeSpan.FromDays(2));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DoStatusCheck(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TicketDbContext>();

                try
                {
                    // Get unresolved tickets older than 2 days
                    var twoDaysAgo = DateTime.Now.AddDays(-2);
                    var unresolvedTickets = dbContext.Tickets
                        .Where(ticket => ticket.DateTicket <= twoDaysAgo)
                        .ToList();

                    foreach (var ticket in unresolvedTickets)
                    {
                        // Send notifications to admin
                        SendNotificationToAdmin(ticket);

                        // Add the ticket to the UnresolvedTickets table
                        dbContext.UnresolvedTickets.Add(new UnresolvedTicket
                        {
                            Id = ticket.Id,
                            DateAdded = DateTime.Now
                        });
                    }

                    // Save changes to the database
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Log the error
                    _logger.LogError(ex, "Error during status check");
                }
            }
        }

        private void SendNotificationToAdmin(Ticket ticket)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    // Configure the SMTP client settings
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = new NetworkCredential("YOUR_EMAIL", "YOUR_PASSWORD");

                    // Create the email message
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("YOUR_EMAIL");
                    mailMessage.To.Add("THE_EMAIL_BEING_SENT_TO");
                    mailMessage.Subject = "Unresolved Ticket Notification";
                    mailMessage.Body = $"A ticket is still unresolved after 2 days.\n\n" +
                        $"Ticket Details:\nName: {ticket.Name}\nDescription: {ticket.Description}\n" +
                        $"Category: {ticket.Category}\nDate Issued: {ticket.DateTicket}";

                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error sending notification");
            }
        }
    }
}
