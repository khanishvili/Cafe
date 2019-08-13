using Cafe.Data;
using Cafe.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cafe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var model = dbContext.Categoris.ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        //Create Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                dbContext.Categoris.Add(category);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? categoryID)
        {
            if (categoryID != null)
            {
                Category model = dbContext.Categoris.Find(categoryID);
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                dbContext.Categoris.Update(category);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
        }

        [HttpGet]
         public IActionResult Delete(int? CategoryID)
        {
            if (CategoryID != null)
            {
                var model = dbContext.Categoris.Find(CategoryID);
                if (model == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category category)
        {
            if (ModelState.IsValid)
            {
                dbContext.Categoris.Remove(category);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }
        }
    }
}