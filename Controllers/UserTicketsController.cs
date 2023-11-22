// Controllers/UserTicketsController.cs


/***************************************************************************************
 *    Title: <How to Send Emails in ASP.Net Web Applications>
 *    Author: <Arno Waegemans>
 *    Date Published: <16 January 2023>
 *    Date Retrieved: <5 September 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://tutorials.eu/how-to-send-emails-in-asp-net-web-applications/>
 *
 ***************************************************************************************/

/***************************************************************************************
 *    Title: <Tutorial: Add sorting, filtering, and paging with the Entity Framework in an ASP.NET MVC application>
 *    Author: <Microsoft>
 *    Date Published: <14 April 2023>
 *    Date Retrieved: <10 September 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://learn.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application>
 *
 ***************************************************************************************/

/***************************************************************************************

 *    Title: <How to generate pdf using iTextSharp in asp.net mvc>
 *    Author: <Code2Night>
 *    Date Published: <6 August 2023>
 *    Date Retrieved: <25 October 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://code2night.com/Blog/MyBlog/how-to-generate-pdf-using-itextsharp-in-aspnet-mvc>

***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Helpful_Hackers._XBCAD7319._POE.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Helpful_Hackers._XBCAD7319._POE.Controllers
{
    public class UserTicketsController : Controller
    {
        private readonly TicketDbContext _context;

        public UserTicketsController(TicketDbContext context)
        {
            _context = context;
        }

        // GET: AdminTickets
        public async Task<IActionResult> Index(string search, int? page)
        {
            try
            {
                ViewBag.CurrentFilter = search;

                // Number of items per page
                int pageSize = 3; // You can adjust this as needed

                var tickets = from t in _context.Tickets
                              select t;

                if (!string.IsNullOrEmpty(search))
                {
                    tickets = tickets.Where(t => t.Email.Contains(search));
                }

                // Calculate the total number of pages
                int totalItems = await tickets.CountAsync();

                if (totalItems == 0)
                {
                    return View("Index", new PaginatedList<Ticket>(new List<Ticket>(), 0, 1, 3));
                }

                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                // Ensure page is within a valid range
                page ??= 1;
                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;

                // Calculate the number of items to skip
                int skip = (page.Value - 1) * pageSize;

                var paginatedTickets = await tickets
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                var model = new PaginatedList<Ticket>(paginatedTickets, totalItems, page.Value, pageSize);

                return View(model);
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        // GET: AdminTickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var ticket = await _context.Tickets.FirstOrDefaultAsync(m => m.Id == id);

                if (ticket == null)
                {
                    return NotFound();
                }

                return View(ticket);
            }
            catch (Exception ex)
            {
                // Handle exceptions here, e.g., log them
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        // GET: UserTickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Category,DateTicket,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                // Send email using SmtpClient
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    // Configure the SMTP client settings
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = new NetworkCredential("YOUR_EMAIL", "THE_EMAIL_BEING_SENT_TO");

                    // Create the email message
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("YOUR_EMAIL");
                    mailMessage.To.Add(ticket.Email);
                    mailMessage.Subject = "Ticket Created";
                    mailMessage.Body = $"Thank you for creating a ticket, a personal will be with you shortly. \n \nTicket Details Made: \nName: {ticket.Name}\nDescription: {ticket.Description}\nCategory: {ticket.Category}\nDate Issued: {ticket.DateTicket}";

                    // Send the email
                    smtpClient.Send(mailMessage);
                }

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: UserTickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: UserTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Category,DateTicket,Description")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: UserTickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: UserTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var ticket = await _context.Tickets.FindAsync(id);
                if (ticket != null)
                {
                    _context.Tickets.Remove(ticket);
                    await _context.SaveChangesAsync();
                }

                var remainingTickets = await _context.Tickets.ToListAsync();

                if (remainingTickets.Count == 0)
                {
                    return View("Index", new PaginatedList<Ticket>(remainingTickets, 0, 1, 3));
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception and handle as required
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // User downloads a copy of their ticket details
        public byte[] GeneratePDF(Ticket tick)
        {
            var ticket = _context.Tickets.Where(x => x.Id == tick.Id).FirstOrDefault();
            List<Ticket> data = _context.Tickets.ToList();

            var document = new Document();
            var memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            foreach (var item in data)
            {
                document.Add(new Paragraph("Ticket Details: \n" + item.Name + "\n" +
                    "----------------------------------------------------------------------\n" +
                    $"Email: {item.Email}\n" +
                    $"Category: {item.Category}\n" +
                    $"DateTicket: {item.DateTicket}\n" +
                    $"Description: {item.Description}\n" +
                    "----------------------------------------------------------------------\n"));
            }

            document.Close();

            return memoryStream.ToArray();
        }

        public async Task<IActionResult> DownloadPDF(int? id)
        {
            var ticket = await _context.Tickets.Where(x => x.Id == id).FirstOrDefaultAsync();
            return View(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadPDF(int? id, Ticket tick)
        {
            var ticket = await _context.Tickets.Where(x => x.Id == tick.Id).FirstOrDefaultAsync();
            var ticketName = await _context.Tickets.Where(n => n.Name == tick.Name).FirstOrDefaultAsync();
            
            if (ticket == null)
            {
                return NotFound();
            }

            var pdfData = GeneratePDF(tick);
            
            return File(pdfData, "application/pdf", "TicketDetails.pdf");
        }
    }
}
