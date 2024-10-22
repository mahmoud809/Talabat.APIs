using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodsRepo;
        private readonly IGenericRepository<Order> _ordersRepo;

        public OrderService(
            IBasketRepository basketRepository,
            IGenericRepository<Product> productRepo,
            IGenericRepository<DeliveryMethod> deliveryMethodsRepo,
            IGenericRepository<Order> ordersRepo
            )
        {
            _basketRepository = basketRepository;
            _productRepo = productRepo;
            _deliveryMethodsRepo = deliveryMethodsRepo;
            _ordersRepo = ordersRepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get Basket From Basket Repo.

            var basket = await _basketRepository.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo.

            var orderItems = new List<OrderItem>();
            if(basket?.Items?.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _productRepo.GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }
            // 3. Calculate subTotal.

            var subTotal = orderItems.Sum(Item => Item.Price * Item.Quantity);

            // 4. Get Delivery Method From DeliveryMethodRepo.

            var deliveryMethod = await _deliveryMethodsRepo.GetAsync(deliveryMethodId);

            // 5. Create Order

            var order = new Order(buyerEmail , shippingAddress , deliveryMethod , orderItems , subTotal);

            await _ordersRepo.Add(order);

            // 6. Save To Data base
            //We Should Implement Unit Of work DP
            
        }

        public Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
