using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BulkyWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		
		public IActionResult Index()
		{
			return View();
		}

        public IActionResult Details(int orderId)
        {
			OrderVM orderVM = new()
			{
				OrderHeader =_unitOfWork.OrderHeader.Get(x => x.Id == orderId, includeProperties:"ApplicationUser"),
				OrderDetail = _unitOfWork.OrderDetail.GetAll(x => x.OrderHeaderId == orderId, includeProperties: "Product")

			};
            return View(orderVM);
        }

        #region API CALLS
        [HttpGet]
		public IActionResult GetAll(string status)
		{
			IEnumerable<OrderHeader> objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

			switch (status)
			{
				case "pending":
                    //This is for the payments,so this "pending" status is diferent
                    objOrderHeaders = objOrderHeaders.Where(x => x.PaymentStatus == SD.PaymentStatusDelayedPayment);
					break;
				case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(x => x.OrderStatus == SD.StatusInProcess);
					break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(x => x.OrderStatus == SD.StatusShipped);
					break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(x => x.OrderStatus == SD.StatusApproved);
					break;
                default:
					break;
			}



			return Json(new { data = objOrderHeaders });
		}

		#endregion

	}
}
