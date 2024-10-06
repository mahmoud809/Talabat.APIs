using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.Product_Spec;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo , IMapper mapper)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
        }

        //{{baseUrl}}/api/Products 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturn>>> GetProducts()
        {
            var spec = new ProductWithBrandAndCategorySpecifications();

            var products = await _productsRepo.GetAllWithSpecAsync(spec);
           
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturn>>(products));
        }


        //{{baseUrl}}/api/Products 
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturn>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var product = await _productsRepo.GetWithSpecAsync(spec);
            
            if(product is null)
                return NotFound();

            return Ok(_mapper.Map<Product , ProductToReturn>(product));
        }
    }
}
