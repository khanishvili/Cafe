using Cafe.Data;
using Cafe.Models;
using Cafe.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cafe.Models;

namespace Cafe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        [TempData]
        public string StatusMessage { get; set; }
        public SubCategoryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            StatusMessage = "Success";
        }
        public IActionResult Index()
        {
            List<SubCategory> model = dbContext.SubCategories.Include(s => s.Category).ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            SubCategoryandCategoryModel model = new SubCategoryandCategoryModel()
            {
                Categories = dbContext.Categoris.ToList(),
                SubCategory = new Models.SubCategory(),
                SubCategoryListName = dbContext.SubCategories.OrderBy(sub => sub.Name).Select(sub => sub.Name).Distinct().ToList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubCategoryandCategoryModel submodel)
        {
            if (ModelState.IsValid)
            {
                var categoryExists = dbContext.SubCategories
                    .Include(sub => sub.Category)
                    .Where(s => s.Name == submodel.SubCategory.Name && s.CategoryID == submodel.SubCategory.CategoryID);
                if (categoryExists.Count() > 0)
                {
                    StatusMessage = "Error"+ submodel.SubCategory.Name + "Already Exists!";
                }
                else
                {
                    dbContext.SubCategories.Add(submodel.SubCategory);
                    dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            var _model = new SubCategoryandCategoryModel()
            {
                Categories = dbContext.Categoris.ToList(),
                SubCategory = submodel.SubCategory,
                SubCategoryListName = dbContext.SubCategories.OrderBy(sub => sub.Name).Select(s => s.Name).ToList(),
                StatusMessage = StatusMessage
            };
            return View(_model);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            subCategories =  await(from subcategory in dbContext.SubCategories
                             where subcategory.CategoryID == id
                             select subcategory).ToListAsync();

            return Json(new SelectList(subCategories, "ID", "Name"));


        }

        // EDIT GET
        [HttpGet]
        public async Task<IActionResult> Edit(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var subCategory = await dbContext.SubCategories.SingleOrDefaultAsync(sub => sub.ID == ID);

            if (subCategory == null)
            {
                return NotFound();
            }

            SubCategoryandCategoryModel model = new SubCategoryandCategoryModel()
            {
                Categories = dbContext.Categoris.ToList(),
                SubCategory = subCategory,
                SubCategoryListName = dbContext.SubCategories.OrderBy(sub => sub.Name).Select(sub => sub.Name).Distinct().ToList()
            };
            return View(model);
        }
        // EDIT post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int ID, SubCategoryandCategoryModel submodel)
        {
            if (ModelState.IsValid)
            {
                var categoryExists = dbContext.SubCategories
                    .Include(sub => sub.Category)
                    .Where(s => s.Name == submodel.SubCategory.Name && s.CategoryID == submodel.SubCategory.ID);
                if (categoryExists.Count() > 0)
                {
                    StatusMessage = "Error" + submodel.SubCategory.Name + "Already Exists!";
                }
                else
                {
                    dbContext.SubCategories.Add(submodel.SubCategory);
                    dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            var _model = new SubCategoryandCategoryModel()
            {
                Categories = dbContext.Categoris.ToList(),
                SubCategory = submodel.SubCategory,
                SubCategoryListName = dbContext.SubCategories.OrderBy(sub => sub.Name).Select(s => s.Name).ToList(),
                StatusMessage = StatusMessage
            };
            return View(_model);
        }
    }
}