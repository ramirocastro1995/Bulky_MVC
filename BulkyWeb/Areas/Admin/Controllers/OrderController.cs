using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

namespace BulkyWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
		public OrderVM OrderVM { get;set; }
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
            OrderVM = new()
			{
				OrderHeader =_unitOfWork.OrderHeader.Get(x => x.Id == orderId, includeProperties:"ApplicationUser"),
				OrderDetail = _unitOfWork.OrderDetail.GetAll(x => x.OrderHeaderId == orderId, includeProperties: "Product")

			};
            return View(OrderVM);
        }

		[HttpPost]
		[Authorize(Roles= SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult UpdateOrderDetail(int orderId)
        {
			var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(x => x.Id == OrderVM.OrderHeader.Id);
			orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
			orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
			orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
			orderHeaderFromDb.City = OrderVM.OrderHeader.City;
			orderHeaderFromDb.State = OrderVM.OrderHeader.State;
			orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
			if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
			{
				orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
			}
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.TrackingNumber;
            }
			_unitOfWork.OrderHeader.Update(orderHeaderFromDb);
			_unitOfWork.Save();
			TempData["Success"] = "Order Details Updated Successfully";

            return RedirectToAction(nameof(Details),new {orderId = orderHeaderFromDb.Id});
        }

        #region API CALLS
        [HttpGet]
		public IActionResult GetAll(string status)
		{
			IEnumerable<OrderHeader> objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

			if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
			{
				objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
			}
			else
			{
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.ApplicationUserId == userId, includeProperties: "ApplicationUser");

			}

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
