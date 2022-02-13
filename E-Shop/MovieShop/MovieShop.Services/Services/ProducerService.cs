using MovieShop.DataAccess;
using MovieShop.DataAccess.Repositories.EntityBaseRepository;
using MovieShop.Models.Models;
using MovieShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Services.Services
{
    public class ProducerService: EntityBaseRepository<Producer>, IProducerService
    {
        public ProducerService(MovieDbContext dbContext) : base(dbContext) { }
        
    }
}
