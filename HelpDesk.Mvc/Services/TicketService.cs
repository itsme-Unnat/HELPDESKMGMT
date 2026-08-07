using System.Net.Http.Json;
using HelpDesk.Mvc.Models;

namespace HelpDesk.Mvc.Services
{
    /// <summary>
    /// Consumes the HelpDesk.Api TicketController endpoints over HttpClient.
    /// MVC controllers must talk to this service only — no direct DB access here.
    /// </summary>
    public class TicketService : ITicketService
    {
        private readonly HttpClient _httpClient;

        public TicketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Ticket>>("api/Ticket/All");
            return result ?? new List<Ticket>();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Ticket/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<Ticket>();
        }

        public async Task<bool> CreateTicketAsync(Ticket ticket)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Ticket", ticket);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTicketAsync(Ticket ticket)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Ticket/{ticket.Id}", ticket);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Ticket/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Ticket>> GetTicketsByStatusAsync(string status)
        {
            var result = await _httpClient.GetFromJsonAsync<List<Ticket>>($"api/Ticket/Status/{status}");
            return result ?? new List<Ticket>();
        }
    }
}
