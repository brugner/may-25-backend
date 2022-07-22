using May25.API.Core.Contracts.Repositories;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Extensions;
using May25.API.Data.DbContexts;
using System.Threading.Tasks;

namespace May25.API.Data.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository Users { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public ICarRepository Cars { get; private set; }
        public ITripRepository Trips { get; private set; }
        public IPlaceRepository Places { get; private set; }
        public IMakeRepository Makes { get; private set; }
        public ISeatRequestRepository SeatRequests { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IRatingRepository Ratings { get; private set; }
        public IPasswordResetRepository PasswordResets { get; private set; }
        public IMessageRepository Messages { get; private set; }
        public IAlertRepository Alerts { get; private set; }

        public UnitOfWork(
            AppDbContext context,
            IUserRepository users,
            IRoleRepository roles,
            ICarRepository cars,
            ITripRepository trips,
            IPlaceRepository places,
            IMakeRepository makes,
            ISeatRequestRepository seatRequests,
            INotificationRepository notificationTokens,
            IRatingRepository ratings,
            IPasswordResetRepository passwordResets,
            IMessageRepository messages,
            IAlertRepository alerts)
        {
            _context = context;

            Users = users.ThrowIfNull(nameof(users));
            Roles = roles.ThrowIfNull(nameof(roles));
            Cars = cars.ThrowIfNull(nameof(cars));
            Trips = trips.ThrowIfNull(nameof(trips));
            Places = places.ThrowIfNull(nameof(places));
            Makes = makes.ThrowIfNull(nameof(makes));
            SeatRequests = seatRequests.ThrowIfNull(nameof(seatRequests));
            Notifications = notificationTokens.ThrowIfNull(nameof(notificationTokens));
            Ratings = ratings.ThrowIfNull(nameof(ratings));
            PasswordResets = passwordResets.ThrowIfNull(nameof(passwordResets));
            Messages = messages.ThrowIfNull(nameof(messages));
            Alerts = alerts.ThrowIfNull(nameof(alerts));
        }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
