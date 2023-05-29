using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStoreAngularjsMVCApp.Controllers
{
    public class OrdersController : Controller
    {
        readonly MusicStoreAppEntities db = new MusicStoreAppEntities();

        // GET: Order
        public JsonResult Index()
        {
            bool status = false;
            string statusMessage = "Failed to retrive list of order";

            int currentUserId = int.Parse(User.Identity.GetUserId());
            int currentSellerId = db.Sellers.Where(seller => seller.UserId == currentUserId).Single().SellerId;

            var orders = db.OrderMovies.Where(order => order.Movie.SellerId == currentSellerId)
                                                     .GroupBy(order => order.Order).AsEnumerable()
                                                     .Select(ordersGroupedByOrderId => new { 
                                                         OrderId = ordersGroupedByOrderId.Key.Id,
                                                         OrderDate = ordersGroupedByOrderId.Key.Date.Value.ToShortDateString(),
                                                         OrderStatus = ordersGroupedByOrderId.Key.Status,
                                                         Total = ordersGroupedByOrderId.Sum(order => order.UnitPrice * order.Quantity),
                                                         OrderItems = ordersGroupedByOrderId.Select(order => new
                                                         {
                                                             order.Id,
                                                             MovieImageUrl = order.Movie.ImageUrl,
                                                             order.UnitPrice,
                                                             order.Quantity
                                                         })
                                                     })
                                                     .OrderByDescending(ordersGroupedByOrderId => ordersGroupedByOrderId.OrderDate)
                                                     .ThenByDescending(ordersGroupedByOrderId => ordersGroupedByOrderId.OrderId);

            status = true;
            statusMessage = "Successfully retrive list of order";

            return Json(new
            {
                Status = status,
                StatusMessage = statusMessage,
                Orders = orders
            }, JsonRequestBehavior.AllowGet);
        }
    }
}