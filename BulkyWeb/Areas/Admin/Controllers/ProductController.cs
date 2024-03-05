using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnviroment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnviroment = webHostEnviroment;

        }
        public IActionResult Index()
        {
            //Get everything to show in index
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            
            return View(objProductList);

        }

        //Upsert -> update and insert
        //Id id exists ->update
        public IActionResult Upsert(int? id)
        {

            //ViewBag.CategoryList = categoryList;
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            
            //If id is null or cero the product dosn't exists,so create new
            if (id==null || id == 0)
            {
                //create
                return View(productVM);

            }
            //if id exists -> update
            else 
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(x => x.Id == id);
                return View(productVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                //Get the wwwroot path from project
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                if(file != null)
                {
                    //generate random number(guid) to replace name of file
                    //then grab the extension of the file ej: .jpeg,.png
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //Where the product is going to be stored
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    using(var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                //If is not valid it populates the category drop down
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }

        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitOfWork.Product.Get(x => id == x.Id);
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Category Updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Product? obj = _unitOfWork.Product.Get(x => id == x.Id);

            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(Product obj)
        {

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");



        }

    }
}
