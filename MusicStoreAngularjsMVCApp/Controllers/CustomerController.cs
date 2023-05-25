using Microsoft.AspNet.Identity;
using MusicStoreAngularjsMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStoreAngularjsMVCApp.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        readonly MusicStoreAppEntities db = new MusicStoreAppEntities();

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MovieDetail(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public JsonResult Cart(int movieId)
        {
            int currentUserId = int.Parse(User.Identity.GetUserId());
            int currentCustomerId = db.Customers.Where(customer => customer.UserId == currentUserId).First().CustomerId;

            db.Carts.Add(new Cart()
            {
                MovieId = movieId,
                CustomerId = currentCustomerId
            });
            db.SaveChanges();

            return Json(new { Status = true, StatusMessage = "" });
        }
    }
}