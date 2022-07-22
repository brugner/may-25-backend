using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Entities.Keyless;
using May25.API.Data.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace May25.API.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<SeatRequest> SeatRequests { get; set; }
        public DbSet<TripPassenger> TripPassengers { get; set; }
        public DbSet<NotificationToken> NotificationTokens { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RecentlyCompletedTrip> RecentlyCompletedTrips { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserEntityConfiguration());
            builder.ApplyConfiguration(new RoleEntityConfiguration());
            builder.ApplyConfiguration(new UserRolesEntityConfiguration());
            builder.ApplyConfiguration(new CarEntityConfiguration());
            builder.ApplyConfiguration(new PlaceEntityConfiguration());
            builder.ApplyConfiguration(new TripEntityConfiguration());
            builder.ApplyConfiguration(new MakeEntityConfiguration());
            builder.ApplyConfiguration(new ModelEntityConfiguration());
            builder.ApplyConfiguration(new TripPassengerEntityConfiguration());
            builder.ApplyConfiguration(new SeatRequestEntityConfiguration());
            builder.ApplyConfiguration(new NotificationTokenEntityConfiguration());
            builder.ApplyConfiguration(new RatingEntityConfiguration());
            builder.ApplyConfiguration(new NotificationEntityConfiguration());
            builder.ApplyConfiguration(new RecentlyCompletedTripEntityConfiguration());
            builder.ApplyConfiguration(new PasswordResetEntityConfiguration());
            builder.ApplyConfiguration(new MessageEntityConfiguration());
            builder.ApplyConfiguration(new AlertEntityConfiguration());
        }
    }
}
