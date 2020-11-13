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
        private readonly IQueueService _queueService;

        public OrderService(IOrderRepository orderRepository, IShoppingCartService shoppingCartService, IQueueService queueService)
        {
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
            _queueService = queueService;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            return _orderRepository.GetOrders(email);
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            const string routingKey = "create-order";
            var retOrder = _orderRepository.CreateNewOrder(email, order);
            _queueService.PublishMessage(routingKey, retOrder);
            _shoppingCartService.DeleteCart(email);
            return retOrder;
        }
    }
}