// File 9: CustomerService.cs
using AutoMapper;
using InvoiceSystem.ErrorMessages;
using InvoiceSystem.Exceptions;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories;
using InvoiceSystem.Repositories.IRepositories;

namespace InvoiceSystem.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(ICustomerRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerDTO> AddCustomerAsync(CustomerDTO dto)
        {
            // Map DTO to entity
            var entity = _mapper.Map<Customer>(dto);

            try
            {
                // Add to repository
                await _repository.AddAsync(entity);

                // Save changes
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Error saving customer to the database.", ex);
            }

            return _mapper.Map<CustomerDTO>(entity);
        }

        public async Task<CustomerDTO?> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                throw new NotFoundExceptions(AllErrors.CustomerNotFound);

            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }
    }
}
