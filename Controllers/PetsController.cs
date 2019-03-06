using Microsoft.AspNetCore.Mvc;
using parrot.Models;
using parrot.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace parrot.Controllers
{
        public class PetsController : Controller
    {
        // public IActionResult Index()
        // {
        //     return View();
        // }

        static IList<Pet> _pets;
        public async Task<ActionResult> Index()
        {
            var petresp = await PetService.GetPetsAsync();
            _pets = petresp.pets;
            ViewData["Pets"] = _pets;
            // ViewData["NumTimes"] = numTimes;

            return View();
        }

        public async Task<ActionResult> Delete(int? id){
            var resp = await PetService.DeletePetAsync(id.ToString());
            var s = (resp.ToString());
            ViewBag.Messages = new []{
                new AlertViewModel("success", "Deleted!", $"The petId {id} was deleted successfully!"),
            };
             var petresp = await PetService.GetPetsAsync();
             _pets = petresp.pets;
            ViewData["Pets"] = _pets;
            return RedirectToAction(nameof(Index));
        }

        // GET: Employee/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Pet());
            else
                return View(_pets.FirstOrDefault(x=>x.id.Equals(id.ToString())));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("id,created,name,animal_type,tags")] Pet pet)
        {
            if (ModelState.IsValid)
            {
               var resp = await PetService.CreatePetAsync(pet.id,pet);
                return RedirectToAction(nameof(Index));
            }
            return View(pet);
        }

    }
}