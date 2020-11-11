using Cryptocop.Software.API.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;
using System.Linq;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;

        public ShoppingCartService(ICryptoCurrencyService cryptoCurrencyService, IShoppingCartRepository shoppingCartRepository)
        {
            _cryptoCurrencyService = cryptoCurrencyService;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            return _shoppingCartRepository.GetCartItems(email);
        }

        public Task AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem)
        {
            var cryptos = _cryptoCurrencyService.GetAvailableCryptocurrencies().Result.ToList();

            var cryptoCoin = cryptos.Find(c => c.Symbol == shoppingCartItemItem.ProductIdentifier.ToUpper());

            var addItem = new Task(() => _shoppingCartRepository.AddCartItem(email, shoppingCartItemItem, cryptoCoin.Price_usd));
            addItem.Start();
            addItem.Wait();
            return addItem;
        }

        public void RemoveCartItem(string email, int id)
        {
            _shoppingCartRepository.RemoveCartItem(email, id);
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            _shoppingCartRepository.UpdateCartItemQuantity(email, id, quantity);
        }

        public void ClearCart(string email)
        {
            _shoppingCartRepository.ClearCart(email);
        }

        public void DeleteCart(string email)
        {
            //TOOD: see
            _shoppingCartRepository.DeleteCart(email);
        }
    }
}
