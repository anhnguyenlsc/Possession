using HR_Manager.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Objects;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HR_Manager.Controllers
{
    public class StaffController : Controller
    {
        //Khai báo database
        HRManagerEntities5 db = new HRManagerEntities5();

        // GET: Staff
        public ActionResult ST_Homepage()
        {
            //Hiển thị dropdown list danh sách loại nghỉ phép
            var listLNP = db.LoaiNghiPheps.ToList();
            ViewBag.MaLoaiNP = new SelectList(listLNP, "MaLoaiNP", "TenLoaiNP");

            return View();
        }


        [HttpPost]
        public ActionResult ST_Homepage(PhieuNghiPhep pnp_model, HttpPostedFileBase ImageUploadPNP, string MaNV, DateTime? ngaybdnghi, DateTime? ngaydilam, string noidungnp)
        {
            #region Tạo phiếu nghỉ phép
            if (ImageUploadPNP != null)
            {
                string fileName = Path.GetFileName(ImageUploadPNP.FileName);
                string path = Path.Combine(Server.MapPath("/Images/ST_ApplicationForLeave/"), fileName);

                pnp_model.AnhNghiPhepNV = "/Images/ST_ApplicationForLeave/" + fileName;
                ImageUploadPNP.SaveAs(path);
            }

            //Add new record
            Random rd = new Random();
            var sophieunp = "PN" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            //pnp_model.SoPhieuNP = sophieunp;
            pnp_model.NgayLapPNP = DateTime.Now;

            var trangthai = "Chưa phê duyệt";
            pnp_model.TrangThai = trangthai;
            //pnp_model.MaNV = pnp_model.NhanVien.MaNV;

            ViewBag.MaLoaiNP = new SelectList(db.LoaiNghiPheps, "MaLoaiNP", "TenLoaiNP", pnp_model.MaLoaiNP);

            //Stored Procedute cập nhật phòng ban bên bảng Nhân Viên
            ObjectParameter ngayNghiConLai = new ObjectParameter("ngayNghiConLai", typeof(int));
            ngayNghiConLai.Value = 0;
            
            var addNN = db.prThemNgayNghi(sophieunp, MaNV, pnp_model.MaLoaiNP, ngaybdnghi, ngaydilam, pnp_model.AnhNghiPhepNV, noidungnp, ngayNghiConLai).ToString();

            if (ngayNghiConLai.Value.Equals(0))
            {
                ViewBag.ImageNoti = "/Images/System/success.gif";
                TempData["NotiAdd"] = "Application registered successfully. Please wait for approval notification through email.";
            }
            else
            {
                ViewBag.ImageNoti = "/Images/System/remove.png";
                TempData["NotiAdd"] = "Application registered unsuccessfully. Your day - off remains : " + ngayNghiConLai.Value;
            }

            //db.PhieuNghiPheps.Add(pnp_model);

            //db.SaveChanges();
            #endregion

            return View();
        }

        [HttpPost]
        public ActionResult ST_TimekeepingCheckIn(ChamCong cc_model)
        {
            //Hiển thị DropdownList danh sách loại nghỉ phép
            var listLNP = db.LoaiNghiPheps.ToList();
            ViewBag.MaLoaiNP = new SelectList(listLNP, "MaLoaiNP", "TenLoaiNP");

            //Check in
            //Add new record
            Random rdcc = new Random();
            var machamcong = "CC" + rdcc.Next(111, 999);
            cc_model.MaCC = machamcong;
            ViewBag.macc = machamcong;

            cc_model.ThoiGianVaoLam = DateTime.Now;

            db.ChamCongs.Add(cc_model);
            //Save changes in record
            db.SaveChanges();

            return View("ST_Homepage");
        }

        [HttpPost]
        public ActionResult ST_TimekeepingCheckOut(ChamCong cc_model, string MaNV)
        {
            //Hiển thị DropdownList danh sách loại nghỉ phép
            var listLNP = db.LoaiNghiPheps.ToList();
            ViewBag.MaLoaiNP = new SelectList(listLNP, "MaLoaiNP", "TenLoaiNP");

            //Check out
            //string maCC = (TempData["MaCC"]).ToString();
            //var nvtimecheckout = db.ChamCongs.Find(cc_model.MaNV);
            //nvtimecheckout.ThoiGianKetThucLV = DateTime.Now;

            DateTime now = DateTime.Now.Date;
            ChamCong chamCong = db.ChamCongs.FirstOrDefault(cc => cc.MaNV == MaNV && cc.ThoiGianVaoLam >= now && cc.ThoiGianKetThucLV == null);
            chamCong.ThoiGianKetThucLV = DateTime.Now;

            //Stored Procedute cập nhật phòng ban bên bảng Nhân Viên

            db.SaveChanges();
            var updateSoGioLamViec = db.prUpdateSoGioLamViec(chamCong.MaCC);

            //Xoá session user hiện tại
            Session.Clear();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Login");
        }

        public ActionResult TransferingApplicationV()
        {
            return View();
        }

        public ActionResult ExportPDFTransfering()
        {
            return new ViewAsPdf("TransferingApplicationV")
            {
                FileName = "TransferingApplication.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }

        public ActionResult BusinessApplicationV()
        {
            return View();
        }

        public ActionResult ExportPDFBusiness()
        {
            return new ViewAsPdf("BusinessApplication")
            {
                FileName = "BusinessApplication.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait
            };
        }



        //Cập nhật thông tin nhân viên
        public ActionResult EmployeePersonalInfo(string id)
        {
            NhanVien nv_model = db.NhanViens.Find(id);
            //Lấy Hình NV
            ViewBag.hinhNV = db.NhanViens.FirstOrDefault(nv => nv.MaNV == id).AnhDaiDienNV;

            ViewBag.MaCV = new SelectList(db.ChucVus, "MaCV", "TenCV", nv_model.MaCV);
            ViewBag.MaPB = new SelectList(db.PhongBans, "MaPB", "TenPB", nv_model.MaPB);

            return View(nv_model);
        }
        //Cập nhật thông tin nhân viên
        public ActionResult UpdateEmployee(string id)
        {
            NhanVien nv_model = db.NhanViens.Find(id);
            //Lấy Hình NV
            ViewBag.hinhNV = db.NhanViens.FirstOrDefault(nv => nv.MaNV == id).AnhDaiDienNV;

            ViewBag.MaCV = new SelectList(db.ChucVus, "MaCV", "TenCV", nv_model.MaCV);
            ViewBag.MaPB = new SelectList(db.PhongBans, "MaPB", "TenPB", nv_model.MaPB);

            return View(nv_model);
        }

        [HttpPost]
        public ActionResult UpdateEmployee(NhanVien nv_model, HttpPostedFileBase ImageUpload)
        {
            
            if (ModelState.IsValid)
            {
                var updateModel = db.NhanViens.Find(nv_model.MaNV);

                updateModel.MaCV = nv_model.MaCV;
                updateModel.MaPB = nv_model.MaPB;
                updateModel.CCCD = nv_model.CCCD;
                updateModel.HoTen = nv_model.HoTen;
                updateModel.NgaySinh = nv_model.NgaySinh;
                updateModel.DanToc = nv_model.DanToc;
                updateModel.TonGiao = nv_model.TonGiao;
                updateModel.QueQuan = nv_model.QueQuan;
                updateModel.NoiOHienTai = nv_model.NoiOHienTai;
                updateModel.DienThoai = nv_model.DienThoai;
                updateModel.Email = nv_model.Email;
                updateModel.TrinhDoHocVan = nv_model.TrinhDoHocVan;
                updateModel.SoNgayNPConLai = nv_model.SoNgayNPConLai;

                if (ImageUpload != null)
                {
                    string fileName = Path.GetFileName(ImageUpload.FileName);
                    string path = Path.Combine(Server.MapPath("/Images/"), fileName);
                    updateModel.AnhDaiDienNV = nv_model.AnhDaiDienNV;
                    ImageUpload.SaveAs(path);

                }

                ViewBag.MaCV = new SelectList(db.ChucVus, "MaCV", "TenCV", nv_model.MaCV);
                ViewBag.MaPB = new SelectList(db.PhongBans, "MaPB", "TenPB", nv_model.MaPB);

                db.SaveChanges();
                return RedirectToAction("ListEmployee");
            }

            return View();


        }
    }
}