using Microsoft.EntityFrameworkCore;
using MovieShop.DataAccess;
using MovieShop.Models.Models;
using MovieShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Services.Services
{
    public class OrderService : IOrdersService
    {
        private readonly MovieDbContext _context;

        public OrderService(MovieDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsyncs(string userId, string userRole)
        {
            var orders = await _context.Orders.Include(o => o.OrderItems).ThenInclude(m => m.Movie).Include(n=>n.User).ToListAsync();

            if(userRole != "Admin")
            {
                orders.Where(o => o.UserId == userId).ToList();
            }

            return orders;
        }

        public async Task StoreOrderAsync(List<ShopingCartItem> items, string userId, string userEmailAdress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAdress
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    MovieId = item.Movie.Id,
                    OrderId = order.Id,
                    Price = item.Movie.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();
        }
    }
}
