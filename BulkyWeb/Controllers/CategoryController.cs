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
        public IActionResult Create() 
        {
            //We dont need to send nothing to the view because we are creating it in the view
            return View();
        }
    }
}
