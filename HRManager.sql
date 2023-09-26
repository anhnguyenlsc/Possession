Use HRManager

Create table NhanVien 
(
	MaNV varchar(10) not null,
	MaCV varchar(10),
	MaPB varchar(10),
	CCCD varchar(12),
	HoTen nvarchar(30),
	NgaySinh date,
	GioiTinh nvarchar(10),
	DanToc nvarchar(30),
	TonGiao nvarchar(30),
	QueQuan nvarchar(50),
	NoiOHienTai nvarchar(50),
	DienThoai varchar(10),
	TrinhDoHocVan nvarchar(20),
	SoNgayNPConLai int,
	AnhDaiDienNV varchar(max),
	Email varchar(100),
	TinhTrangTaiKhoan bit default (0) not null
)

Create table ChucVu
(
	MaCV varchar(10) not null,	
	TenCV nvarchar(20),
)

Create table PhongBan
(
	MaPB varchar(10) not null,
	TenPB nvarchar(20),
)

Create table ChamCong 
(
	MaCC varchar(10) not null,
	MaNV varchar(10),
	MaPL varchar(10),
	ThoiGianVaoLam datetime,
	ThoiGianKetThucLV datetime,
	SoGioLamViec time
) 

Create table PhieuLuong
(
	MaPL varchar(10) not null,
	MaNV varchar(10),   
	Thang date,
	TroCapXang int,
	TruSaiPham int,
	KhenThuong int,
	LuongLamThemGio int,
	ThucNhan int
)

Create table BacLuong
(
	MaBL varchar(10) not null,
	MaCV varchar(10),
	MucLuongBac_I int,
	MucLuongBac_II int,
	MucLuongBac_III int,
	MucLuongBac_IV int,
	MucLuongBac_V int,
	MucLuongBac_VI int,
	MucLuongBac_VII int,
)

Create table PhieuKLKT
(
	SoPhieuKLKT varchar(10) not null,
	MaNV varchar(10),
	MaPL varchar(10),
	MaKLKT varchar(10),
	ThoiGian datetime
)

Create table LoaiKLKT
(
	MaKLKT varchar(10) not null,
	TenKLKT nvarchar(50),
	SoTien int
)

Create table PhieuNghiPhep
(
	SoPhieuNP varchar(10) not null,
	MaNV varchar(10),
	MaLoaiNP varchar(10),
	NgayLapPNP date,
	NgayBDNghi date,
	NgayDiLam date,
	TrangThai nvarchar(20) default(N'Chưa phê duyệt') not null,
	AnhNghiPhepNV varchar(max),
	NoiDungNP nvarchar(max)
)

Create table LoaiNghiPhep
(
	MaLoaiNP varchar(10) not null,
	TenLoaiNP nvarchar(30)
)

Create table NgayNghiCoDinh
(
	MaNN varchar(10) not null,
	TenNN nvarchar(100),
	NgayNghi date
)

Create table PhieuCongTac 
(
	SoPhieuCT varchar(10) not null,
	MaNV varchar(10),
	NoiCT nvarchar(30),
	TuNgay date,
	DenNgay date,
	NoiDungCongTac nvarchar (100),
	TinhTrangCongTac nvarchar(30) default(N'Đang công tác') not null
)

Create table PhieuDieuChuyen
(
	SoPhieuDC varchar(10) not null,
	MaNV varchar(10),
	MaPB varchar(10),
	NgayLapPDC date,
	PBDi nvarchar(20),
	NgayChuyen date,
	NoiDung nvarchar(100)
)

Create table HopDongLaoDong
(
	MaHD varchar(10) not null,
	MaNV varchar(10),
	MaLoaiHD varchar(10),
	NgayBDHD date,
	NgayKTHD date
)

Create table LoaiHopDong
(	
	MaLoaiHD varchar(10) not null,
	TenLoaiHD nvarchar(30)
)

Create table TaiKhoan
(
	MaTK varchar(10) not null,
	MaNV varchar(10),
	Username varchar(10),
	Password varchar(10),
)

--- TẠO CÁC RÀNG BUỘC KHOÁ CHÍNH, KHOÁ NGOẠI ---
------- KHOÁ CHÍNH
ALTER TABLE NhanVien add constraint MaNV_PK primary key (MaNV);
ALTER TABLE PhongBan add constraint MaPB_PK primary key (MaPB);
ALTER TABLE ChucVu add constraint MaCV_PK primary key (MaCV);
ALTER TABLE ChamCong add constraint MaCC_PK primary key (MaCC);
ALTER TABLE PhieuLuong add constraint MaPL_PK primary key (MaPL);
ALTER TABLE BacLuong add constraint MaBL_PK primary key (MaBL);
ALTER TABLE PhieuKLKT add constraint SoPhieuKLKT_PK primary key (SoPhieuKLKT);
ALTER TABLE LoaiKLKT add constraint MaKLKT_PK primary key (MaKLKT);
ALTER TABLE PhieuNghiPhep add constraint SoPhieuNP_PK primary key (SoPhieuNP);
ALTER TABLE LoaiNghiPhep add constraint MaLoaiNP_PK primary key (MaLoaiNP);
ALTER TABLE NgayNghiCoDinh add constraint MaNN_PK primary key (MaNN);
ALTER TABLE PhieuCongTac add constraint SoPhieuCT_PK primary key (SoPhieuCT);
ALTER TABLE TaiKhoan add constraint MaTK_PK primary key (MaTK);
ALTER TABLE PhieuDieuChuyen add constraint SoPhieuDC_PK primary key (SoPhieuDC);
ALTER TABLE HopDongLaoDong add constraint MaHD_PK primary key (MaHD);
ALTER TABLE LoaiHopDong add constraint MaLoaiHD_PK primary key (MaLoaiHD);

------- KHOÁ NGOẠI
ALTER TABLE ChamCong add constraint MaNV_CC_FK foreign key (MaNV) references NhanVien (MaNV);
ALTER TABLE PhieuLuong add constraint MaNV_PL_FK foreign key (MaNV) references NhanVien (MaNV);
ALTER TABLE PhieuKLKT add constraint MaNV_PKLKT_FK foreign key (MaNV) references NhanVien (MaNV);
ALTER TABLE PhieuNghiPhep add constraint MaNV_PNP_FK foreign key (MaNV) references NhanVien (MaNV);
ALTER TABLE PhieuCongTac add constraint MaNV_PCT_FK foreign key (MaNV) references NhanVien (MaNV);
ALTER TABLE PhieuDieuChuyen add constraint MaNV_PDC_FK foreign key (MaNV) references NhanVien (MaNV);
ALTER TABLE PhieuDieuChuyen add constraint MaPB_PDC_FK foreign key (MaPB) references PhongBan (MaPB);
ALTER TABLE HopDongLaoDong add constraint MaNV_HDLD_FK foreign key (MaNV) references NhanVien (MaNV);
ALTER TABLE TaiKhoan add constraint MaNV_TK_FK foreign key (MaNV) references NhanVien (MaNV);

ALTER TABLE PhieuKLKT add constraint MaKLKT_FK foreign key (MaKLKT) references LoaiKLKT(MaKLKT);
ALTER TABLE PhieuNghiPhep add constraint MaLoaiNP_FK foreign key (MaLoaiNP) references LoaiNghiPhep(MaLoaiNP);
ALTER TABLE HopDongLaoDong add constraint MaLoaiHD_FK foreign key (MaLoaiHD) references LoaiHopDong(MaLoaiHD);

ALTER TABLE NhanVien add constraint MaPB_FK foreign key (MaPB) references PhongBan (MaPB);
ALTER TABLE NhanVien add constraint MaCV_FK foreign key (MaCV) references ChucVu (MaCV);

ALTER TABLE BacLuong add constraint MaCV_BL_FK foreign key (MaCV) references ChucVu (MaCV);


--- DỮ LIỆU CÁC BẢNG ---
-------------- [TABLE NhanVien] -----------------
Select * from NhanVien
-------------- [TABLE TaiKhoan] -----------------
Select * from TaiKhoan

-------------- [TABLE ChucVu] -----------------
Select * from ChucVu
insert into ChucVu(MaCV, TenCV)
	values('NV', N'Nhân Viên')
insert into ChucVu(MaCV, TenCV)
	values('QL_NhanSu', N'Quản Lý Nhân Sự')
insert into ChucVu(MaCV, TenCV)
	values('QL_Luong', N'Quản Lý Lương')
insert into ChucVu(MaCV, TenCV)
	values('TP', N'Trưởng phòng')

-------------- [TABLE PhongBan] -----------------
Select * from PhongBan
insert into PhongBan(MaPB, TenPB)
	values('HTKT', N'Hỗ trợ kỹ thuật')
insert into PhongBan(MaPB, TenPB)
	values('KHKD', N'Marketing')
insert into PhongBan(MaPB, TenPB)
	values('TCLD', N'Tổ chức - Lao động')
insert into PhongBan(MaPB, TenPB)
	values('HCTH', N'Hành chính')
insert into PhongBan(MaPB, TenPB)
	values('NHSU', N'Nhân sự')

-------------- [TABLE LoaiHopDong] -----------------
Select * from LoaiHopDong 
insert into LoaiHopDong(MaLoaiHD, TenLoaiHD)
	values('HDKTH', N'Hợp đồng không thời hạn')
insert into LoaiHopDong(MaLoaiHD, TenLoaiHD)
	values('HDCTH', N'Hợp đồng có thời hạn')

-------------- [TABLE HopDongLaoDong] -----------------
Select * from HopDongLaoDong

-------------- [TABLE PhieuCongTac] -----------------
Select * from PhieuCongTac

-------------- [TABLE PhieuDieuChuyen] -----------------
Select * from PhieuDieuChuyen

-------------- [TABLE LoaiKLKT] -----------------
Select * from LoaiKLKT 
insert into LoaiKLKT(MaKLKT, TenKLKT, SoTien)
	values('KLNQ', N'Kỷ luật nghỉ quá số ngày quy định', 200000)
insert into LoaiKLKT(MaKLKT, TenKLKT, SoTien)
	values('KLTD', N'Kỷ luật không hoàn thành công việc đúng hạn', 500000)
insert into LoaiKLKT(MaKLKT, TenKLKT, SoTien)
	values('KLTG', N'Kỷ luật không đúng giờ', 100000)
insert into LoaiKLKT(MaKLKT, TenKLKT, SoTien)
	values('KTCC', N'Khen thưởng chuyên cần', 400000)
insert into LoaiKLKT(MaKLKT, TenKLKT, SoTien)
	values('KTDS', N'Khen thưởng đạt doanh số', 500000)
insert into LoaiKLKT(MaKLKT, TenKLKT, SoTien)
	values('KTHT', N'Khen thưởng hoàn thành dự án', 1000000)

-------------- [TABLE PhieuKLKT] -----------------
Select * from PhieuKLKT

-------------- [TABLE LoaiNghiPhep] -----------------
Select * from LoaiNghiPhep
insert into LoaiNghiPhep(MaLoaiNP, TenLoaiNP)
	values('NP_NO', N'Nghỉ ốm')
insert into LoaiNghiPhep(MaLoaiNP, TenLoaiNP)
	values('NP_TN', N'Nghỉ do tai nạn')
insert into LoaiNghiPhep(MaLoaiNP, TenLoaiNP)
	values('NP_TS', N'Nghỉ thai sản')

-------------- [TABLE PhieuNghiPhep] -----------------
select * from PhieuNghiPhep

-------------- [TABLE NgayNghiCoDinh] -----------------
select * from NgayNghiCoDinh
insert into NgayNghiCoDinh(MaNN, TenNN, NgayNghi)
	values('NL_DL',N'Tết Dương lịch', '2023-01-01')
insert into NgayNghiCoDinh(MaNN, TenNN, NgayNghi)
	values('NL_CT',N'Ngày Chiến Thắng', '2023-04-30')
insert into NgayNghiCoDinh(MaNN, TenNN, NgayNghi)
	values('NL_LD',N'Ngày Quốc tế lao động', '2023-05-01')
insert into NgayNghiCoDinh(MaNN, TenNN, NgayNghi)
	values('NL_QK',N'Quốc khánh', '2023-09-02')
insert into NgayNghiCoDinh(MaNN, TenNN, NgayNghi)
	values('NL_GT',N'Giỗ Tổ Hùng Vương', '2023-09-24')

-------------- [TABLE ChamCong] -----------------
select * from ChamCong

-------------- [TABLE BacLuong] -----------------
select * from BacLuong
insert into BacLuong(MaBL,MaCV,MucLuongBac_I,MucLuongBac_II,MucLuongBac_III,MucLuongBac_IV,MucLuongBac_V,MucLuongBac_VI,MucLuongBac_VII)
	values('BL_QLNS','QL_NhanSu',8000000,8400000,8820000,9261000,9724500,10210253,10720765)
insert into BacLuong(MaBL,MaCV,MucLuongBac_I,MucLuongBac_II,MucLuongBac_III,MucLuongBac_IV,MucLuongBac_V,MucLuongBac_VI,MucLuongBac_VII)
	values('BL_QLLG','QL_Luong',7000000,7350000,7717500,8103375,8508544,8933971,9380669)
insert into BacLuong(MaBL,MaCV,MucLuongBac_I,MucLuongBac_II,MucLuongBac_III,MucLuongBac_IV,MucLuongBac_V,MucLuongBac_VI,MucLuongBac_VII)
	values('BL_TP','TP',6000000,6300000,6615000,6945750,7293038,7657689,8040574)
insert into BacLuong(MaBL,MaCV,MucLuongBac_I,MucLuongBac_II,MucLuongBac_III,MucLuongBac_IV,MucLuongBac_V,MucLuongBac_VI,MucLuongBac_VII)
	values('BL_NV','NV',5100000,5355000,5622750,5903888,6199082,6509036,6834488)

-------------- [Procedure Update Department Employee] -----------------
Go
create proc prUpdateChuyenPB @MaPBden varchar(10), @MaNV varchar(10)
as
	begin
		update NhanVien
		set MaPB = @MaPBden
		where MaNV = @MaNV
	end

-------------- [Procedure Update SoGioLamViec] -----------------
Go
Create proc prUpdateSoGioLamViec @MaCC varchar(10)
as 
Begin
	update ChamCong
	set SoGioLamViec = datediff(hour, ThoiGianVaoLam, ThoiGianKetThucLV)
	where MaCC = @MaCC
End

exec prUpdateSoGioLamViec 'CC212'
select * from ChamCong

-------------- [Procedure Update SoNGayNPConLai] -----------------
select * from PhieuNghiPhep
Go
Create or alter proc prUpdateSoNgayNPConLai @MaNV varchar(10)
as 
Begin
	declare @TrangThai nvarchar(20)
	select @TrangThai = TrangThai
	from PhieuNghiPhep
	where MaNV = @MaNV

	declare @NgayBDNghi date, @NgayDiLam date
	select @NgayBDNghi = NgayBDNghi, @NgayDiLam = NgayDiLam
	from PhieuNghiPhep
	where MaNV = @MaNV

	if (@TrangThai = N'Đã phê duyệt')
		update NhanVien
		set SoNgayNPConLai = SoNgayNPConLai - datediff(day, @NgayBDNghi, @NgayDiLam)
		where MaNV = @MaNV
End

exec prUpdateSoNgayNPConLai 'NV425'
select * from PhieuNghiPhep
select * from NhanVien

-------------- [Procedure ThemNgayNghi] -----------------
go
create or alter proc prThemNgayNghi (@SoPhieuNP varchar(10), @MaNV varchar(10), @MaLoaiNP varchar(10), 
	@NgayBDNghi date, @NgayDiLam date, @AnhNghiPhepNV varchar(max), @NoiDungNP nvarchar(max), @NgayNghiConLai int = null output )
as
begin
	declare @biendem int
	select @biendem = count(*) from PhieuNghiPhep where MaNV = @MaNV

	if @biendem > 0
		begin
			declare @sumNgayNghiPrevious int -- Tong so ngay nghi cua 1 nhan vien nhung lan truoc do
			set @sumNgayNghiPrevious = 0
			declare @sumNgayNghiPresent int
			set @sumNgayNghiPresent = 0

			declare @NgayBDNghiTEMP date -- Bien tam chua ngay bat dau nghi
			declare @NgayDiLamTEMP date -- Bien tam chua ngay di lam lai

			declare @NgayNghiTheoQD int -- So ngay nghi duoc cho phep cua nhan vien
			select @NgayNghiTheoQD = SoNgayNPConLai from NhanVien where MaNV = @MaNV
			
			if @NgayNghiTheoQD = 0
				begin
					set @NgayNghiConLai = @NgayNghiTheoQD - @sumNgayNghiPrevious
					return; -- Gan gia tri cho so ngay con lai ( Hien thi trong MVC ) va dung procedure
				end

			declare cursorLoop cursor scroll 
			for select NgayBDNghi, NgayDiLam from PhieuNghiPhep where MaNV = @MaNV and TrangThai = N'Đã phê duyệt'
			open cursorLoop
			fetch first from cursorLoop into @NgayBDNghiTEMP, @NgayDiLamTEMP
			while @@FETCH_STATUS = 0
				begin
					set @sumNgayNghiPrevious = @sumNgayNghiPrevious + datediff(day, @NgayBDNghiTEMP, @NgayDiLamTEMP)
					fetch next from cursorLoop into @NgayBDNghiTEMP, @NgayDiLamTEMP
				end
			close cursorLoop
			deallocate cursorLoop

			-- Tinh sum cua nhung lan truoc do va hien tai ( cai dang insert vao )
			set @sumNgayNghiPresent = @sumNgayNghiPrevious + datediff(day, @NgayBDNghi, @NgayDiLam)

			-- So ngay nghi co dinh
			if @sumNgayNghiPresent > @NgayNghiTheoQD
				begin
					set @NgayNghiConLai = @NgayNghiTheoQD - @sumNgayNghiPrevious
					return; -- Gan gia tri cho so ngay con lai ( Hien thi trong MVC ) va dung procedure
				end -- end if
			else
				begin
					insert into PhieuNghiPhep ( SoPhieuNP, MaNV, MaLoaiNP, NgayLapPNP, NgayBDNghi, NgayDiLam, TrangThai, AnhNghiPhepNV, NoiDungNP )
					values ( @SoPhieuNP , @MaNV, @MaLoaiNP, GETDATE(), @NgayBDNghi, @NgayDiLam, N'Chưa được duyệt', @AnhNghiPhepNV, @NoiDungNP )
				end
		end -- end if
	else
		begin
			insert into PhieuNghiPhep ( SoPhieuNP, MaNV, MaLoaiNP, NgayLapPNP, NgayBDNghi, NgayDiLam, TrangThai, AnhNghiPhepNV, NoiDungNP )
			values ( @SoPhieuNP , @MaNV, @MaLoaiNP, GETDATE(), @NgayBDNghi, @NgayDiLam, N'Chưa được duyệt', @AnhNghiPhepNV, @NoiDungNP )
		end -- end else
end --end procedure

-- Insert values
declare @SoNgayConLai int;
exec prThemNgayNghi 'PN823', 'NV425','NP_TN', '2023-04-21', '2023-04-23', null, 'Car crash', @SoNgayConLai output
if ( @SoNgayConLai is null )
	print 'Them phieu nghi phep thanh cong'
else
	print N'Gia tri la: ' + cast(@SoNgayConLai as varchar)

declare @SoNgayConLai int;
exec prThemNgayNghi 'PN899', 'NV425','NP_TN', '2023-05-06', '2023-05-08', null, 'Car crash', @SoNgayConLai output
if ( @SoNgayConLai is null )
	print 'Them phieu nghi phep thanh cong'
else
	print 'Them phieu nghi phep khong thanh cong. So ngay nghi con lai cua ban: ' + cast(@SoNgayConLai as varchar)

declare @SoNgayConLai int;
exec ThemNgayNghi 'PN901', 'NV928','NP_TN', '2023-05-01', '2023-05-06', null, 'Car crash', @SoNgayConLai output
if ( @SoNgayConLai is null )
	print 'Gia tri la null'
else
	print 'Gia tri la: ' + cast(@SoNgayConLai as varchar)

-- Testing
drop procedure ThemNgayNghi
select * from PhieuNghiPhep
delete from PhieuNghiPhep
delete from PhieuNghiPhep where SoPhieuNP = 'PN901'

update NhanVien
set SoNgayNPConLai = 8
where MaNV = 'NV928'