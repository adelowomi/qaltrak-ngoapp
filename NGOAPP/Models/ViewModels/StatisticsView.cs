namespace NGOAPP;

public class StatisticsView
{
    public decimal GrossSales { get; set; }
    public decimal NetSales { get; set; }
    public int TotalRegisteredAttendees { get; set; }
}

public class StatisticsEventView
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime DateCreated { get; set; }
    public int TotalRegisteredAttendees { get; set; }
    public string GroupName { get; set; }
}

public class RevenueByTicketTypeView 
{
    public string TicketType { get; set; }
    public double Revenue { get; set; }
    public int TotalTicketsSold { get; set; }
}

public class EventAttendeeDetailsView
{
    public UserView User { get; set; }
    public Ticket Ticket { get; set; }
}