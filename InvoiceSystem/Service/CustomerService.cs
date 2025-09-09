using AutoMapper;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;

namespace InvoiceSystem.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CustomerDTO> AddCustomerAsync(CustomerDTO dto)
        {
            // Error for duplicate name/email       
            var (nameTaken, emailTaken) = await _unitOfWork.Customers.WhichExistAsync(dto.Name, dto.Email);

            if (nameTaken || emailTaken)
            {
                var reasons = new List<string>();
                if (nameTaken) reasons.Add("name");
                if (emailTaken) reasons.Add("email");

                var detail = reasons.Count == 2
                    ? "A customer with this name and email already exists."
                    : $"A customer with this {reasons[0]} already exists.";

                throw new ArgumentException(detail); // middleware => 400
            }

            var entity = _mapper.Map<Customer>(dto);
            await _unitOfWork.Customers.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CustomerDTO>(entity);
        }

        public async Task<CustomerDTO?> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            return customer == null ? null : _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all customers...");
            var customers = await _unitOfWork.Customers.GetAllAsync();

            if (!customers.Any())
                _logger.LogWarning("No customers found in the system.");

            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }
    }
}
