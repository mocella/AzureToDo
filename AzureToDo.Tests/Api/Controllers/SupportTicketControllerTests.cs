using Moq;
using AzureToDo.ApiService.Controllers;
using AzureToDo.Db.Entities;
using AzureToDo.Db.Repositories;
using AzureToDo.Tests;

[Trait(Constants.TestCategory, Constants.UnitTestCategory)]
public class SupportTicketControllerTests
{
    private readonly Mock<IGenericRepository<SupportTicket>> _mockRepo;
    private readonly SupportTicketController _controller;

    public SupportTicketControllerTests()
    {
        _mockRepo = new Mock<IGenericRepository<SupportTicket>>();
        _controller = new SupportTicketController(_mockRepo.Object);
    }

    [Fact]
    public void GetSupportTickets_ReturnsAllTickets()
    {
        // Arrange
        var tickets = new List<SupportTicket>
        {
            GetSupportTicketInstance(1),
            GetSupportTicketInstance(2)
        };
        _mockRepo.Setup(repo => repo.Get(null, null, string.Empty))
            .Returns(tickets);

        // Act
        var result = _controller.GetSupportTickets();

        // Assert
        Assert.Equal(tickets.Count, ((List<SupportTicket>)result).Count);
    }

    [Fact]
    public void GetSupportTicket_ReturnsTicketById()
    {
        // Arrange
        var ticket = GetSupportTicketInstance();
        _mockRepo.Setup(repo => repo.GetById(1))
            .Returns(ticket);

        // Act
        var result = _controller.GetSupportTicket(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ticket.Id, result.Id);
    }

    [Fact]
    public void PostSupportTicket_AddsNewTicket()
    {
        // Arrange
        var ticket = GetSupportTicketInstance();

        // Act
        _controller.PostSupportTicket(ticket);

        // Assert
        _mockRepo.Verify(repo => repo.Insert(ticket), Times.Once);
        _mockRepo.Verify(repo => repo.Save(), Times.Once);
    }

    [Fact]
    public void DeleteSupportTicket_DeletesTicket()
    {
        // Arrange
        var ticketId = 1;

        // Act
        _controller.DeleteSupportTicket(ticketId);

        // Assert
        _mockRepo.Verify(repo => repo.Delete(ticketId), Times.Once);
        _mockRepo.Verify(repo => repo.Save(), Times.Once);
    }

    private static SupportTicket GetSupportTicketInstance(int id = 1)
    {
        var ticket = new SupportTicket { Id = id, Description = Faker.Lorem.Sentence(), Title = Faker.Lorem.GetFirstWord() };
        return ticket;
    }
}
