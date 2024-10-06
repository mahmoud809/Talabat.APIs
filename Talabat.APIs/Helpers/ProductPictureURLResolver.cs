using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureURLResolver : IValueResolver<Product, ProductToReturn, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureURLResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturn destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseURL"]}/{source.PictureUrl}";
            
            return string.Empty;
        }
    }
}
