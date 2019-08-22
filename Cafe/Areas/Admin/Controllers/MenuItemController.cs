using Cafe.Data;
using Cafe.Models;
using Cafe.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {


        private readonly ApplicationDbContext dbContext;
        private readonly IHostingEnvironment hostingEnvironment;

        [BindProperty]
        public MenuViewModel MenuViewModel { get; set; }

        public MenuItemController(ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {


            this.dbContext = dbContext;
            this.hostingEnvironment = hostingEnvironment;


            MenuViewModel = new MenuViewModel()
            {
                Category = dbContext.Categoris,
                MenuItem = new Models.MenuItem(),
                SubCategories = dbContext.SubCategories
            };

        }


        public async Task<IActionResult> Index()
        {


            List<MenuItem> menuItems = await dbContext.MenuItems
                                      .Include(m => m.Category)
                                      .Include(m => m.SubCategory)
                                      .ToListAsync();
            return View(menuItems);
        }

        //CREATE

        public IActionResult Create()
        {
            return View(MenuViewModel);
        }
    }
}