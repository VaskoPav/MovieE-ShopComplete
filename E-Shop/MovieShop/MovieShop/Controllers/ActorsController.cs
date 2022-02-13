using Microsoft.AspNetCore.Mvc;
using MovieShop.DataAccess;
using MovieShop.Models.Models;
using MovieShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorService _services;

        public ActorsController(IActorService services)
        {
            _services = services;
        }
        public async Task<IActionResult> Index()
        {
            var data =await _services.GetAllAsync();
            return View(data);
        }

        public IActionResult Create()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureUrl,Bio")] Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _services.AddAsync(actor);
            return  RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails =await _services.GetByIdAsync(id);
            if (actorDetails == null) return View("Not Found");
            return View(actorDetails);
        }

        public async  Task<IActionResult> Edit(int id)
        {

            var actorDetails = await _services.GetByIdAsync(id);
            if (actorDetails == null) return View("Not Found");
            return View(actorDetails);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureUrl,Bio")] Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            await _services.UpdateAsync(id,actor);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {

            var actorDetails = await _services.GetByIdAsync(id);
            if (actorDetails == null) return View("Not Found");
            return View(actorDetails);
        }
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await _services.GetByIdAsync(id);
            if (actorDetails == null) return View("Not Found");

            await _services.DeleteAsync(id);
            
            return RedirectToAction(nameof(Index));

        }
    }
}
