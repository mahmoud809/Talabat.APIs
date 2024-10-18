using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturn>()
                .ForMember(D => D.Brand , O => O.MapFrom(S => S.Brand.Name))
                .ForMember(D => D.Category , O => O.MapFrom(S => S.Category.Name))
                .ForMember(D => D.PictureUrl , O => O.MapFrom<ProductPictureURLResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();
            
            CreateMap<CustomerBasketDto , CustomerBasket>();
            CreateMap<BasketItemDto , BasketItem>();
        }
    }
}
