using MovieShop.DataAccess.Repositories;
using MovieShop.Models.Models;
using MovieShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Services.Interfaces
{
    public interface IMoviesService: IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task<NewMovieDropdownsVM> GetNewMovieDropsDownsValues();

        Task AddNewMovieAsync(MovieViewModels data);

        Task UpdateMovieAsync(MovieViewModels data);
    }
}
