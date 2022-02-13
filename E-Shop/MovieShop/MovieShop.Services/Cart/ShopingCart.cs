using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieShop.DataAccess;
using MovieShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Services.Cart
{
    public class ShopingCart
    {
        public MovieDbContext _context { get; set; }
        public string ShopingCartId { get; set; }
        public List<ShopingCartItem> ShopingCartItems { get; set; }

        public ShopingCart(MovieDbContext context)
        {
            _context = context;
        }

       public static ShopingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<MovieDbContext>();

            string cardId = session.GetString("CardId") ?? Guid.NewGuid().ToString();
            session.SetString("CardId", cardId);
            return new ShopingCart(context) { ShopingCartId = cardId };

        }


        public void AddItemToCart(Movie movie)
        {
            var shopingCartItem = _context.ShopingCartItems.FirstOrDefault(s => s.Movie.Id == movie.Id && s.ShopingCartId == ShopingCartId);
            if (shopingCartItem == null)
            {
                shopingCartItem = new ShopingCartItem()
                {
                    ShopingCartId = ShopingCartId,
                    Movie = movie,
                    Amount = 1
                };
                _context.ShopingCartItems.Add(shopingCartItem);
            }
            else
            {
                shopingCartItem.Amount++;
            }
            _context.SaveChanges();
        }
        public void RemoveItemFromCart(Movie movie)
        {
            var shopingCartItem = _context.ShopingCartItems.FirstOrDefault(s => s.Movie.Id == movie.Id && s.ShopingCartId == ShopingCartId);
            if (shopingCartItem != null)
            {
                if(shopingCartItem.Amount > 1)
                {
                    shopingCartItem.Amount--;
                }
                else
                {
                    _context.ShopingCartItems.Remove(shopingCartItem);
                }
                
            }
           
            _context.SaveChanges();

        }

        public List<ShopingCartItem> GetShopingCartItems()
        {
            return ShopingCartItems ?? (ShopingCartItems = _context.ShopingCartItems.Where(n => n.ShopingCartId == ShopingCartId).Include(s => s.Movie).ToList());
        }
        public double GetShopingCartTotal()
        {
            var total = _context.ShopingCartItems.Where(n => n.ShopingCartId == ShopingCartId).Select(n => n.Movie.Price * n.Amount).Sum();
            return total;
        }

        public async Task ClearShopingCartAsync()
        {
            var items = await _context.ShopingCartItems.Where(n => n.ShopingCartId == ShopingCartId).ToListAsync();
            _context.ShopingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
