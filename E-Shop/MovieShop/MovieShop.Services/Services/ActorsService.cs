using Microsoft.EntityFrameworkCore;
using MovieShop.DataAccess;
using MovieShop.DataAccess.Repositories.EntityBaseRepository;
using MovieShop.Models.Models;
using MovieShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Services.Services
{
    public class ActorsService :EntityBaseRepository<Actor>, IActorService
    {
       
        public ActorsService(MovieDbContext dbContext) : base(dbContext) { }
       

    }
}
