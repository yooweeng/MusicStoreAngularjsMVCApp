using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStoreAngularjsMVCApp.Controllers
{
    public class GenresController : Controller
    {
        readonly MusicStoreAppEntities db = new MusicStoreAppEntities();

        // GET: Genres
        public JsonResult Index()
        {
            bool status;
            string statusMessage;

            var genres = db.Genres.Select(genre => new { genre.Id, genre.GenreType }).ToList();
            status = true;
            statusMessage = "Successfully retrive list of genre";

            return Json(new
            {
                Status = status,
                StatusMessage = statusMessage,
                Genres = genres
            }, JsonRequestBehavior.AllowGet);
        }

    }
}