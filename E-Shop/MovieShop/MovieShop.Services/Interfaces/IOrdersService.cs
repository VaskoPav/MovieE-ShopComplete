using MovieShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Services.Interfaces
{
    public interface IOrdersService
    {
        Task StoreOrderAsync(List<ShopingCartItem> items, string userId, string userEmailAdress);
        Task<List<Order>> GetOrdersByUserIdAndRoleAsyncs(string userId, string userRole);

    }
}
