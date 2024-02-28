using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //Create an instance the dbcontext in private
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            //Then inside the constructor,asign it to the actual instance,to use it in all the class
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            //Create a list and populate it with all the information inside Category table
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
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
            Category? categoryFromDb = _unitOfWork.Category.Get(x => x.Id == id);
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
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
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
            Category? categoryFromDb = _unitOfWork.Category.Get(x => id == x.Id);
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        //To use the post method

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            //Optional special validation
            Category? obj = _unitOfWork.Category.Get(x => id == x.Id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);

            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
            // return View();
        }


    }
}
