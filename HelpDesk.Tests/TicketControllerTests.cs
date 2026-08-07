using HelpDesk.Api.Controllers;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HelpDesk.Tests
{
    public class TicketControllerTests
    {
        private readonly Mock<ITicketRepository> _mockRepo;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _mockRepo = new Mock<ITicketRepository>();
            _controller = new TicketController(_mockRepo.Object);
        }

        // 1. GetAllTickets_ReturnsOkResult_WhenTicketsExist
        [Fact]
        public async Task GetAllTickets_ReturnsOkResult_WhenTicketsExist()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "Printer not working", Priority = "Low", Status = "Open" },
                new Ticket { Id = 2, Title = "VPN issue", Priority = "High", Status = "Open" }
            };
            _mockRepo.Setup(r => r.GetAllTicketsAsync()).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetAllTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Equal(2, returnedTickets.Count);
        }

        // 2. GetTicketById_ReturnsOkResult_WhenTicketExists
        [Fact]
        public async Task GetTicketById_ReturnsOkResult_WhenTicketExists()
        {
            // Arrange
            var ticket = new Ticket { Id = 1, Title = "Laptop overheating", Priority = "Medium", Status = "Open" };
            _mockRepo.Setup(r => r.GetTicketByIdAsync(1)).ReturnsAsync(ticket);

            // Act
            var result = await _controller.GetTicketById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(1, returnedTicket.Id);
        }

        // 3. GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist
        [Fact]
        public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.GetTicketById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // 4. CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully
        [Fact]
        public async Task CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully()
        {
            // Arrange
            var newTicket = new Ticket { Title = "Network down", Priority = "High", Status = "Open" };
            _mockRepo.Setup(r => r.CreateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(10);

            // Act
            var result = await _controller.CreateTicket(newTicket);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedTicket = Assert.IsType<Ticket>(createdResult.Value);
            Assert.Equal(10, returnedTicket.Id);
        }

        // 5. CreateTicket_ReturnsBadRequest_WhenTicketIsNull
        [Fact]
        public async Task CreateTicket_ReturnsBadRequest_WhenTicketIsNull()
        {
            // Act
            var result = await _controller.CreateTicket(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        // 6. GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketsExist
        [Fact]
        public async Task GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketsExist()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "Slow wifi", Priority = "Low", Status = "Open" }
            };
            _mockRepo.Setup(r => r.GetTicketsByStatusAsync("Open")).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetTicketsByStatus("Open");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Single(returnedTickets);
            Assert.Equal("Open", returnedTickets[0].Status);
        }
    }
}
