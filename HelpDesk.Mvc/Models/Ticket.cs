using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Mvc.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public string Status { get; set; }

        public string RaisedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public static class TicketOptions
    {
        public static readonly string[] Priorities = { "Low", "Medium", "High" };
        public static readonly string[] Statuses = { "Open", "In Progress", "Closed" };
    }

    public class DashboardViewModel
    {
        public int TotalTickets { get; set; }
        public int OpenTickets { get; set; }
        public int ClosedTickets { get; set; }
    }
}
