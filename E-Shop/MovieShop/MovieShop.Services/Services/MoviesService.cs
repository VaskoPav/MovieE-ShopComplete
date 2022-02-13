using Microsoft.EntityFrameworkCore;
using MovieShop.DataAccess;
using MovieShop.DataAccess.Repositories.EntityBaseRepository;
using MovieShop.Models.Models;
using MovieShop.Services.Interfaces;
using MovieShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Services.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
    {
        private readonly MovieDbContext _dbContext;
        public MoviesService(MovieDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;

        }

        public async Task AddNewMovieAsync(MovieViewModels data)
        {
            var newMovie = new Movie()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageUrl = data.ImageURL,
                CinemaId = data.CinemaId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                MovieCategory = data.MovieCategory,
                ProducerId = data.ProducerId
            };
            await _dbContext.Movies.AddAsync(newMovie);
            await _dbContext.SaveChangesAsync();

            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId
                };
                await _dbContext.Actors_Movies.AddAsync(newActorMovie);
            }
            await _dbContext.SaveChangesAsync();

        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails = await _dbContext.Movies.Include(c => c.Cinema)
                                                .Include(p => p.Producer)
                                                .Include(a => a.Actors_Movies).ThenInclude(am => am.Actor)
                                                .FirstOrDefaultAsync(n => n.Id == id);
            return movieDetails;
        }

        public async Task<NewMovieDropdownsVM> GetNewMovieDropsDownsValues()
        {
            var response = new NewMovieDropdownsVM()
            {
                Actors = await _dbContext.Actors.OrderBy(a => a.FullName).ToListAsync(),
                Cinemas = await _dbContext.Cinemas.OrderBy(c => c.Name).ToListAsync(),
                Producers = await _dbContext.Producers.OrderBy(p => p.FullName).ToListAsync()
            };
            

            return response;
        }

        public async Task UpdateMovieAsync(MovieViewModels data)
        {
            var dbMovie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == data.Id);

            if(dbMovie != null)
            {
                dbMovie.Name = data.Name;
                dbMovie.Description = data.Description;
                dbMovie.Price = data.Price;
                dbMovie.ImageUrl = data.ImageURL;
                dbMovie.CinemaId = data.CinemaId;
                dbMovie.StartDate = data.StartDate;
                dbMovie.EndDate = data.EndDate;
                dbMovie.MovieCategory = data.MovieCategory;
                dbMovie.ProducerId = data.ProducerId;




                await _dbContext.SaveChangesAsync();
            }

            var existingActorsDb = _dbContext.Actors_Movies.Where(a => a.MovieId == data.Id).ToList();
            _dbContext.Actors_Movies.RemoveRange(existingActorsDb);
            await _dbContext.SaveChangesAsync();

            

            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = data.Id,
                    ActorId = actorId
                };
                await _dbContext.Actors_Movies.AddAsync(newActorMovie);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
