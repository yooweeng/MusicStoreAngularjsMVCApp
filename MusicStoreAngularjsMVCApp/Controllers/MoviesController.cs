using MusicStoreAngularjsMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStoreAngularjsMVCApp.Controllers
{
    public class MoviesController : Controller
    {
        readonly MusicStoreAppEntities db = new MusicStoreAppEntities();

        [HttpGet]
        public JsonResult Index(int? id)
        {
            bool status = false;
            string statusMessage = "Failed to retrive list of movie";

            // if id is passed in as parameter
            if(id != null)
            {
                status = false;
                statusMessage = "Failed to retrieve movie with respective id";

                MovieViewModel movie = null;

                Movie movieById = db.Movies.Where(movieItem => movieItem.Id == id).FirstOrDefault();

                if (movieById != null)
                {
                    List<string> genresByMovie = new List<string>();

                    // add genre that match the current movie id into a list
                    foreach (MovieGenre movieGenre in db.MovieGenres.Where(movieGenre => movieGenre.MovieId == movieById.Id))
                    {
                        genresByMovie.Add(movieGenre.Genre.GenreType);
                    }

                    movie = new MovieViewModel()
                    {
                        Id = movieById.Id,
                        MovieTitle = movieById.MovieTitle,
                        Description = movieById.Description,
                        ImageUrl = movieById.ImageUrl,
                        Price = movieById.Price,
                        ReleasedYear = movieById.ReleasedYear,
                        GenreTypes = genresByMovie,
                        Seller = new SellerViewModel()
                        {
                            Fname = movieById.Seller.Fname,
                            Lname = movieById.Seller.Lname
                        }
                    };

                    status = true;
                    statusMessage = "Successfully retrive movie";
                }

                return Json(
                    new
                    {
                        Status = status,
                        StatusMessage = statusMessage,
                        Movie = movie
                    }, JsonRequestBehavior.AllowGet);
            }

            // only when no id is passed in
            List<MovieViewModel> movies = new List<MovieViewModel>();
            List<GenreViewModel> genres = new List<GenreViewModel>();
            List<MovieGenre> movieGenres = db.MovieGenres.ToList();

            foreach (Movie movie in db.Movies)
            {
                List<string> genresByMovie = new List<string>();

                // add genre that match the current movie id into a list
                foreach (MovieGenre movieGenre in db.MovieGenres.Where(movieGenre => movieGenre.MovieId == movie.Id))
                {
                    genresByMovie.Add(movieGenre.Genre.GenreType);
                }

                movies.Add(new MovieViewModel()
                {
                    Id = movie.Id,
                    MovieTitle = movie.MovieTitle,
                    Description = movie.Description,
                    ImageUrl = movie.ImageUrl,
                    Price = movie.Price,
                    ReleasedYear = movie.ReleasedYear,
                    GenreTypes = genresByMovie
                });
            }

            foreach (Genre genre in db.Genres)
            {
                genres.Add(new GenreViewModel()
                {
                    Id = genre.Id,
                    GenreType = genre.GenreType
                });
            }

            status = true;
            statusMessage = "Successfully retrive list of movie";

            return Json(
                new { 
                        Status = status,
                        StatusMessage = statusMessage, 
                        Data = new { 
                            Movies = movies,
                            Genres = genres
                        } 
                }, JsonRequestBehavior.AllowGet);
        }
    }
}