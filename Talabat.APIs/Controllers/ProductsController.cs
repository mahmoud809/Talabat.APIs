using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.Product_Spec;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductCategory> _categoriesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductBrand> brandsRepo,
            IGenericRepository<ProductCategory> categoriesRepo,
            IMapper mapper)
        {
            _productsRepo = productsRepo;
            _brandsRepo = brandsRepo;
            _categoriesRepo = categoriesRepo;
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

        [ProducesResponseType(typeof(ProductToReturn) , StatusCodes.Status200OK)] //just Improving for Swagger Docs.
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] //{{baseUrl}}/api/Products
        public async Task<ActionResult<ProductToReturn>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var product = await _productsRepo.GetWithSpecAsync(spec);
            
            if(product is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product , ProductToReturn>(product));
        }

        [HttpGet("brands")] // GET: {{baseURL}}/api/products/brands
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRepo.GetAllAsync();

            return Ok(brands);
        }

        [HttpGet("categories")] // GET : {{baseURL}}/api/products/categories
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetCategories()
        {
            var categories = await _categoriesRepo.GetAllAsync();
            return Ok(categories);
        }
    }
}
