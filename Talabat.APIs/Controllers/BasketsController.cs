using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{
    
    public class BasketsController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketsController(IBasketRepository basketRepository) 
        {
            _basketRepository = basketRepository;
        }

        [HttpGet] //GET : /api/baskets
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return basket is null ? new CustomerBasket(id) : basket;
        }
        
        [HttpPost] // POST : /api/baskets
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            if (createdOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(createdOrUpdatedBasket);
        }

        [HttpDelete] //DELETE : /api/baskets
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
