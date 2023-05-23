using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStoreAngularjsMVCApp.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public int ReleasedYear { get; set; }
        public IEnumerable<string> GenreTypes { get; set; }
    }
}