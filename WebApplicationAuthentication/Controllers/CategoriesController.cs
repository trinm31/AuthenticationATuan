using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAuthentication.Data;
using WebApplicationAuthentication.Models;

namespace WebApplicationAuthentication.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        //------------------------- Index -------------------------
        [HttpGet]
        public IActionResult Index()
        {
            var categoryList = _db.Categories.ToList();
            return View(categoryList);
        }
        
        //------------------------- UpSert -------------------------
        [HttpGet]
        [Authorize]
        public IActionResult UpSert(int? id)
        {
            if (id == 0 || id == null)
            {
                return View(new Category());
            }

            var category = _db.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpSert(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (category.Id == 0 || category.Id == null)
            {
                _db.Categories.Add(category);
            }
            else
            {
                _db.Categories.Update(category);
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        [Authorize]
        //------------------------- Delete -------------------------
        public IActionResult Delete(int id)
        {
            var category = _db.Categories.Find(id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        
    }
}