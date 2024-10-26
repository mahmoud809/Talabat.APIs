using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.Product_Spec;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        ///private readonly IGenericRepository<Product> _productsRepo;
        ///private readonly IGenericRepository<ProductBrand> _brandsRepo;
        ///private readonly IGenericRepository<ProductCategory> _categoriesRepo;
        private readonly IMapper _mapper;

        public ProductsController
            (
            ///IGenericRepository<Product> productsRepo,
            ///IGenericRepository<ProductBrand> brandsRepo,
            ///IGenericRepository<ProductCategory> categoriesRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            ///_productsRepo = productsRepo;
            ///_brandsRepo = brandsRepo;
            ///_categoriesRepo = categoriesRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //[Authorize]
        [HttpGet]   //{{baseUrl}}/api/Products 
        public async Task<ActionResult<Pagination<ProductToReturn>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturn>>(products);

            var countSpec = new ProductWithFilterationAndCountSpecifications(specParams);

            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);

            return Ok(new Pagination<ProductToReturn>(specParams.PageIndex , specParams.PageSize , count ,data));
        }

        [ProducesResponseType(typeof(ProductToReturn) , StatusCodes.Status200OK)] //just Improving for Swagger Docs.
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] //{{baseUrl}}/api/Products
        public async Task<ActionResult<ProductToReturn>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
            
            if(product is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product , ProductToReturn>(product));
        }

        [HttpGet("brands")] // GET: {{baseURL}}/api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return Ok(brands);
        }

        [HttpGet("categories")] // GET : {{baseURL}}/api/products/categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return Ok(categories);
        }
    }
}
