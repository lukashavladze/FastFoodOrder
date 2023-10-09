using FastFood.Models;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var items = _context.Items.Include(x=>x.Category).Include(y=>y.SubCategory).Select(model=> new ItemViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                SubCategoryId = model.SubCategoryId,
                Category = model.Category,
                subCategory = model.SubCategory

            }).ToList();
            return View(items);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ItemViewModel vm = new ItemViewModel();
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title");
            ViewBag.SubCategory = new SelectList(_context.SubCategories, "Id", "Title"); // unda miebas category-s
            return View();
        }
        [HttpGet]
        public IActionResult GetSubCategory(int id)
        {
            var subCategory = _context.SubCategories.Where(x=>x.CategoryId==id).FirstOrDefault();

            return Json(subCategory);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModel vm) 
        {
            Item model = new Item();
            if (ModelState.IsValid)
            {
                if (vm.ImageUrl != null && vm.ImageUrl.Length > 0) 
                {
                    var uploadDir = @"Images/Items";
                    var filename = Guid.NewGuid().ToString() + "-" + vm.ImageUrl.FileName;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, uploadDir, filename);
                    await vm.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    model.Image = "/" + uploadDir + "/" + filename;
                }
                model.Price = vm.Price;
                model.Description = vm.Description;
                model.Title = vm.Title;
                model.CategoryId = vm.CategoryId;
                model.SubCategoryId = vm.SubCategoryId;
                _context.Items.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }  return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _context.Items.Include(x => x.Category).Include(y => y.SubCategory)
        .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
               
                return NotFound();
            }
            var itemViewModel = new ItemViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Price = item.Price,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                Category = item.Category,
                subCategory = item.SubCategory
            };
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", item.CategoryId);
            ViewBag.SubCategory = new SelectList(_context.SubCategories, "Id", "Title", item.SubCategoryId);
            return View(itemViewModel);
        }
        // edit-i gasaketebelia
        [HttpPost]
        public IActionResult Edit(ItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _context.Items.FirstOrDefault(x => x.Id == vm.Id);

                if (existingItem == null)
                {
                    // Handle the case where the item with the specified ID is not found
                    return NotFound();
                }

                existingItem.Price = vm.Price;
                existingItem.Description = vm.Description;
                existingItem.Title = vm.Title;
                existingItem.CategoryId = vm.CategoryId;
                existingItem.SubCategoryId = vm.SubCategoryId;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // If ModelState is not valid, return to the edit view with validation errors
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", vm.CategoryId);
            ViewBag.SubCategory = new SelectList(_context.SubCategories, "Id", "Title", vm.SubCategoryId);
            return View(vm);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var item = _context.Items.Where(x => x.Id == id).FirstOrDefault();
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
