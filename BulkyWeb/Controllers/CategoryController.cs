using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        //Create an instance the dbcontext in private
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            //Then inside the constructor,asign it to the actual instance,to use it in all the class
            _db = db;

        }
        public IActionResult Index()
        {
            //Create a list and populate it with all the information inside Category table
            List<Category> objCategoryList = _db.Categories.ToList();
            //pass the list to the view
            return View(objCategoryList);
        }

        //Create a new function that retuns IActionResult with what you need for new button

        //Just return the view
        public IActionResult Create()
        {
            //We dont need to send nothing to the view because we are creating it in the view
            return View();
        }

        //To use the post method
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //Optional special validation
            if (obj.Name.ToLower() == obj.DisplayOrder.ToString().ToLower())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        //To use the post method
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            //Optional special validation

            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        //To use the post method

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            //Optional special validation
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);

            _db.SaveChanges();

            return RedirectToAction("Index");
            // return View();
        }


    }
}
