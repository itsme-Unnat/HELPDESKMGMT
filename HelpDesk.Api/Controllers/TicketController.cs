using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _repository;

        public TicketController(ITicketRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Ticket/All
        [HttpGet("All")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _repository.GetAllTicketsAsync();
            return Ok(tickets);
        }

        // GET: api/Ticket/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = await _repository.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
        {
            if (ticket == null || string.IsNullOrWhiteSpace(ticket.Title))
            {
                return BadRequest("Ticket data is invalid.");
            }

            var newId = await _repository.CreateTicketAsync(ticket);
            ticket.Id = newId;
            return CreatedAtAction(nameof(GetTicketById), new { id = newId }, ticket);
        }

        // PUT: api/Ticket/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] Ticket ticket)
        {
            if (ticket == null || id != ticket.Id)
            {
                return BadRequest("Ticket id mismatch.");
            }

            var existing = await _repository.GetTicketByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _repository.UpdateTicketAsync(ticket);
            return NoContent();
        }

        // DELETE: api/Ticket/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var existing = await _repository.GetTicketByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _repository.DeleteTicketAsync(id);
            return NoContent();
        }

        // GET: api/Ticket/Status/{status}
        [HttpGet("Status/{status}")]
        public async Task<IActionResult> GetTicketsByStatus(string status)
        {
            var tickets = await _repository.GetTicketsByStatusAsync(status);
            return Ok(tickets);
        }
    }
}
