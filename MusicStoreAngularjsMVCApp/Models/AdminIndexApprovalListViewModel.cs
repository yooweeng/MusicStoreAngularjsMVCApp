﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStoreAngularjsMVCApp.Models
{
    public class AdminIndexApprovalListViewModel
    {
        public int Id { get; set; }
        public string SellerFname { get; set; }
        public string SellerLname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public byte Status { get; set; }
        public string SellerEmail { get; set; }
    }
}