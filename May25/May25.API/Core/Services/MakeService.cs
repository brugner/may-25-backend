using AutoMapper;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class MakeService : IMakeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MakeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
        }

        public async Task<IEnumerable<MakeDTO>> GetAllAsync()
        {
            var makes = await _unitOfWork.Makes.GetAllAsync();

            return _mapper.Map<IEnumerable<MakeDTO>>(makes);
        }
    }
}
