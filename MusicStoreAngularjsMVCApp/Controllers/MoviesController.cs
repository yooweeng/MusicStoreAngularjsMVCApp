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
        public JsonResult Index()
        {
            bool status = false;
            string statusMessage = "Failed to retrive list of movie";

            List<MovieViewModel> movies = new List<MovieViewModel>();
            List<GenreViewModel> genres = new List<GenreViewModel>();
            List<MovieGenre> movieGenres = db.MovieGenres.ToList();
            List<MovieIdGenreIdModel> movieIdGenreIds = new List<MovieIdGenreIdModel>();

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

            foreach (MovieGenre movieGenre in movieGenres)
            {
                movieIdGenreIds.Add(new MovieIdGenreIdModel()
                {
                    MovieId = movieGenre.MovieId,
                    GenreId = movieGenre.GenreId
                });
            }

            GuestCustomerIndexViewModel model = new GuestCustomerIndexViewModel()
            {
                Movies = movies,
                Genres = genres,
                MovieIdGenreIds = movieIdGenreIds
            };

            status = true;
            statusMessage = "Successfully retrive list of movie";

            return Json(new { Status = status, StatusMessage = statusMessage, Data = model }, JsonRequestBehavior.AllowGet);
        }
    }
}