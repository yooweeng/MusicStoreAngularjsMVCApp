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

        [HttpPost]
        public JsonResult RemoveCarts(int movieId)
        {
            bool status = false;
            string statusMessage = "Failed to remove item from the cart";

            Cart cartItemByMovieId = db.Carts.Where(cart => cart.MovieId == movieId).FirstOrDefault();

            if (cartItemByMovieId != null)
            {
                db.Carts.Remove(cartItemByMovieId);
                db.SaveChanges();
                status = true;
                statusMessage = "Successfully remove item from the cart";
            }

            return Json(new { Status = status, StatusMessage = statusMessage });
        }

        public ActionResult Checkout(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        [HttpPost]
        public JsonResult Checkout(List<int> selectedMovieIds)
        {
            bool status = false;
            string statusMessage = "Failed to checkout items from the cart";
            int orderId = 0;

            if (selectedMovieIds != null)
            {
                int currentUserId = int.Parse(User.Identity.GetUserId());
                int currentCustomerId = db.Customers.Where(customer => customer.UserId == currentUserId).First().CustomerId;
                int quantity;

                List<Movie> moviesBySelectedMovieId = new List<Movie>();
                Dictionary<int, Order> orderDict = new Dictionary<int, Order>();

                // get list of movie by the movieId
                foreach (int movieId in selectedMovieIds)
                {
                    Movie movie = db.Movies.Where(m => m.Id == movieId).Single();
                    moviesBySelectedMovieId.Add(movie);
                }
                // group the list of movie by sellerid, number of group here represent number of unique seller
                foreach (var group in moviesBySelectedMovieId.GroupBy(movie => movie.SellerId))
                {
                    // create new order
                    Order order = db.Orders.Add(new Order() { Date = DateTime.Now, Status = "pending", CustomerId = currentCustomerId });
                    // add the order to dictionary:
                    // - sellerid (key)
                    // - order (value)
                    orderDict.Add(group.Key, order);
                }
                db.SaveChanges();

                foreach (int movieId in selectedMovieIds)
                {
                    Movie movie = db.Movies.Where(m => m.Id == movieId).Single();
                    List<Cart> cartItemsByMovieId = db.Carts.Where(cartItem => (cartItem.MovieId == movieId) &&
                                                                                (cartItem.CustomerId == currentCustomerId)).ToList();
                    quantity = cartItemsByMovieId.Count;

                    // remove from cart
                    foreach (Cart cartItem in cartItemsByMovieId)
                    {
                        db.Carts.Remove(cartItem);
                    }

                    OrderMovie orderMovie = db.OrderMovies.Add(new OrderMovie()
                    {
                        MovieId = movieId,
                        UnitPrice = movie.Price,
                        Quantity = quantity,
                        Order = orderDict[movie.SellerId]
                    });
                    orderId = orderMovie.OrderId.HasValue ? orderMovie.OrderId.Value : 0;
                }
                db.SaveChanges();
                status = true;
                statusMessage = "Successfully checkout items from the cart";
            }

            return Json(new { Status = status, StatusMessage = statusMessage, OrderId = orderId });
        }

        public ActionResult OrderHistory()
        {
            return View();
        }
    }
}