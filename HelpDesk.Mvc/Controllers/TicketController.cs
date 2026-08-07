using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Mvc.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: /Ticket/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();

            var model = new DashboardViewModel
            {
                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => t.Status == "Open"),
                ClosedTickets = tickets.Count(t => t.Status == "Closed")
            };

            return View(model);
        }

        // GET: /Ticket/Index  (View All Tickets)
        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return View(tickets);
        }

        // GET: /Ticket/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: /Ticket/Create
        public IActionResult Create()
        {
            ViewBag.Priorities = TicketOptions.Priorities;
            var ticket = new Ticket { Status = "Open" };
            return View(ticket);
        }

        // POST: /Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            // Status is hardcoded to Open regardless of what was posted.
            ticket.Status = "Open";

            if (!ModelState.IsValid)
            {
                ViewBag.Priorities = TicketOptions.Priorities;
                return View(ticket);
            }

            var success = await _ticketService.CreateTicketAsync(ticket);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to create ticket.");
                ViewBag.Priorities = TicketOptions.Priorities;
                return View(ticket);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Ticket/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.Priorities = TicketOptions.Priorities;
            ViewBag.Statuses = TicketOptions.Statuses;
            return View(ticket);
        }

        // POST: /Ticket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Priorities = TicketOptions.Priorities;
                ViewBag.Statuses = TicketOptions.Statuses;
                return View(ticket);
            }

            var success = await _ticketService.UpdateTicketAsync(ticket);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Failed to update ticket.");
                ViewBag.Priorities = TicketOptions.Priorities;
                ViewBag.Statuses = TicketOptions.Statuses;
                return View(ticket);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Ticket/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: /Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Ticket/FilterByStatus?status=Open
        public async Task<IActionResult> FilterByStatus(string status)
        {
            ViewBag.Statuses = TicketOptions.Statuses;
            ViewBag.SelectedStatus = status;

            List<Ticket> tickets = string.IsNullOrEmpty(status)
                ? new List<Ticket>()
                : await _ticketService.GetTicketsByStatusAsync(status);

            return View(tickets);
        }
    }
}
