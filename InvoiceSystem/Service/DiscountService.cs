using AutoMapper;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Repositories.IRepositories;

namespace InvoiceSystem.Service
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiscountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(DiscountDTO discountDto)
        {
            var discount = _mapper.Map<Discount>(discountDto);
            await _unitOfWork.Discounts.AddAsync(discount);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<DiscountDTO>> GetAllAsync()
        {
            var discounts = await _unitOfWork.Discounts.GetAllAsync();
            return _mapper.Map<List<DiscountDTO>>(discounts);
        }
    }
}