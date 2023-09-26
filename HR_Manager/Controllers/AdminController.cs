using HR_Manager.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_Manager.Controllers
{
    public class AdminController : Controller
    {
        //Khai báo database
        HRManagerEntities5 db = new HRManagerEntities5();

        // GET: Admin
        public ActionResult Admin_HomePage()
        {
            return View();
        }

        //Danh sách tài khoản
        public ActionResult ListAccount()
        {
            List<TaiKhoan> dsTaiKhoan = db.TaiKhoans.ToList();
            return View(dsTaiKhoan);
        }

        //Thêm tài khoản mới
        public ActionResult AddNewAccount()
        {
            List<NhanVien> dsNV = db.NhanViens.Where(st => st.TinhTrangTaiKhoan == false).ToList();
            List<SelectListItem> myNV = new List<SelectListItem>();
            foreach (NhanVien v in dsNV)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = v.MaNV,
                    Value = v.MaNV
                };

                myNV.Add(item);
            }

            ViewBag.dsNhanVien = myNV;

            return View();
        }
        
        [HttpPost]
        public ActionResult AddNewAccount(TaiKhoan model)
        {
            //Add new record
            Random rd = new Random();
            var matk = "NVPO_" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            model.MaTK = matk;
            var findNV = db.NhanViens.Find(model.MaNV);

            var tinhtrangTK = true;
            findNV.TinhTrangTaiKhoan = tinhtrangTK;

            db.TaiKhoans.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListAccount");
                       
        }

        //Check không trùng username
        public JsonResult IsUserNameExist(string Username)
        {
            return Json(!db.TaiKhoans.Any(us => us.Username == Username), JsonRequestBehavior.AllowGet);
        }

        //Xoá tài khoản ()
        public ActionResult DeleteAccount(string id)
        {
            // Find objects by ID
            TaiKhoan model = db.TaiKhoans.Find(id);
            model.NhanVien.TinhTrangTaiKhoan = false;

            //var updateModel = db.MayTinh.Find(id);
            db.TaiKhoans.Remove(model);

            // Save Change
            db.SaveChanges();

            return RedirectToAction("ListAccount");
        }

    }
}