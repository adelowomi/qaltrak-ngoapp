namespace NGOAPP;

using NGOAPP.Models.AppModels;
using NGOAPP.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class Context : IdentityDbContext<User, Role, Guid>
{
    public Context(DbContextOptions<Context> options)
          : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
    }

    public DbSet<Code> Codes { get; set; }
    public DbSet<EventCategory> EventCategories { get; set; }
    public DbSet<EventSubCategory> EventSubCategories { get; set; }
    public DbSet<EventType> EventTypes { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<LocationType> LocationTypes { get; set; }
    public DbSet<TicketType> TicketTypes { get; set; }

    public DbSet<Event> Events { get; set; }
    public DbSet<EventTicket> EventTickets { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupFollow> GroupFollows { get; set; }
    public DbSet<GroupUser> GroupUsers { get; set; }
    public DbSet<AdminSchedule> AdminSchedules { get; set; }
}
