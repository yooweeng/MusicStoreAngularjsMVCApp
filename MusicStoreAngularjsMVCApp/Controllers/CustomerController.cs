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

        public ActionResult Cart()
        {
            return View();
        }

        public JsonResult Carts()
        {
            bool status = false;
            string statusMessage = "Failed to retrive list of cart";
            List<CartViewModel> carts = new List<CartViewModel>();

            int currentUserId = int.Parse(User.Identity.GetUserId());
            int currentCustomerId = db.Customers.Where(customer => customer.UserId == currentUserId).First().CustomerId;

            List<Cart> cartsFromDb = db.Carts.Where(cartItem => cartItem.CustomerId == currentCustomerId)
                                            .OrderBy(cart => cart.MovieId).ToList();
            foreach(Cart cartItem in cartsFromDb)
            {
                carts.Add(new CartViewModel()
                {
                    Id = cartItem.Id,
                    Movie = new MovieViewModel()
                    {
                        MovieTitle = cartItem.Movie.MovieTitle,
                        Price = cartItem.Movie.Price,
                        Id =cartItem.Movie.Id,
                        ImageUrl = cartItem.Movie.ImageUrl,
                        Seller =new SellerViewModel()
                        {
                            SellerId = cartItem.Movie.Seller.SellerId,
                            Fname = cartItem.Movie.Seller.Fname,
                            Lname = cartItem.Movie.Seller.Lname
                        }
                    },
                    CustomerId = cartItem.CustomerId
                });
            }
            status = true;
            statusMessage = "Successfully retrive list of cart";

            return Json(new { Status = status, StatusMessage = statusMessage, Carts = carts }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Carts(int movieId)
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