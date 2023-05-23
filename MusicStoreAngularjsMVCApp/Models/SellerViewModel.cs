using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStoreAngularjsMVCApp.Models
{
    public class SellerViewModel
    {
        public int SellerId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}