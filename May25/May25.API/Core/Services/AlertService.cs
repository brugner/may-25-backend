using AutoMapper;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Exceptions;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class AlertService : IAlertService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _authUser;

        public AlertService(IUnitOfWork unitOfWork, IMapper mapper, ClaimsPrincipal authUser)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _authUser = authUser.ThrowIfNull(nameof(authUser));
        }

        public async Task<AlertDTO> CreateAsync(AlertForCreationDTO alertForCreation)
        {
            var userId = _authUser.GetId();

            var alert = _mapper.Map<Alert>(alertForCreation);
            alert.ValidUntil = DateTime.Now.AddDays(30);
            alert.UserId = userId;

            _unitOfWork.Alerts.Add(alert);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AlertDTO>(alert);
        }

        public async Task DeleteAlertAsync(int id)
        {
            var userId = _authUser.GetId();
            var alert = await _unitOfWork.Alerts.GetAsync(id, userId);

            if (alert == null)
            {
                throw new NotFoundException($"Alert {id} not found");
            }

            _unitOfWork.Alerts.Remove(alert);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<AlertDTO> GetAlertAsync(int id)
        {
            var userId = _authUser.GetId();
            var alert = await _unitOfWork.Alerts.GetAsync(id, userId);

            if (alert == null)
            {
                throw new NotFoundException($"Alert {id} not found");
            }

            return _mapper.Map<AlertDTO>(alert);
        }

        public async Task<IEnumerable<AlertDTO>> GetMyAlerts()
        {
            var userId = _authUser.GetId();
            var alerts = await _unitOfWork.Alerts.GetAllAsync(userId);

            return _mapper.Map<IEnumerable<AlertDTO>>(alerts);
        }
    }
}
