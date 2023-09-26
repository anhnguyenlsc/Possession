using HR_Manager.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HR_Manager.Controllers
{
    public class HRController : Controller
    {
        //Khai báo database
        HRManagerEntities5 db = new HRManagerEntities5();
        // GET: HR
        public ActionResult HR_HomePage()
        {
            return View();
        }

        #region Quản Lý Nhân Viên
        // Xem danh sách nhân viên
        public ActionResult ListEmployee()
        {
            
            List<NhanVien> dsNhanVien = db.NhanViens.ToList();

            return View(dsNhanVien);
        }

        [HttpPost]
        public ActionResult ListEmployee(string search_employeelist)
        {
            if (search_employeelist != null)
            {
                List<NhanVien> nv = db.NhanViens.Where(x => x.HoTen.Contains(search_employeelist)).ToList();
                return View(nv);
            }

            List<NhanVien> dsNhanVien = db.NhanViens.ToList();

            return View(dsNhanVien);

            
        }

        // Thêm nhân viên
        public ActionResult AddNewEmployee()
        {
            var listCV = db.ChucVus.ToList();
            ViewBag.MaCV = new SelectList(listCV, "MaCV", "TenCV");

            var listPB = db.PhongBans.ToList();
            ViewBag.MaPB = new SelectList(listPB, "MaPB", "TenPB");
            return View();
        }

        [HttpPost] //Check tên file nếu trùng thì ko cho upload, khi upload đổi tên file thành mã nhân viên (SẼ LÀM)
        public ActionResult AddNewEmployee(NhanVien model, HttpPostedFileBase ImageUpload)
        {
            if (ImageUpload != null)
            {
                string fileName = Path.GetFileName(ImageUpload.FileName);
                string path = Path.Combine(Server.MapPath("/Images/"), fileName);

                model.AnhDaiDienNV = "/Images/" + fileName;
                ImageUpload.SaveAs(path);
               
            }

            //Dropdown List
            var listCV = db.ChucVus.ToList();
            ViewBag.MaCV = new SelectList(listCV, "MaCV", "TenCV");

            var listPB = db.PhongBans.ToList();
            ViewBag.MaPB = new SelectList(listPB, "MaPB", "TenPB");

            //Add new record
            Random rd = new Random();
            var manv = "NV" + rd.Next(0,9) + rd.Next(0,9) + rd.Next(0,9);

            model.MaNV = manv;
            model.SoNgayNPConLai = 8;

            db.NhanViens.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListEmployee");
        }

        #region Check unique
        //Check không trùng CCCD
        public JsonResult IsIdentifyCitzenExist(string cccd)
        {
            return Json(!db.NhanViens.Any(ci => ci.CCCD == cccd), JsonRequestBehavior.AllowGet);
        }

        //Check không trùng Email
        public JsonResult IsEmailExist(string email)
        {
            return Json(!db.NhanViens.Any(emNV => emNV.Email == email), JsonRequestBehavior.AllowGet);
        }

        #endregion

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
            ModelState.Remove("HoTen");
            ModelState.Remove("MaCV");
            ModelState.Remove("MaPB");
            ModelState.Remove("CCCD");
            ModelState.Remove("NgaySinh");
            ModelState.Remove("DanToc");
            ModelState.Remove("TonGiao");
            ModelState.Remove("QueQuan");
            ModelState.Remove("NoiOHienTai");
            ModelState.Remove("DienThoai");
            ModelState.Remove("TrinhDoHocVan");
            ModelState.Remove("Email");

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

        //Thông tin chi tiết nhân viên
        public ActionResult DetailsEmployee(string id)
        {
            //Lấy nhân viên với mã nhân viên tương ứng
            var nv = db.NhanViens.FirstOrDefault(s => s.MaNV == id);
            return View(nv);
        }

        //Xoá nhân viên 
        public ActionResult DeleteEmployee(string id)
        {
            NhanVien model = db.NhanViens.Find(id);
            db.NhanViens.Remove(model);

            //Save Change
            db.SaveChanges();
            return RedirectToAction("ListEmployee");
        }
        #endregion

        #region Quản Lý Phòng Ban

        //Xem danh sách nhân viên với phòng ban tương ứng (filter data)
        public ActionResult ListDepartment_Employee(string maPB)
        {
            //Dropdown list PhongBan
            var listPB = db.PhongBans.ToList();
            ViewBag.MaPB = new SelectList(listPB, "MaPB", "TenPB");

            //List ChucVu
            var listCV = db.ChucVus.ToList();
            ViewBag.MaCV = new SelectList(listCV, "MaCV", "TenCV");

            //Lấy danh sách nhân viên trong phòng ban theo MaPB
            List<NhanVien> dsNVPB = new List<NhanVien>();
            if (maPB == null)
            {
                dsNVPB = db.NhanViens.ToList();
            }
            else
            {
                dsNVPB = db.NhanViens.Where(pb => pb.MaPB == maPB).ToList();
            }
            return View(dsNVPB);      
            
        }

        //Thêm phiếu điều chuyển công tác
        public ActionResult AddTransferingDepartment(string id)
        {
            //Lấy MaNV
            ViewBag.MaNVDC = id;

            //Lấy Hình NV
            ViewBag.hinhNV = db.NhanViens.FirstOrDefault(nv => nv.MaNV == id).AnhDaiDienNV;

            //Lấy Phòng Ban Cũ
            ViewBag.phongbancu = db.NhanViens.FirstOrDefault(nv => nv.MaNV == id).PhongBan.TenPB;

            //Dropdown list PhongBan Mới
            var listPB = db.PhongBans.ToList();
            ViewBag.MaPB = new SelectList(listPB, "MaPB", "TenPB");


            return View();
        }

        [HttpPost]
        public ActionResult AddTransferingDepartment(PhieuDieuChuyen model, string id)
        {
            //Add new record
            Random rd = new Random();
            var sophieudc = "DC" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            model.SoPhieuDC = sophieudc;
            model.MaNV = id;

            model.NgayLapPDC = DateTime.Now; 
            //PhongBan hiện tại 
            ViewBag.phongbancu = db.NhanViens.FirstOrDefault(nv => nv.MaNV == id).MaPB;
            model.PBDi = ViewBag.phongbancu; 

            //Dropdown list lưu PhongBan Đến
            ViewBag.MaPB = new SelectList(db.PhongBans, "MaPB", "TenPB", model.MaPB);

            //Stored Procedute cập nhật phòng ban bên bảng Nhân Viên
            var updatePB = db.prUpdateChuyenPB(model.MaPB, id).ToString();

            db.PhieuDieuChuyens.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListDepartment_Employee");
        }

        #endregion

        #region Quản Lý Hợp Đồng 
        //Thêm hợp đồng cho nhân viên
        public ActionResult AddEmployeeAgreement(string idmaNV)
        {
            var listHD = db.LoaiHopDongs.ToList();
            ViewBag.MaLoaiHD = new SelectList(listHD, "MaLoaiHD", "TenLoaiHD");
            ViewBag.MaNV = idmaNV;
            //var nv = db.HopDongLaoDongs.FirstOrDefault(s => s.MaNV == id);

            //Lấy hình NV
            ViewBag.hinhNV = db.NhanViens.FirstOrDefault(nvs => nvs.MaNV == idmaNV).AnhDaiDienNV;

            
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployeeAgreement(HopDongLaoDong model, string idmaNV)
        {
            //Add new record
            Random rd = new Random();
            var mahd = "HD" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            model.MaHD = mahd;
            model.MaNV = idmaNV;

            db.HopDongLaoDongs.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("AgreementList");
        }

        //Danh sách hợp đồng
        public ActionResult AgreementList(string MaLoaiHD)
        {
            //Dropdown list LoaiHopDong
            var listLHD = db.LoaiHopDongs.ToList();
            ViewBag.MaLoaiHD = new SelectList(listLHD, "MaLoaiHD", "TenLoaiHD");

            //Filter data LoaiHopDong
            List<HopDongLaoDong> dsHopDong = new List<HopDongLaoDong>();
            if (MaLoaiHD == null)
            {
                 dsHopDong = db.HopDongLaoDongs.ToList();
            }
            else
            {
                 dsHopDong = db.HopDongLaoDongs.Where(st => st.MaLoaiHD == MaLoaiHD).ToList();
            }

            return View(dsHopDong);
        }

        //Cập nhật loại hợp đồng cho nhân viên
        public ActionResult UpdateEmployeeAgreement(string id, string idMaNV)
        {
            HopDongLaoDong hd_model = db.HopDongLaoDongs.Find(id);
            //Lấy Hình NV
            ViewBag.hinhNV = db.NhanViens.FirstOrDefault(nv => nv.MaNV == idMaNV).AnhDaiDienNV;

            ViewBag.MaLoaiHD = new SelectList(db.LoaiHopDongs, "MaLoaiHD", "TenLoaiHD", hd_model.MaLoaiHD);


            return View(hd_model);

        }

        [HttpPost]
        public ActionResult UpdateEmployeeAgreement(HopDongLaoDong hd_model)
        {
            var updateModel = db.HopDongLaoDongs.Find(hd_model.MaHD);

            ViewBag.MaLoaiHD = new SelectList(db.LoaiHopDongs, "MaLoaiHD", "TenLoaiHD", hd_model.MaLoaiHD);

            updateModel.NgayBDHD = hd_model.NgayBDHD;
            updateModel.NgayKTHD = hd_model.NgayKTHD;

            db.SaveChanges();
            return RedirectToAction("AgreementList");

        }


        #endregion

        #region Quản Lý Công Tác
        //Thêm phiếu công tác
        public ActionResult AddEmployeeBusiness(string id)
        {
            //Lấy hình nhân viên
            ViewBag.hinhNV = db.NhanViens.FirstOrDefault(nv => nv.MaNV == id).AnhDaiDienNV;

            //Dropdown List MaNV
            var listMaNV = db.NhanViens.ToList();
            ViewBag.MaNV = new SelectList(listMaNV, "MaNV", "MaNV");


            return View();
        }

        [HttpPost]
        public ActionResult AddEmployeeBusiness(PhieuCongTac model)
        {
            //Add new record
            Random rd = new Random();
            var sophieuct = "CT" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            model.SoPhieuCT = sophieuct;
            model.TinhTrangCongTac = "Đang công tác";

            db.PhieuCongTacs.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListEmployeeBusiness");
        }

        //Danh sách nhân viên công tác
        public ActionResult ListEmployeeBusiness()
        {
            List<PhieuCongTac> dsPhieuCongTac = db.PhieuCongTacs.ToList();

            return View(dsPhieuCongTac);
        }

        //Cập nhật quá trình công tác
        public ActionResult UpdateEmployeeBusiness(string id, string idMaNV)
        {
            PhieuCongTac model = db.PhieuCongTacs.Find(id);
            //Lấy Hình NV
            ViewBag.hinhNV = db.NhanViens.FirstOrDefault(nv => nv.MaNV == idMaNV).AnhDaiDienNV;

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateEmployeeBusiness(PhieuCongTac model)
        {
            // Find objects
            var updateModel = db.PhieuCongTacs.Find(model.SoPhieuCT);

            // Assign value for objects
            updateModel.NoiCT = model.NoiCT;
            updateModel.NoiDungCongTac = model.NoiDungCongTac;
            updateModel.TuNgay = model.TuNgay;
            updateModel.DenNgay = model.DenNgay;

            //Save Changes
            db.SaveChanges();

            return RedirectToAction("ListEmployeeBusiness");
        }

        //Xoá phiếu công tác
        public ActionResult DeleteEmployeeBusiness(string id)
        {
            // Find objects by ID
            PhieuCongTac model = db.PhieuCongTacs.Find(id);
            db.PhieuCongTacs.Remove(model);

            // Save Change
            db.SaveChanges();

            return RedirectToAction("ListEmployeeBusiness");
        }
        #endregion

        #region Quản Lý Nghỉ Phép
        public ActionResult ListApplicationForLeave()
        {
            List<PhieuNghiPhep> dsPNP = db.PhieuNghiPheps.ToList();
            
            return View(dsPNP);
        }

        public ActionResult DetailsApplicationForLeave(string id, string idNV)
        {
            var sopnp = db.PhieuNghiPheps.FirstOrDefault(s => s.SoPhieuNP == id);
            var findEmail = db.NhanViens.Find(idNV);

            ViewBag.emailNV = findEmail.Email;

            // Ẩn/hiện button phê duyệt
            var findMaNV = db.PhieuNghiPheps.FirstOrDefault(manv => manv.MaNV == idNV);
            ViewBag.showButton = false;
            if (findMaNV.TrangThai == "Chưa phê duyệt")
            {
                ViewBag.showButton = true;
            }    
            else if (findMaNV.TrangThai == "Đã phê duyệt")
            {
                ViewBag.showButton = false;
            }    

            return View(sopnp);
        }

        [HttpPost]
        public ActionResult SendEmailApproveApplication(string catchEmail, string SoPhieuNP)
        {
            var findNV = db.PhieuNghiPheps.FirstOrDefault(t => t.SoPhieuNP == SoPhieuNP);
            var tenNV = findNV.NhanVien.HoTen;
            string subject = "[NOTIFICATION APPLICATION FOR LEAVE]";
            var body = String.Format("<i>Dear </i>" + tenNV + "," + "<br />Your application for leave has been approved by the manager. Please be punctual to your schedule in order to maintain the work performance." +
                "<br /> Hope to see you soon. <br /> <br />") + @"<img src=""https://i.imgur.com/huzVZ0u.png"" />";

            WebMail.Send(catchEmail, subject, body);
            var findEmail = db.PhieuNghiPheps.Find(SoPhieuNP);

            var trangthaiNP = "Đã phê duyệt";
            findEmail.TrangThai = trangthaiNP;

            db.SaveChanges();

            return RedirectToAction("ListApplicationForLeave");
        }

        
        #endregion

        #region Quản Lý KLKT

        //Thêm phiếu KLKT
        public ActionResult AddRewardDiscipline()
        {
            //Dropdown List MaNV
            var listMaNV = db.NhanViens.ToList();
            ViewBag.MaNV = new SelectList(listMaNV, "MaNV", "HoTen");

            //Dropdown List MaPL
            var listMaPL = db.PhieuLuongs.ToList();
            ViewBag.MaPL = new SelectList(listMaPL, "MaPL", "MaPL");

            //Dropdown List LoaiKLKT
            var listKLKT = db.LoaiKLKTs.ToList();
            ViewBag.MaKLKT = new SelectList(listKLKT, "MaKLKT", "TenKLKT");

            return View();
        }

        [HttpPost]
        public ActionResult AddRewardDiscipline(PhieuKLKT model)
        {
            //Dropdown List MaNV
            var listMaNV = db.NhanViens.ToList();
            ViewBag.MaNV = new SelectList(listMaNV, "MaNV", "HoTen");

            //Dropdown List MaPL
            var listMaPL = db.PhieuLuongs.ToList();
            ViewBag.MaPL = new SelectList(listMaPL, "MaPL", "MaPL");

            //Dropdown List LoaiKLKT
            var listKLKT = db.LoaiKLKTs.ToList();
            ViewBag.MaKLKT = new SelectList(listKLKT, "MaKLKT", "TenKLKT");

            //Add new record
            Random rd = new Random();
            var sophieuKLKT = "RD" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            model.SoPhieuKLKT = sophieuKLKT;

            db.PhieuKLKTs.Add(model);

            //Save changes in record
            db.SaveChanges();
            return RedirectToAction("ListRewardDiscipline");
        }

        //Danh sách phiếu KLKT
        public ActionResult ListRewardDiscipline()
        {
            List<PhieuKLKT> dsPhieuKLKT = db.PhieuKLKTs.ToList();

            var kt = db.LoaiKLKTs.Where(lkt => lkt.MaKLKT.StartsWith("KT")).ToList();
            ViewBag.LKT = kt.ToString();

            var kl = db.LoaiKLKTs.Where(lkl => lkl.MaKLKT.StartsWith("KL")).ToList();
            ViewBag.LKL = kl.ToString();

            return View(dsPhieuKLKT);
        }

        //Cập nhật phiếu KLKT
        public ActionResult UpdateRewardsDisciplines(string id)
        {
            PhieuKLKT model = db.PhieuKLKTs.Find(id);

            //Dropdown List MaPL
            ViewBag.MaPL = new SelectList(db.PhieuLuongs, "MaPL", "MaPL", model.MaPL);

            //Dropdown List LoaiKLKT
            ViewBag.MaKLKT = new SelectList(db.LoaiKLKTs, "MaKLKT", "TenKLKT", model.MaKLKT);

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateRewardsDisciplines(PhieuKLKT model)
        {
            // Find objects
            var updateModel = db.PhieuKLKTs.Find(model.SoPhieuKLKT);

            //Dropdown List MaPL
            var listMaPL = db.PhieuLuongs.ToList();
            ViewBag.MaPL = new SelectList(listMaPL, "MaPL", "MaPL");

            //Dropdown List LoaiKLKT
            var listKLKT = db.LoaiKLKTs.ToList();
            ViewBag.MaKLKT = new SelectList(listKLKT, "MaKLKT", "TenKLKT");

            // Assign value for objects
            updateModel.MaPL = model.MaPL;
            updateModel.MaKLKT = model.MaKLKT;
            updateModel.ThoiGian = model.ThoiGian;

            //Save Changes
            db.SaveChanges();

            return RedirectToAction("ListRewardDiscipline");
        }

        #endregion

        #region Quản lý Chấm Công
        //Danh sách Chấm công
        public ActionResult ListTimekeepingEmployee(string MaNV, int? month)
        {
            //Dropdown list NhanVien
            var listNV = db.NhanViens.ToList();
            ViewBag.MaNV = new SelectList(listNV, "MaNV", "MaNV"); ;

            var months = Enumerable.Range(1, 12).Select(x => new SelectListItem
            {
                Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x),
                Value = x.ToString()
            }).ToList();

            months.Insert(0, new SelectListItem
            {
                Text = "-- Select --",
                Value = db.ChamCongs.ToList().ToString()
            });

            ViewBag.month = months;


            List<ChamCong> dsChamCong = new List<ChamCong>();
            if (MaNV == null)
            {
                dsChamCong = db.ChamCongs.ToList();
            }
            else
            {
                dsChamCong = db.ChamCongs.Where(cc => cc.MaNV == MaNV && cc.ThoiGianVaoLam.Value.Month == month).ToList();
            }


            return View(dsChamCong);
        }



        #endregion

    }
}
    