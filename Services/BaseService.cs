using AutoMapper;
using INVENTORY.SERVER.Services.Interfaces;

namespace INVENTORY.SERVER.Services
{
    public class BaseService
    {
        protected readonly IMapper Mapper;
        protected readonly ILoggerManager _logger;

        public BaseService(IMapper mapper, ILoggerManager loggerManager)
        {
            Mapper = mapper;
            _logger = loggerManager;
        }
    }
}

