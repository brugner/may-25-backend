using May25.API.Core.Contracts.Repositories;
using System.Threading.Tasks;

namespace May25.API.Core.Contracts.UnitsOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        ICarRepository Cars { get; }
        ITripRepository Trips { get; }
        IPlaceRepository Places { get; }
        IMakeRepository Makes { get; }
        ISeatRequestRepository SeatRequests { get; }
        INotificationRepository Notifications { get; }
        IRatingRepository Ratings { get; }
        IPasswordResetRepository PasswordResets { get; }
        IMessageRepository Messages { get; }
        IAlertRepository Alerts { get; }

        Task<int> CompleteAsync();
    }
}
