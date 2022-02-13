using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieShop.DataAccess;
using MovieShop.Services.Interfaces;
using MovieShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;
        public MoviesController(IMoviesService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var allMovies = await _service.GetAllAsync(n=>n.Cinema);
            return View(allMovies);
        }


        public async Task<IActionResult> Filter(string searchString)
        {
            var allMovies = await _service.GetAllAsync(n => n.Cinema);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filterResult = allMovies.Where(m => m.Name.Contains(searchString) || m.Description.Contains(searchString)).ToList();
                return View("Index", filterResult);
            }

            return View("Index",allMovies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var movieDetail = await _service.GetMovieByIdAsync(id);
            return View(movieDetail);
        }

        public async Task<IActionResult> Create()
        {
            var movieDropdownsData = await _service.GetNewMovieDropsDownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MovieViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _service.AddNewMovieAsync(model);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int id)
        {

            var movieDetails = await _service.GetMovieByIdAsync(id);

            if (movieDetails == null) return View("NotFound");

            var reponse = new MovieViewModels()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                StartDate=movieDetails.StartDate,
                EndDate=movieDetails.EndDate,
                ImageURL = movieDetails.ImageUrl,
                MovieCategory = movieDetails.MovieCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                ActorIds = movieDetails.Actors_Movies.Select(a => a.ActorId).ToList(),
            };


            var movieDropdownsData = await _service.GetNewMovieDropsDownsValues();

            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

            return View(reponse);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieViewModels model)
        {
            if (id != model.Id) return View("NotFound");


            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _service.GetNewMovieDropsDownsValues();

                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");

                return View(model);
            }


            await _service.UpdateMovieAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
