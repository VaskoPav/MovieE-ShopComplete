using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Services.Cart;

namespace MovieShop.ViewComponents
{
    public class ShoppingCartSummary:ViewComponent
    {
        private readonly ShopingCart _shopingCart;

        public ShoppingCartSummary(ShopingCart shopingCart)
        {
            _shopingCart = shopingCart;
        }
        public IViewComponentResult Invoke()
        {
            var items = _shopingCart.GetShopingCartItems();
            return View(items.Count);
        }
    }
}
