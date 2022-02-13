using Microsoft.AspNetCore.Mvc;
using MovieShop.Services.Cart;
using MovieShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMoviesService _service;
        private readonly ShopingCart _shopingCart;
        private readonly IOrdersService _orderService;

        public OrdersController(IMoviesService service, ShopingCart shopingCart, IOrdersService orderService)
        {
            _service = service;
            _shopingCart = shopingCart;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders =await _orderService.GetOrdersByUserIdAndRoleAsyncs(userId, userRole);
            return View(orders);
        }



        public IActionResult ShopingCart()
        {
            var items = _shopingCart.GetShopingCartItems();
            _shopingCart.ShopingCartItems = items;
            var responce = new ShopingCartViewModel()
            {
                ShopingCart = _shopingCart,
                ShopingCartTotal = _shopingCart.GetShopingCartTotal()
            };

            return View(responce);
        }
        public async Task<IActionResult> AddToShopingCart(int id)
        {
            var item =await _service.GetMovieByIdAsync(id);

            if(item != null)
            {
                _shopingCart.AddItemToCart(item);
                
            }
            return RedirectToAction(nameof(ShopingCart));
        }

        public async Task<IActionResult> RemoveItemFromShopingCart(int id)
        {
            var item = await _service.GetMovieByIdAsync(id);

            if (item != null)
            {
                _shopingCart.RemoveItemFromCart(item);

            }
            return RedirectToAction(nameof(ShopingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shopingCart.GetShopingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAdrdress = User.FindFirstValue(ClaimTypes.Email);

            await _orderService.StoreOrderAsync(items, userId, userEmailAdrdress);

            await _shopingCart.ClearShopingCartAsync();

            return View();
        }
    }
}
