using AutoMapper;
using InvoiceSystem.Exceptions;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories;
using InvoiceSystem.Repositories.IRepositories;
using InvoiceSystem.Service;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InvoiceSystem.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            // Arrange: create substitutes for dependencies
            _repository = Substitute.For<ICustomerRepository>();
            _mapper = Substitute.For<IMapper>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _service = new CustomerService(_repository, _mapper, _unitOfWork);
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldReturnCustomerDTO_WhenSuccessful()
        {
            // Arrange
            var dto = new CustomerDTO { Id = 2, Name = "Mihrilda Shehu", Email = "mihrilda_shehu@universitetipolis.edu.al" };
            var entity = new Customer { Id = 2, Name = "Mihrilda Shehu", Email = "mihrilda_shehu@universitetipolis.edu.al" };

            //Isolate the service logic by mocking the mapper behavior  
            _mapper.Map<Customer>(dto).Returns(entity);
            _mapper.Map<CustomerDTO>(entity).Returns(dto);

            // Act (call the method that needs to be tested )
            var result = await _service.AddCustomerAsync(dto);

            // Assert
            await _repository.Received(1).AddAsync(entity);
            await _unitOfWork.Received(1).SaveAsync();
            Assert.Equal(dto.Id, result.Id);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Email, result.Email);
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldThrowDatabaseException_WhenSaveFails()
        {
            // Arrange
            var dto = new CustomerDTO { Id = 2, Name = "Test", Email = "test@example.com" };
            var entity = new Customer { Id = 2, Name = "Test", Email = "test@example.com" };

            _mapper.Map<Customer>(dto).Returns(entity);
            _unitOfWork.When(u => u.SaveAsync()).Do(x => throw new Exception("DB error"));

            // Act
            var exception = await Assert.ThrowsAsync<DatabaseException>(
                () => _service.AddCustomerAsync(dto));

            // Assert
            Assert.Contains("Error saving customer to the database", exception.Message);
        }


    }
}
