using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Helpful_Hackers._XBCAD7319._POE.Models;

namespace Helpful_Hackers._XBCAD7319._POE.Controllers
{
    public class AdditionalSupportController : Controller
    {
        // GET: /AdditionalSupport/ContactSupport

        public IActionResult ContactSupport()
        {
            return RedirectToAction("AdditionalSupport", "AdditionalSupport");
           // return RedirectToAction("AdditionalSupport");
        }

        public IActionResult AdditionalSupport()
        {
            return View();
            // return RedirectToAction("AdditionalSupport");
        }

        // POST: /AdditionalSupport/ContactSupport
        [HttpPost]
        public IActionResult ContactSupport(Support supportRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SendSupportRequestEmail(supportRequest);

                    // Redirect the user back to the home page or a success page
                    return RedirectToAction("UserProfile", "Auth");
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    return View("Error");
                }
            }

            return View();
        }
        private void SendSupportRequestEmail(Support supportRequest)
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
                mailMessage.Subject = "Support Request";
                mailMessage.Body = $"Support Request Details:\n\nName: {supportRequest.Name}\nEmail: {supportRequest.Email}\nSubject: {supportRequest.Subject}\nMessage: {supportRequest.Message}";

                // Send the email
                smtpClient.Send(mailMessage);
            }
        }
    }
}