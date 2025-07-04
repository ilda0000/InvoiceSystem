using AutoMapper;
using InvoiceSystem.Models.DTO;
using InvoiceSystem.Models.Entity;

namespace InvoiceSystem.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerDTO, Customer>();
            CreateMap<Customer, CustomerDTO>();

            CreateMap<PlanDTO, Plan>();
            CreateMap<Plan, PlanDTO>()
                .ForMember(dest => dest.PricePerMonth, opt =>
                    opt.MapFrom(src => $"{src.PricePerMonth} LEKË"));

            CreateMap<Subscription, SubscriptionDTO>();
            CreateMap<SubscriptionDTO, Subscription>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Let DB generate Id
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddMonths(3)))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            CreateMap<DiscountDTO, Discount>();

            CreateMap<Payment, PaymentDTO>();
            CreateMap<PaymentDTO, Payment>() 
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentId)) // Corrected
                .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethodName)); // Adjusted mapping

            CreateMap<Invoice, InvoiceDTO>()
                .ForMember(dest => dest.DiscountApplied, opt =>
                    opt.MapFrom(src =>
                        (src.Subscription != null && src.Subscription.Plan != null)
                            ? src.Subscription.Plan.PricePerMonth - src.TotalAmount
                            : 0));
        }
    }
}