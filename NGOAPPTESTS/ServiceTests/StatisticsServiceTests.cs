namespace NGOAPPTESTS;

public class StatisticsServiceTests
    {
        [Fact]
        public async Task GetStatistics_WithoutEventId_ReturnsStatistics()
        {
            // Arrange
            var eventRepository = Substitute.For<IBaseRepository<Event>>();
            var ticketRepository = Substitute.For<IBaseRepository<Ticket>>();
            var statisticsService = new StatisticsService(eventRepository, ticketRepository);

            var tickets = new List<Ticket>
            {
                new Ticket { Price = 10 },
                new Ticket { Price = 20 },
                new Ticket { Price = 30 }
            };
            ticketRepository.Query().Returns(tickets.AsQueryable());
            
            // Act
            var result = await statisticsService.GetStatistics();

            // Assert
            Assert.True(result.Status);
            Assert.Equal(60, result.Data.GrossSales);
            Assert.Equal(60, result.Data.NetSales);
            Assert.Equal(3, result.Data.TotalRegisteredAttendees);
        }

        [Fact]
        public async Task GetStatistics_WithEventId_ReturnsStatistics()
        {
            // Arrange
            var eventRepository = Substitute.For<IBaseRepository<Event>>();
            var ticketRepository = Substitute.For<IBaseRepository<Ticket>>();
            var statisticsService = new StatisticsService(eventRepository, ticketRepository);

            var eventId = Guid.NewGuid();
            var tickets = new List<Ticket>
            {
                new Ticket { EventId = eventId, Price = 10 },
                new Ticket { EventId = eventId, Price = 20 },
                new Ticket { EventId = eventId, Price = 30 }
            };
            ticketRepository.Query().Where(x => x.EventId == eventId).Returns(tickets.AsQueryable());
            
            // Act
            var result = await statisticsService.GetStatistics(eventId);

            // Assert
            Assert.True(result.Status);
            Assert.Equal(60, result.Data.GrossSales);
            Assert.Equal(60, result.Data.NetSales);
            Assert.Equal(3, result.Data.TotalRegisteredAttendees);
        }
    }
