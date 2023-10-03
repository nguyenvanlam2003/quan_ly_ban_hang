create database DoAn_C#

create table Quyen( 
MaQuyen nvarchar(50) primary key,
TenQuyen nvarchar(50) not null
)
insert into Quyen
values
('QL','Quan li'),
('NVBH','Nhan vien ban hang'),
('NVK','Nhan vien kho')
select * from Quyen
create table TaiKhoan(
MaNV nvarchar(50) primary key ,
Username nvarchar(50) not null, 
MatKhau nvarchar(50) not null, 
MaQuyen nvarchar(50) not null, 
foreign key (MaQuyen) references Quyen(MaQuyen),
foreign key (MaNV) references NhanVien(MaNV)
)
drop table TaiKhoan
insert into TaiKhoan
values
('NV01','a123','a123','QL'),
('NV02','b123','b123','NVBH'),
('NV03','c123','c123','NVK')

create table NhanVien(
MaNV nvarchar(50) primary key,
TenNV nvarchar(50) ,
GoiTinh nvarchar(50),
NgaySinh date,
DiaChi nvarchar(50) , 
Email nvarchar(50) ,
SDT nvarchar(50)  
)
insert into NhanVien
values
('NV01','Nguyen van a','nam','1-7-2003','ha noi', 'a@gmail.com','123456'),
('NV02','Nguyen van b','nu','3-4-2003','nam dinh', 'b@gmail.com','123456'),
('NV03','Nguyen van c','nam','1-3-2003','ha nam', 'c@gmail.com','123456')

create table NhaCungCap(
MaNCC nvarchar(50) primary key,
TenNCC nvarchar(50) ,
DiaChi nvarchar(50) , 
Email nvarchar(50) ,
SDT nvarchar(50) 
)
insert into NhaCungCap
values
('NCC01','vot cau long thuy hanh','ha noi','TH@gmail.com','12346'),
('NCC02','giay the thao minh trang','ha noi','MT@gmail.com','12346'),
('NCC03','dung cu the thao hong hai','ha noi','HH@gmail.com','12346')
create table PhieuNhap(
MaPN nvarchar(50) primary key,
MaNCC nvarchar(50),
MaNV nvarchar(50) ,
TongTien float
)

insert into PhieuNhap(MaPN,MaNCC,MaNV)
values
('PN01','NCC01','NV03'),
('PN02','NCC02','NV03'),
('PN03','NCC03','NV03')
create table SanPham(
MaSP nvarchar(50) primary key,
TenSP nvarchar(50),
XuatXu nvarchar(50) ,
MoTa nvarchar(50),
GiaBan float,
SoLuong int check(SoLuong>=0),
Anh nvarchar(500) 
)
insert into SanPham(MaSP,TenSP,GiaBan,SoLuong )
values
('SP01','a123',3.4,10),
('SP02','b123',7.4,20),
('SP03','c123',9.4,15)

select *from SanPham

create table ChiTietPN(
MaPN nvarchar(50),
MaSP nvarchar(50),
GiaNhap float DEFAULT 0 ,
SoLuong int check(SoLuong>=0)DEFAULT 0,
NgayNhap date,
ThanhTien float,
foreign key (MaPN) references PhieuNhap(MaPN),
foreign key (MaSP) references SanPham(MaSP)
)
DROP TABLE ChiTietPN
create table HoaDon(
MaHD nvarchar(50) primary key,
NgayBan date,
MaNV nvarchar(50) ,
TongTien float
)



delete from HoaDon
delete from chitietHD
create table ChiTietHD(
MaHD nvarchar(50) ,
MaSP nvarchar(50) ,
GiaBan  float ,
SoLuong int check(SoLuong>=0) DEFAULT 0,
GiamGia float DEFAULT 0,
ThanhTien float
foreign key (MaSP) references SanPham(MaSP),
foreign key (MaHD) references HoaDon(MaHD)
)
insert into ChiTietHD(MaHD,MaSP,GiaBan,SoLuong,GiamGia) values ('','','','','') 
GO

go
--1
create trigger them_Gia_ban_CTHD
on ChiTietHD
after insert,update
as
begin
SET NOCOUNT ON
update ChiTietHD
set 
GiaBan=SanPham.GiaBan,
ThanhTien=round(SanPham.GiaBan*ChiTietHD.SoLuong-SanPham.GiaBan*ChiTietHD.SoLuong*ChiTietHD.GiamGia/100,3)
from SanPham
inner join ChiTietHD  on ChiTietHD.MaSP=SanPham.MaSP
where ChiTietHD.MaSP in (select MaSP from inserted)
end
go
drop trigger them_Gia_ban_CTHD
go 
select * from ChiTietHD

go 


go
--2
CREATE TRIGGER tinh_tong_HD
ON ChiTietHD
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF (SELECT COUNT(*) FROM deleted) > 0
    BEGIN
        -- Xử lý khi có sự kiện DELETE
        UPDATE HoaDon
        SET TongTien = (
            SELECT round(SUM(ThanhTien),3)
            FROM ChiTietHD
            WHERE ChiTietHD.MaHD = HoaDon.MaHD
        )
        FROM HoaDon
        WHERE EXISTS (
            SELECT 1
            FROM deleted
            WHERE deleted.MaHD = HoaDon.MaHD
        );
    END
    ELSE
    BEGIN
        -- Xử lý khi có sự kiện INSERT hoặc UPDATE
        UPDATE HoaDon
        SET TongTien = (
            SELECT SUM(ThanhTien)
            FROM ChiTietHD
            WHERE ChiTietHD.MaHD = HoaDon.MaHD
        )
        FROM HoaDon
        WHERE EXISTS (
            SELECT 1
            FROM inserted
            WHERE inserted.MaHD = HoaDon.MaHD
        );
    END
END;


go

--3
CREATE TRIGGER them_sanPhamHD
ON ChiTietHD
AFTER INSERT
AS
BEGIN
  DECLARE  @slsphd INT, @soluongsp INT
  SELECT @slsphd =soluong
  from ChiTietHD  where MaSP in(select MaSP from inserted)
  select @soluongsp =soluong from SanPham
  where MaSP =(SELECT masp FROM inserted)

  if(@soluongsp>=@slsphd)
  begin
  update SanPham
  set SoLuong=@soluongsp-@slsphd
   where MaSP in(select MaSP from inserted)
   end
   else
   begin
      print('không đủ số lượng')
      ROLLBACK TRAN
   end
END;
drop trigger them_sanPhamHD

go

--4
CREATE TRIGGER delete_sanPham
ON ChiTietHD
AFTER delete
AS
BEGIN
  update SanPham
  set SoLuong=SanPham.SoLuong+deleted.SoLuong
  from deleted 
  inner join SanPham on deleted.MaSP=SanPham.MaSP
END;


select *from SanPham
DROP TRIGGER delete_sanPham
go

--5
CREATE TRIGGER sua_sanPhamHD_thaydoiMSP
ON ChiTietHD
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @SoLuongCu INT, @SoLuongMoi INT;
    DECLARE @SoLuongSanPhamcu INT, @SoLuongSanPham INT;
    SELECT @SoLuongSanPhamcu = SoLuong
    FROM SanPham
	where MaSP IN (SELECT masp FROM deleted)

    SELECT @SoLuongCu = SoLuong
    FROM deleted
    where MaSP IN(SELECT masp FROM deleted)
	update SanPham
	set SoLuong=@SoLuongSanPhamcu+@SoLuongCu
	WHERE MaSP IN (SELECT masp FROM deleted);

	SELECT @SoLuongMoi = SoLuong
    FROM inserted
	SELECT @SoLuongSanPham = SoLuong
    FROM SanPham
	where MaSP IN (SELECT masp FROM inserted)
    IF (@SoLuongSanPham >= @SoLuongMoi )
    BEGIN
        UPDATE SanPham
        SET SoLuong = @SoLuongSanPham - @SoLuongMoi 
        WHERE MaSP IN (SELECT masp FROM inserted);
    END
    ELSE
    BEGIN
        PRINT('Không đủ số lượng');
        ROLLBACK TRANSACTION;
    END;
END;

drop TRIGGER sua_sanPhamHD_thaydoiMSP

--6

CREATE TRIGGER TINH_ThanhTien
ON ChiTietPN
AFTER INSERT , UPDATE 
AS
BEGIN
SET NOCOUNT ON;
UPDATE ChiTietPN
SET ThanhTien= ROUND(GiaNhap*SoLuong,3)
END

DELETE FROM nhanvien WHERE MaNV='NV02'

DROP TRIGGER TINH_ThanhTien
delete from ChiTietHD

SELECT *FROM ChiTietPN
DELETE FROM ChiTietPN WHERE MaPN='PN01'AND MaSP='SP01'
--7
go
CREATE TRIGGER THEM_SPPN
ON ChiTietPN
AFTER INSERT
AS
BEGIN
	UPDATE SanPham
  set SoLuong=SanPham.SoLuong+inserted.SoLuong
  from inserted 
  inner join SanPham on inserted.MaSP=SanPham.MaSP  
END
--8
CREATE TRIGGER XOA_SPPN
ON ChiTietPN
AFTER DELETE
AS
BEGIN
SET NOCOUNT ON;
update SanPham
  set SoLuong=SanPham.SoLuong-deleted.SoLuong
  from deleted 
  inner join SanPham on deleted.MaSP=SanPham.MaSP
END

DROP TRIGGER THEM_SPPN
--9
GO
CREATE TRIGGER sua_spPN
ON ChiTietPN
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	DECLARE @SoLuongCu INT;
	SELECT @SoLuongCu =deleted.SoLuong
	FROM deleted
	INNER JOIN SanPham ON SanPham.MaSP=deleted.MaSP
   UPDATE SanPham
   SET SoLuong=SanPham.SoLuong+ inserted.SoLuong-deleted.SoLuong
   from inserted,deleted
  WHERE  SanPham.MaSP IN(SELECT MaSP FROM inserted) AND SanPham.MaSP IN(SELECT MaSP FROM deleted) 
END;
GO
SELECT  *FROM SanPham
DROP TRIGGER sua_spPN
select * from SanPham
--10
CREATE TRIGGER tinh_tong_PN
ON ChiTietPN
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF (SELECT COUNT(*) FROM deleted) > 0
    BEGIN
        -- Xử lý khi có sự kiện DELETE
        UPDATE PhieuNhap
        SET TongTien = (
            SELECT round(SUM(ThanhTien),3)
            FROM ChiTietPN
            WHERE ChiTietPN.MaPN = PhieuNhap.MaPN
        )
        FROM PhieuNhap
        WHERE EXISTS (
            SELECT 1
            FROM deleted
            WHERE deleted.MaPN =PhieuNhap.MaPN
        );
    END
    ELSE
    BEGIN
        -- Xử lý khi có sự kiện INSERT hoặc UPDATE
        UPDATE PhieuNhap
        SET TongTien = (
            SELECT SUM(ThanhTien)
            FROM ChiTietPN
            WHERE ChiTietPN.MaPN = PhieuNhap.MaPN
        )
        FROM PhieuNhap
        WHERE EXISTS (
            SELECT 1
            FROM inserted
            WHERE inserted.MaPN = PhieuNhap.MaPN
        );
    END
END;
