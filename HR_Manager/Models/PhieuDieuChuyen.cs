//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HR_Manager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PhieuDieuChuyen
    {
        public string SoPhieuDC { get; set; }
        public string MaNV { get; set; }
        public string MaPB { get; set; }
        public Nullable<System.DateTime> NgayLapPDC { get; set; }
        public string PBDi { get; set; }
        public string NoiDung { get; set; }
    
        public virtual NhanVien NhanVien { get; set; }
        public virtual PhongBan PhongBan { get; set; }
    }
}
