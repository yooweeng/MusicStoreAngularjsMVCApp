using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStoreAngularjsMVCApp.Controllers
{
    public class OrderHistoriesController : Controller
    {
        readonly MusicStoreAppEntities db = new MusicStoreAppEntities();

        // GET: OrderHistories
        public JsonResult Index(string orderStatus)
        {
            bool status = false;
            string statusMessage = "Failed to retrive order history list";

            orderStatus = orderStatus == null ? "pending" : orderStatus;

            ViewBag.Status = orderStatus;

            int currentUserId = int.Parse(User.Identity.GetUserId());
            int currentCustomerId = db.Customers.Where(customer => customer.UserId == currentUserId).First().CustomerId;

            IEnumerable<OrderMovie> ordersByCustomerIdAndStatus = db.OrderMovies.Where(order => (order.Order.CustomerId == currentCustomerId) &&
                                                                                (order.Order.Status == orderStatus))
                                                                                .OrderByDescending(order => order.Order.Date)
                                                                                .ThenByDescending(order => order.Order.Id).AsEnumerable();

            var orders = ordersByCustomerIdAndStatus.Select(order => new { 
                OrderId = order.Order.Id, 
                OrderDate = order.Order.Date.Value.ToShortDateString(),
                OrderStatus = order.Order.Status,
                MovieImageUrl = order.Movie.ImageUrl,
                order.Quantity,
                order.UnitPrice 
            });

            var groupOrderByOrderId = orders.GroupBy(order => order.OrderId)
                                            .Select(items => new { 
                                                Orders =items, 
                                                Total = items.Sum(order => order.UnitPrice * order.Quantity)
                                            }).ToList();

            status = true;
            statusMessage = "Successfully retrive order history list";

            return Json(new
            {
                Status = status,
                StatusMessage = statusMessage,
                OrderHistories = groupOrderByOrderId
            }, JsonRequestBehavior.AllowGet);
        }
    }
}