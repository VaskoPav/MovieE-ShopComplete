using MovieShop.DataAccess;
using MovieShop.DataAccess.Repositories.EntityBaseRepository;
using MovieShop.Models.Models;
using MovieShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Services.Services
{
    public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
    {
        public CinemasService(MovieDbContext dbContext) : base(dbContext) { }
    }
}
 