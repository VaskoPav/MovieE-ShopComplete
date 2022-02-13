using MovieShop.DataAccess.Repositories;
using MovieShop.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieShop.Services.Interfaces
{
    public interface ICinemasService: IEntityBaseRepository<Cinema>
    {
    }
}
