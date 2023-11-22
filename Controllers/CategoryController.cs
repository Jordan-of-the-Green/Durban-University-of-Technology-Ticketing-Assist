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

namespace Helpful_Hackers._XBCAD7319._POE.Controllers
{

   
    // This code will allow to set the priorites for the tickets
    public class CategoryController : Controller
    {
        private readonly TicketDbContext _context;
        public CategoryController(TicketDbContext context)
        {
            _context = context;
        }
        // GET: AdminTickets

        // High Tickets
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



        // Medium Tickets
        public async Task<IActionResult> Medium(string search, int? page)
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



        // Low Tickets
        public async Task<IActionResult> Low(string search, int? page)
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

    }
}
