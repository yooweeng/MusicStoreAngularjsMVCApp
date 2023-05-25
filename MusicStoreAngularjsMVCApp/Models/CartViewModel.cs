using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStoreAngularjsMVCApp.Models
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public MovieViewModel Movie { get; set; }
        public int CustomerId { get; set; }
    }
}