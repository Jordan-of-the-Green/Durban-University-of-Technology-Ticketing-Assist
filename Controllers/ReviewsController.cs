using Helpful_Hackers._XBCAD7319._POE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Helpful_Hackers._XBCAD7319._POE.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly TicketDbContext _context;

        public ReviewsController(TicketDbContext context)
        {
            _context = context;
        }

        // GET: ReviewsController
        public ActionResult Index()
        {
            return View();
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

                var ticket = await _context.Reviews.FirstOrDefaultAsync(m => m.Id == id);

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


        // GET: ReviewsController/Create
        public ActionResult Create(int id)
        {
            var comment = _context.Reviews.Where(x => x.Id == id).FirstOrDefault();

            return View(comment);
        }

        // POST: ReviewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();

                return RedirectToAction("UserProfile", "Auth");
            }

            return View(review);

            /*try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }*/
        }

        [HttpGet]
        public IActionResult UserReviews(int id, Review review)
        {
            var listOfComments = _context.Reviews.ToList();

            return View(listOfComments);
        }
    }
}
