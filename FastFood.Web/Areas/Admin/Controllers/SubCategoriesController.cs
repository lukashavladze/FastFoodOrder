using FastFood.Models;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var subCategory = _context.SubCategories.Include(x => x.Category).ToList();
            return View(subCategory);
        }
        [HttpGet]
        public IActionResult Create()
        {
            SubCategoryViewModel vm = new SubCategoryViewModel();
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title");
            return View(vm);
        }
        [HttpPost]

        public IActionResult Create(SubCategoryViewModel vm)
        {
            SubCategory model = new SubCategory();
            if (ModelState.IsValid)
            {
                model.Title = vm.Title;
                model.CategoryId = vm.CategoryId;
                _context.SubCategories.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Failed";
            return View(vm);
        }

        public IActionResult Edit(int id)
        {
            var subCategory = _context.SubCategories.FirstOrDefault(x => x.Id == id);

            if (subCategory == null)
            {
                // Handle the case where the subcategory with the specified ID is not found
                return NotFound();
            }

            var subCategoryViewModel = new SubCategoryViewModel
            {
                Id = subCategory.Id,
                Title = subCategory.Title,
                CategoryId = subCategory.CategoryId
            };

            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", subCategory.CategoryId);

            return View(subCategoryViewModel);
        }

        [HttpPost]
        public IActionResult Edit(SubCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var existingSubCategory = _context.SubCategories.FirstOrDefault(x => x.Id == vm.Id);

                if (existingSubCategory == null)
                {
                    // Handle the case where the subcategory with the specified ID is not found
                    return NotFound();
                }

                existingSubCategory.Title = vm.Title;
                existingSubCategory.CategoryId = vm.CategoryId;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // If ModelState is not valid, return to the edit view with validation errors
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", vm.CategoryId);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var subCategory = _context.SubCategories.Where(x=>x.Id == id).FirstOrDefault();
            if (subCategory != null)
            {
                _context.SubCategories.Remove(subCategory);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
