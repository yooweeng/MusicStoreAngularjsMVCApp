using Microsoft.AspNet.Identity;
using MusicStoreAngularjsMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStoreAngularjsMVCApp.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        readonly MusicStoreAppEntities db = new MusicStoreAppEntities();

        // GET: Seller
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MovieDetail()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Movies(int? id)
        {
            bool status = false;
            string statusMessage = "Failed to retrive list of movie";

            // if id is passed in as parameter
            if (id != null)
            {
                status = false;
                statusMessage = "Failed to retieve movie with respective id";

                Movie movieByIdDb = db.Movies.Where(movie => movie.Id == id).SingleOrDefault();

                if (movieByIdDb != null)
                {
                    var genresByMovieId = db.MovieGenres.Where(movieGenre => movieGenre.MovieId == movieByIdDb.Id)
                                                    .Select(movieGenre => movieGenre.Genre.GenreType);

                    var movieById = new
                    {
                        movieByIdDb.Id,
                        movieByIdDb.MovieTitle,
                        movieByIdDb.Description,
                        movieByIdDb.ImageUrl,
                        movieByIdDb.Price,
                        movieByIdDb.ReleasedYear,
                        Genres = genresByMovieId,
                        Seller = new { 
                            movieByIdDb.Seller.Fname,
                            movieByIdDb.Seller.Lname
                        }
                    };

                    var genres = db.Genres.Select(genre => genre.GenreType).ToList();

                    status = true;
                    statusMessage = "Successfully retieve movie with respective id";

                    return Json(
                    new
                    {
                        Status = status,
                        StatusMessage = statusMessage,
                        Movie = movieById,
                        Genres = genres
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            // only when no id is passed in
            int currentUserId = int.Parse(User.Identity.GetUserId());
            int currentSellerId = db.Sellers.Where(seller => seller.UserId == currentUserId).First().SellerId;

            var moviesByCurrentSeller = db.Movies.Where(movie => movie.SellerId == currentSellerId)
                                                    .Select(movie => new { 
                                                        movie.Id,
                                                        movie.MovieTitle,
                                                        movie.Description,
                                                        movie.ImageUrl,
                                                        Genres = movie.MovieGenres.Select(movieGenre => movieGenre.Genre.GenreType)
                                                    }).ToList();

            status = true;
            statusMessage = "Successfully retrive list of movie";

            return Json(
                new
                {
                    Status = status,
                    StatusMessage = statusMessage,
                    Movies = moviesByCurrentSeller
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Movies(Movie movie, List<int> selectedGenresId, HttpPostedFileBase file)
        {
            int currentUserId = int.Parse(User.Identity.GetUserId());
            int currentSellerId = db.Sellers.Where(seller => seller.UserId == currentUserId).First().SellerId;

            string folderDirectory = "/MovieCover";
            string filename = "";

            // insert into Movie table without imageUrl
            var insertedMovie = db.Movies.Add(new Movie()
            {
                MovieTitle = movie.MovieTitle,
                Description = movie.Description,
                Price = movie.Price,
                ReleasedYear = movie.ReleasedYear,
                SellerId = currentSellerId
            });
            db.SaveChanges();

            // save movie cover
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~" + folderDirectory + "/"),
                                               insertedMovie.Id.ToString());
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filename = Path.GetFileName(file.FileName);
                    path = Path.Combine(path, filename);
                    file.SaveAs(path);
                }
                catch (Exception ex)
                { Debug.WriteLine(ex); }
            }

            // update movie for imageUrl
            insertedMovie.ImageUrl = folderDirectory + "/" + insertedMovie.Id.ToString() + "/" + filename;

            // insert into MovieGenre table
            foreach (int genreId in selectedGenresId)
            {
                db.MovieGenres.Add(new MovieGenre()
                {
                    MovieId = insertedMovie.Id,
                    GenreId = genreId
                });
            }
            db.SaveChanges();

            return Json(new { Status = true, StatusMessage = "Successfully created movie", RedirectURL = "/Seller" });
        }

        [HttpPost]
        public ActionResult EditMovie(int id, Movie movie, List<int> selectedGenresId)
        {
            Movie movieById = db.Movies.Where(movieItem => movieItem.Id == id).SingleOrDefault();

            if (movieById != null)
            {
                // update movie
                movieById.MovieTitle = movie.MovieTitle;
                movieById.Description = movie.Description;
                movieById.Price = movie.Price;
                movieById.ReleasedYear = movie.ReleasedYear;

                // update genre
                List<MovieGenre> movieGenreByMovieIds = db.MovieGenres.Where(movieGenre => movieGenre.MovieId == id).ToList();
                // remove all genre
                foreach(MovieGenre item in movieGenreByMovieIds)
                {
                    db.MovieGenres.Remove(item);
                }
                // insert new genre
                foreach(int selectedGenreId in selectedGenresId)
                {
                    db.MovieGenres.Add(new MovieGenre() { MovieId = id, GenreId = selectedGenreId });
                }

                db.SaveChanges();
            }

            return RedirectToAction("MovieDetail", new { id = id });
        }

        [HttpPost]
        public JsonResult DeleteMovie(int id)
        {
            List<MovieGenre> movieGenresByMovieId = db.MovieGenres.Where(movieGenre => movieGenre.MovieId == id).ToList();
            foreach (MovieGenre movieGenre in movieGenresByMovieId)
            {
                db.MovieGenres.Remove(movieGenre);
            }

            Movie movieById = db.Movies.Where(movie => movie.Id == id).FirstOrDefault();
            db.Movies.Remove(movieById);
            db.SaveChanges();

            return Json(new { Status = true, StatusMessage = "Successfully remove movie record" });
        }


        public ActionResult AddMovie()
        {
            List<Genre> genres = db.Genres.ToList();

            AddEditMovieViewModel model = new AddEditMovieViewModel() { Genres = genres };

            return View(model);
        }

        public ActionResult Order()
        {
            int currentUserId = int.Parse(User.Identity.GetUserId());
            int currentSellerId = db.Sellers.Where(seller => seller.UserId == currentUserId).Single().SellerId;

            List<OrderMovie> orders = db.OrderMovies.Where(order => order.Movie.SellerId == currentSellerId)
                                                     .OrderByDescending(order => order.Order.Date)
                                                     .ThenByDescending(order => order.Order.Id).ToList();

            return View(orders);
        }

        [HttpPost]
        public JsonResult Order(int id, string orderStatus)
        {
            bool status = false;
            string statusMessage = "Failed to update the order status";

            Order orderById = db.Orders.Where(order => order.Id == id).SingleOrDefault();

            if(orderById != null)
            {
                orderById.Status = orderStatus;
                db.SaveChanges();

                status = true;
                statusMessage = "Successfully update the order status";
            }

            return Json(new { Status = status, StatusMessage = statusMessage });
        }
    }
}