using System.Collections.Generic;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartService _shoppingCartService;

        public OrderService(IOrderRepository orderRepository, IShoppingCartService shoppingCartService)
        {
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            throw new System.NotImplementedException();
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            //TODO: Publish message to RabbitMQ with routing key "create-order"
            var retOrder = _orderRepository.CreateNewOrder(email, order);
            //_shoppingCartService.DeleteCart(email);
            return retOrder;
        }
    }
}