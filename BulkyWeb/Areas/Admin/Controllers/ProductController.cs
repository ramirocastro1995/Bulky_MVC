using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

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
            if (id == null || id == 0)
            {
                //create
                return View(productVM);

            }
            //if id exists -> update
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(x => x.Id == id, includeProperties : "ProductImages");
                return View(productVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile>? files)
        {
            if (ModelState.IsValid)
            {


                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();
                //Get the wwwroot path from project
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {

                        //generate random number(guid) to replace name of file
                        //then grab the extension of the file ej: .jpeg,.png
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        //Where the product is going to be stored
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if(!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }


                        using(var fileStream = new FileStream(Path.Combine(finalPath,fileName),FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl=@"\"+productPath+@"\" + fileName,
                            ProductId = productVM.Product.Id,

                        };

                        if(productVM.Product.ProductImages == null)
                        {
                            productVM.Product.ProductImages = new List<ProductImage>();
                        }

                        productVM.Product.ProductImages.Add(productImage);
                        //_unitOfWork.ProductImage.Add(productImage);

                    }
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();



                    //replace old file if new image uploaded
                    //if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    //{
                    //    //Delete old image
                    //    var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                    //    if(System.IO.File.Exists(oldImagePath))
                    //    {
                    //        System.IO.File.Delete(oldImagePath);
                    //    }
                    //}

                    //using(var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    //{
                    //    file.CopyTo(fileStream);
                    //}
                    //productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
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

        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDelete = _unitOfWork.ProductImage.Get(x => x.Id == imageId);
            int productId = imageToBeDelete.ProductId;

            if(imageToBeDelete != null)
            {
                if(!string.IsNullOrEmpty(imageToBeDelete.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnviroment.WebRootPath, imageToBeDelete.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.ProductImage.Remove(imageToBeDelete);
                _unitOfWork.Save();

                TempData["success"] = "Deleted successfully";
            }
            return RedirectToAction(nameof(Upsert), new {id=productId});
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

        //public IActionResult Delete(int? id)
        //{
        //    if(id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? obj = _unitOfWork.Product.Get(x => id == x.Id);

        //    if(obj == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(obj);

        //}

        //[HttpPost,ActionName("Delete")]
        //public IActionResult DeletePOST(Product obj)
        //{

        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Category deleted successfully";
        //    return RedirectToAction("Index");



        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(x => x.Id == id);

            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            //var oldImagePath = Path.Combine(_webHostEnviroment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    System.IO.File.Delete(oldImagePath);
            //}
            string productPath = @"images\products\product-" + id;
            //Where the product is going to be stored
            string finalPath = Path.Combine(_webHostEnviroment.WebRootPath, productPath);

            if (!Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach(var filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);

                }


                Directory.Delete(finalPath);
            }


            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successfull" });
        }
        #endregion

    }
}
