﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MusicStoreAngularjsMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStoreAngularjsMVCApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        readonly MusicStoreAppEntities db = new MusicStoreAppEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ApprovalList()
        {
            bool status;
            string statusMessage;

            List<ApprovalList> approvalList = db.ApprovalLists.ToList();
            List<AdminIndexApprovalListViewModel> adminIndexApprovalList = new List<AdminIndexApprovalListViewModel>();

            foreach (ApprovalList approvalListItem in approvalList)
            {
                adminIndexApprovalList.Add(new AdminIndexApprovalListViewModel
                {
                    Id = approvalListItem.Id,
                    SellerEmail = approvalListItem.SellerEmail,
                    SellerFname = approvalListItem.SellerFname,
                    SellerLname = approvalListItem.SellerLname,
                    Address = approvalListItem.Address,
                    PhoneNumber = approvalListItem.PhoneNumber,
                    Status = approvalListItem.Status
                });
            }
            status = true;
            statusMessage = "Successfully retrive approval list";

            return Json(new
            {
                Status = status,
                StatusMessage = statusMessage,
                ApprovalList = adminIndexApprovalList
            },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approve(int Id)
        {
            // update approval list status
            ApprovalList approvalItemById = db.ApprovalLists.SingleOrDefault(item => item.Id == Id);
            approvalItemById.Status = 1;

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByName(approvalItemById.SellerEmail);

            // insert record into seller table
            Seller insertedSeller = db.Sellers.Add(new Seller()
            {
                Fname = approvalItemById.SellerFname,
                Lname = approvalItemById.SellerLname,
                Address = approvalItemById.Address,
                PhoneNumber = approvalItemById.PhoneNumber,
                UserId = Convert.ToInt32(user.Id)
            });

            approvalItemById.SellerId = insertedSeller.SellerId;

            db.SaveChanges();

            return Json(new { 
                Status = true,
                StatusMessage = "Successfully updated the status of selected seller account to approve"
            });
        }

        [HttpPost]
        public JsonResult Reject(int Id)
        {
            ApprovalList approvalItemById = db.ApprovalLists.SingleOrDefault(item => item.Id == Id);
            approvalItemById.Status = 2;
            db.SaveChanges();

            return Json(new
            {
                Status = true,
                StatusMessage = "Successfully updated the status of selected seller account to reject"
            });
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Genre(string GenreType)
        {
            db.Genres.Add(new Genre() { GenreType = GenreType });
            db.SaveChanges();

            return RedirectToAction("AddCategory");
        }
    }
}