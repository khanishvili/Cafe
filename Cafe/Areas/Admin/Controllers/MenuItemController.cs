﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


using Cafe.Data;
using Cafe.Models;
using Cafe.Models.ViewModels;
using Cafe.Utility;
using System.Linq;

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

        //CREATE - 
        [HttpGet]
        public IActionResult Create()
        {
            return View(MenuViewModel);
        }

        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            MenuViewModel.MenuItem.SubCategoryID = Convert.ToInt32(Request.Form["SubCategoryID"].ToString());
            if (!ModelState.IsValid)
            {
                return View(MenuViewModel);
            }
            dbContext.MenuItems.Add(MenuViewModel.MenuItem);
            await dbContext.SaveChangesAsync();
            //Saving Image Section

            string WebRootPath = hostingEnvironment.WebRootPath;
            Microsoft.AspNetCore.Http.IFormFileCollection files = HttpContext.Request.Form.Files;
            MenuItem menuItemFromDb = await dbContext.MenuItems.FindAsync(MenuViewModel.MenuItem.ID);

            if (files.Count > 0)

            {
                // files has been uploaded
                string uploads = Path.Combine(WebRootPath, "images");
                string extension = Path.GetExtension(files[0].FileName);
                using (FileStream fileStream = new FileStream(Path.Combine(uploads, MenuViewModel.MenuItem.ID + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                menuItemFromDb.Image = @"\images\" + MenuViewModel.MenuItem.ID + extension;
            }
            else
            {
                //no files was uploaded

                var uploads = Path.Combine(WebRootPath, @"\images\" + SD.DefaultFoodImage);
                System.IO.File.Copy(uploads, WebRootPath + @"\images\" + MenuViewModel.MenuItem.ID + ".jpg");
                menuItemFromDb.Image = @"\images\" + MenuViewModel.MenuItem.ID + ".jpg";
            }
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        //Edit - 
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            MenuViewModel.MenuItem = await dbContext.MenuItems.Include(s => s.Category).Include(sub => sub.SubCategory).SingleOrDefaultAsync(m => m.ID == id);
            MenuViewModel.SubCategories = await dbContext.SubCategories.Where(s => s.CategoryID == MenuViewModel.MenuItem.SubCategoryID).ToListAsync();

            if (MenuViewModel.MenuItem == null)
            {
                NotFound();
            }



            return View(MenuViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EdiPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            MenuViewModel.MenuItem.SubCategoryID = Convert.ToInt32(Request.Form["SubCategoryID"].ToString());
            if (!ModelState.IsValid)
            {
                MenuViewModel.SubCategories = await dbContext.SubCategories.Where(s=>s.CategoryID ==MenuViewModel.MenuItem.CategoryID).ToListAsync();
                return View(MenuViewModel);
            }
 
            //Saving Image Section

            string WebRootPath = hostingEnvironment.WebRootPath;
            Microsoft.AspNetCore.Http.IFormFileCollection files = HttpContext.Request.Form.Files;
            MenuItem menuItemFromDb = await dbContext.MenuItems.FindAsync(MenuViewModel.MenuItem.ID);

            if (files.Count > 0)

            {
                // New Image will  upload
                string uploads = Path.Combine(WebRootPath, "images");
                string extension_new = Path.GetExtension(files[0].FileName);

                var imagepath = Path.Combine(WebRootPath, menuItemFromDb.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                //we will upload new file
                using (FileStream fileStream = new FileStream(Path.Combine(uploads, MenuViewModel.MenuItem.ID + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                menuItemFromDb.Image = @"\images\" + MenuViewModel.MenuItem.ID + extension_new;
            }

            menuItemFromDb.Name = MenuViewModel.MenuItem.Name;
            menuItemFromDb.Description = MenuViewModel.MenuItem.Description;
            menuItemFromDb.Price = MenuViewModel.MenuItem.Price;
            menuItemFromDb.Taste = MenuViewModel.MenuItem.Taste;
            menuItemFromDb.CategoryID = MenuViewModel.MenuItem.CategoryID;
            menuItemFromDb.SubCategoryID = MenuViewModel.MenuItem.SubCategoryID;


            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


    }
}