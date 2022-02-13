using MovieShop.DataAccess.Repositories;
using MovieShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Services.Interfaces
{
    public interface IActorService : IEntityBaseRepository<Actor>
    {
        
    }
}
