﻿--1. 2001215707, Nguyễn Từ Thành Đạt, Lớp 12DHTH03
--2. 2001210341, Võ Nhất Bảo, Lớp 12DHTH15
--3. 2001210210, Lê Ngọc Đình Trí, Lớp 12DHTH18
CREATE DATABASE QLSANBONG
GO

USE QLSANBONG
GO
-- Bảo
CREATE TABLE ACCOUNT
(
	UserName VARCHAR(100) NOT NULL,
	Password VARCHAR(15) NOT NULL,
	DisplayName NVARCHAR(100) NOT NULL,
	Role INT CHECK(Role >= 0) DEFAULT 0,
	CONSTRAINT PK_ACCOUNT PRIMARY KEY (UserName)
)
-- Đạt
CREATE TABLE LOAISAN
(
	MaLoai INT IDENTITY(1,1) NOT NULL,
	TenLoai NVARCHAR(100) NOT NULL,
	GiaThue decimal CHECK(GiaThue > 0) DEFAULT 100000,
	CONSTRAINT PK_LOAISAN PRIMARY KEY (MaLoai)
)
-- Đạt
CREATE TABLE SANBONG 
(
	MaSan INT IDENTITY(1,1) NOT NULL,
	TenSan NVARCHAR(100) NOT NULL,
	MaLoai INT NOT NULL,
	CONSTRAINT PK_SANBONG PRIMARY KEY (MaSan),
	CONSTRAINT FK_SANBONG_LOAISAN FOREIGN KEY (MaLoai) REFERENCES LOAISAN(MaLoai)
)
-- Bảo
CREATE TABLE KHACHHANG
(
	MaKH INT IDENTITY(1,1) NOT NULL,
	TenKH NVARCHAR(100) NOT NULL,
	DiaChi NVARCHAR(50) DEFAULT N'Chưa xác định',
	SDT VARCHAR(10) NOT NULL UNIQUE,
	CONSTRAINT PK_KHACHHANG PRIMARY KEY (MaKH)
)
-- Đạt
CREATE TABLE LICHDATSAN
(
	MaLich INT IDENTITY(1,1) NOT NULL,
	ThoiGianBD DATETIME DEFAULT GETDATE(),
	ThoiGianKT DATETIME NOT NULL,
	MaKH INT NOT NULL,
	MaSan INT NOT NULL,
	ThanhTien decimal CHECK(ThanhTien >= 0) DEFAULT 0,
	CONSTRAINT PK_LICHDATSAN PRIMARY KEY (MaLich),
	CONSTRAINT FK_LICHDATSAN_KHACHHANG FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH),
	CONSTRAINT FK_LICHDATSAN_SANBONG FOREIGN KEY (MaSan) REFERENCES SANBONG(MaSan)
)
-- Trí
CREATE TABLE HOADON 
(
	MaHD INT IDENTITY(1,1) NOT NULL,
	NgayTao DATETIME DEFAULT GETDATE(),
	MaSan INT NOT NULL,
	MaKH INT NOT NULL,
	TongPhut INT NOT NULL,
	DonGia decimal check(DonGia > 0) DEFAULT 100000,
	TongTien decimal CHECK(TongTien >= 0) DEFAULT 0,
	CONSTRAINT PK_HOADON PRIMARY KEY (MaHD),
	CONSTRAINT FK_HOADON_SANBONG FOREIGN KEY (MaSan) REFERENCES SANBONG(MaSan),
	CONSTRAINT FK_HOADON_KHACHHANG FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH)
)
-- Bảo
CREATE TABLE DICHVU
(
	MaDV INT IDENTITY(1,1) NOT NULL,
	TenDV NVARCHAR(30) NOT NULL,
	DonGia INT CHECK(DonGia > 0) DEFAULT 10000,
	CONSTRAINT PK_DICHVU PRIMARY KEY (MaDV)
)
-- Trí

CREATE TABLE CHITIETHD
(
	MaDV INT NOT NULL,
	MaHD INT NOT NULL,
	SoLuong INT CHECK(SoLuong > 0) DEFAULT 1,
	CONSTRAINT PK_CHITIETHD PRIMARY KEY (MaDV,MaHD),
	CONSTRAINT PK_CHITIETHD_HOADON FOREIGN KEY (MaHD) REFERENCES HOADON(MaHD),
	CONSTRAINT PK_CHITIETHD_DICHVU FOREIGN KEY (MaDV) REFERENCES DICHVU(MaDV)
)

-- Đạt
-- cập nhật lại thành tiền của lịch đặt sân khi thay đổi giá thuê của loại sân
GO
CREATE TRIGGER UpdateTotalLoaiSan
ON LOAISAN
AFTER UPDATE
AS
BEGIN
    UPDATE LICHDATSAN
    SET THANHTIEN = (i.GiaThue/60) * DATEDIFF(MINUTE, LICHDATSAN.THOIGIANBD, LICHDATSAN.THOIGIANKT)
    FROM LICHDATSAN, SANBONG, inserted i
    WHERE i.MaLoai = SANBONG.MaLoai
	AND LICHDATSAN.MASAN = SANBONG.MASAN
END
GO
-- Đạt
-- cập nhật thành tiền của lịch đặt sân khi đặt
GO
CREATE TRIGGER InsertTotalLichDatSan
ON LICHDATSAN
AFTER INSERT
AS
BEGIN
    UPDATE LICHDATSAN
    SET THANHTIEN = (GIATHUE/60) * DATEDIFF(MINUTE, i.THOIGIANBD, i.THOIGIANKT)
    FROM SANBONG, LOAISAN, inserted i
    WHERE i.MaSan = SANBONG.MASAN
	AND LOAISAN.MALOAI = SANBONG.MaLoai
	AND LICHDATSAN.MASAN = i.MaSan
END
GO

-- Đạt
GO
CREATE TRIGGER UpdateTotalLichDatSan
ON LICHDATSAN
AFTER UPDATE
AS
BEGIN
	UPDATE LICHDATSAN
    SET THANHTIEN = (GIATHUE/60) * DATEDIFF(MINUTE, i.THOIGIANBD, i.THOIGIANKT)
    FROM SANBONG, LOAISAN, inserted i
    WHERE i.MaSan = SANBONG.MASAN
	AND LOAISAN.MALOAI = SANBONG.MaLoai
	AND LICHDATSAN.MASAN = i.MaSan
END
GO
-- Đạt

CREATE TRIGGER DeleteTotalLichDatSan
ON LICHDATSAN
AFTER DELETE
AS
BEGIN
	DECLARE @MaSan INT,  @MaKH INT, @TongPhut INT, @GiaThue DECIMAL, @ThanhTien DECIMAL, @THOIGIANBD TIME,  @THOIGIANKT TIME
	SELECT @MaSan = MaSan,
       @MaKH = MaKH,
       @THOIGIANBD = THOIGIANBD,
       @THOIGIANKT = THOIGIANKT,
       @ThanhTien = ThanhTien
	FROM deleted;
	SET @GiaThue = (Select GiaThue
					FROM SANBONG, LOAISAN
					WHERE @MaSan = SANBONG.MASAN
					AND LOAISAN.MaLoai = SANBONG.MaLoai
					)
    SET @TongPhut = DATEDIFF(MINUTE, @THOIGIANBD, @THOIGIANKT)
	INSERT INTO HOADON (MaKH, MaSan, TongPhut, DonGia, TongTien)
	VALUES (@MaKH,@MaSan, @TongPhut, @GiaThue, @ThanhTien)
END
GO

-- Bảo
-- Cập nhật tổng tiền của hóa đơn khi thay đổi giá của dịch vụ
GO
CREATE TRIGGER UpdateTotalDichVu
ON DICHVU
AFTER UPDATE
AS
BEGIN
	DECLARE @DONGIA decimal, @DonGiaCu decimal
	SET @DONGIA = (SELECT DONGIA FROM inserted)
	SET @DonGiaCu = (SELECT DONGIA FROM deleted)

    UPDATE HOADON
    SET TONGTIEN = (TONGTIEN - @DonGiaCu * SOLUONG) + @DONGIA * SoLuong
    FROM CHITIETHD, HOADON, DICHVU
    WHERE (SELECT MADV FROM inserted) = CHITIETHD.MADV
	AND HOADON.MAHD = CHITIETHD.MAHD
END

GO
CREATE TRIGGER TR_KHACHHANG_CapNhatDiaChiSDT
ON KHACHHANG
AFTER INSERT
AS
BEGIN
    UPDATE KHACHHANG
    SET DiaChi = N'Chưa xác định'
    FROM KHACHHANG
    INNER JOIN inserted ON KHACHHANG.MaKH = inserted.MaKH
    WHERE inserted.DiaChi IS NULL OR inserted.DiaChi = '';

    UPDATE KHACHHANG
    SET SDT = REPLACE(REPLACE(REPLACE(inserted.SDT, ' ', ''), '-', ''), '.', '')
    FROM KHACHHANG
    INNER JOIN inserted ON KHACHHANG.MaKH = inserted.MaKH;
END;
-- Trí
-- Cập nhật tổng tiền của hóa đơn khi thay đổi số lượng trong chi tiết hóa đơn
GO
CREATE TRIGGER InsertTotalChiTietHD
ON CHITIETHD
AFTER INSERT
AS
BEGIN
	DECLARE @SOLUONG INT, @MAHD INT, @MADV INT
	SET @SOLUONG = (SELECT SOLUONG FROM inserted)
	
	UPDATE HOADON
	SET TONGTIEN = TONGTIEN + DICHVU.DONGIA * @SOLUONG
	FROM LICHDATSAN, DICHVU, HOADON
	WHERE (SELECT MAHD FROM inserted) = HOADON.MAHD
	AND DICHVU.MADV = (SELECT MADV FROM inserted)
END
GO

-- Trí
GO
CREATE TRIGGER UpdateTotalChiTietHD
ON CHITIETHD
FOR UPDATE
AS
BEGIN
	DECLARE @SOLUONG INT, @MAHD INT, @MADV INT
	SELECT @SOLUONG = i.SoLuong, @MAHD = i.MaHD, @MADV = i.MaDV
	FROM inserted i

	IF(@SOLUONG < (SELECT SOLUONG FROM deleted)) 
	BEGIN
			UPDATE HOADON
			SET TONGTIEN = TONGTIEN - (DICHVU.DONGIA * ((SELECT SOLUONG FROM deleted) - @SOLUONG))
			FROM DICHVU,HOADON
			WHERE HOADON.MAHD = @MAHD AND DICHVU.MADV = @MADV
	END
	ELSE
	BEGIN
			UPDATE HOADON
			SET TONGTIEN = (TONGTIEN - (DICHVU.DONGIA * (SELECT SOLUONG FROM deleted))) + DICHVU.DONGIA * @SOLUONG
			FROM DICHVU,HOADON
			WHERE HOADON.MAHD = @MAHD AND DICHVU.MADV = @MADV
	END
END
GO
--Bảo
INSERT INTO ACCOUNT
VALUES ('admin1', '123456', N'Đạt', 1),
('admin2', 'admin123', N'Tài', 1),
('admin3', 'admin123456', N'Tú', 1),
('nhanvien1', 'nhanvien123', N'Bảo', 0),
('nhanvien2', 'nhanvien123456', N'Trí', 0)
-- Đạt
INSERT INTO LOAISAN
VALUES (N'Sân 5 Người', 100000),
(N'Sân 7 Người', 300000),
(N'Sân 9 Người', 500000),
(N'Sân 11 Người', 1000000)

-- Đạt
INSERT INTO SANBONG
VALUES (N'Sân 5 - 1', 1),
(N'Sân 5 - 2', 1),
(N'Sân 5 - 3', 1),
(N'Sân 7 - 1', 2),
(N'Sân 7 - 2', 2),
(N'Sân 7 - 3', 2),
(N'Sân 9 - 1', 3),
(N'Sân 9 - 2', 3),
(N'Sân 9 - 3', 3),
(N'Sân 11 - 1', 4),
(N'Sân 11 - 2', 4),
(N'Sân 11 - 3', 4)
-- Bảo
INSERT INTO KHACHHANG
VALUES (N'Nguyễn Văn An', N'Quận 1 TP.HCM', '0399127841'),
(N'Trần Văn Khánh', N'Quận Bình Tân TP.HCM', '0392127321'),
(N'Bùi Văn Hùng', N'Quận Bình Thạnh TP.HCM', '0982127612'),
(N'Nguyễn Trường Giang', N'Quận 2 TP.HCM', '0219127513'),
(N'Đinh Văn Quế', N'Quận 3 TP.HCM', '0339127333'),
(N'Bùi Văn Bường', N'Quận 4 TP.HCM', '0333127666')
-- Đạt
INSERT INTO LICHDATSAN (THOIGIANBD, THOIGIANKT, MAKH, MASAN)
VALUES ('11-20-2023 16:00', '11-20-2023 18:00', 1, 1)

INSERT INTO LICHDATSAN (THOIGIANBD, THOIGIANKT, MAKH, MASAN)
VALUES('11-20-2023 15:30', '11-20-2023 18:00', 2, 4)

INSERT INTO LICHDATSAN (THOIGIANBD, THOIGIANKT, MAKH, MASAN)
VALUES('11-20-2023 15:00', '11-20-2023 17:00', 3, 8)

INSERT INTO LICHDATSAN (THOIGIANBD, THOIGIANKT, MAKH, MASAN)
VALUES('11-18-2023 14:00', '11-18-2023 17:00', 4, 1)

INSERT INTO LICHDATSAN (THOIGIANBD, THOIGIANKT, MAKH, MASAN)
VALUES('11-17-2023 13:00', '11-17-2023 18:00', 5, 2)

INSERT INTO LICHDATSAN (THOIGIANBD, THOIGIANKT, MAKH, MASAN)
VALUES('11-20-2023 15:00', '11-20-2023 17:00', 6, 11)
-- Trí
INSERT INTO HOADON
VALUES ('11-20-2023 15:00',1,1,120,100000, 200000)

INSERT INTO HOADON
VALUES('10-20-2023 18:00',2,2,180,100000, 300000)

INSERT INTO HOADON
VALUES('12-10-2023 21:00',3,3,120,100000, 200000)
-- Bảo
INSERT INTO DICHVU
VALUES (N'Nước uống Sting', 10000),
(N'Nước uống Olong', 10000),
(N'Mì xào', 30000),
(N'Cơm gà xối mỡ', 30000),
(N'Cơm tấm', 30000),
(N'Hủ tiếu', 30000)
-- Trí
INSERT INTO CHITIETHD
VALUES (1, 1, 5)

INSERT INTO CHITIETHD
VALUES(2, 2, 7)

INSERT INTO CHITIETHD
VALUES(3, 3, 11)

-- Đạt
-- Stored Procedures Login
GO
CREATE PROC SP_Login
@username nvarchar(100), @password nvarchar(15)
AS 
BEGIN
	SELECT * FROM ACCOUNT WHERE USERNAME = @username AND PASSWORD = @password
END
GO

-- Stored Procedures Quản lý sân
GO
CREATE PROC SP_GetListSan
AS
BEGIN
	SELECT * FROM SANBONG
END
GO

GO
CREATE PROC SP_KiemTraTrungTenSan
@TenSan nvarchar(100)
AS 
BEGIN
	SELECT * FROM SANBONG WHERE TenSan = @TenSan
END
GO

GO
CREATE PROC SP_ThemSan 
@TenSan nvarchar(100), @Maloai int
AS
BEGIN
	INSERT INTO SANBONG
	VALUES (@TenSan, @Maloai)
END
GO

GO
CREATE PROC SP_XoaSan 
@MaSan int
AS
BEGIN
	DELETE FROM SANBONG WHERE MASAN = @MaSan
END
GO

GO
CREATE PROC SP_SuaSan
@MaSan int, @TenSan nvarchar(100), @MaLoai int
AS
BEGIN
	UPDATE SANBONG
	SET TenSan = @TenSan, MaLoai = @MaLoai
	WHERE SANBONG.MaSan = @MaSan
END
GO

GO
CREATE PROC SP_TimKiemSan
@str nvarchar(100)
AS
BEGIN
	SELECT * FROM SANBONG WHERE TENSAN LIKE '%' + @str + '%' OR MaSan LIKE '%' + @str + '%' OR MaLoai LIKE '%' + @str + '%'
END
GO

-- Stored Procedures Quản lý loại sân
GO
CREATE PROC SP_GetListLoaiSan
AS
BEGIN
	SELECT * FROM LOAISAN
END
GO

GO
CREATE PROC SP_KiemTraTrungTenLoai
@TenLoai nvarchar(100)
AS 
BEGIN
	SELECT * FROM LOAISAN WHERE TenLoai = @TenLoai
END
GO

GO
CREATE PROC SP_ThemLoaiSan
@TenLoai nvarchar(100), @GiaThue float
AS
BEGIN
	INSERT INTO LOAISAN
	VALUES (@TenLoai, @GiaThue)
END
GO

GO
CREATE PROC SP_XoaLoaiSan 
@MaLoai int
AS
BEGIN
	DELETE FROM LOAISAN WHERE Maloai = @MaLoai
END
GO

GO
CREATE PROC SP_SuaLoaiSan
@MaLoai int, @TenLoai nvarchar(100), @GiaThue float
AS
BEGIN
	UPDATE LOAISAN
	SET TenLoai = @TenLoai, GiaThue = @GiaThue
	WHERE LOAISAN.MaLoai = @MaLoai
END
GO

GO
CREATE PROC SP_TimKiemLoaiSan
@str nvarchar(100)
AS
BEGIN
	SELECT * FROM LOAISAN WHERE TenLoai LIKE '%' + @str + '%' OR MaLoai LIKE '%' + @str + '%' OR GiaThue LIKE '%' + @str + '%'
END
GO

-- Stored Procedures Quản lý lịch đặt sân
GO
CREATE PROC SP_GetListLichDatSan
AS
BEGIN
	SELECT * FROM LICHDATSAN
END
GO

GO
CREATE PROC SP_ThemLichDatSan
@ThoiGianBD DateTime, @ThoiGianKT DateTime, @MaKH int, @MaSan int
AS
BEGIN
	INSERT INTO LICHDATSAN (THOIGIANBD, THOIGIANKT, MAKH, MASAN)
	VALUES (@ThoiGianBD, @ThoiGianKT, @MaKH, @MaSan)
END
GO

GO
CREATE PROC SP_XoaLichDatSan
@MaLich int
AS
BEGIN
	DELETE FROM LICHDATSAN WHERE MaLich = @MaLich
END
GO

GO
CREATE PROC SP_SuaLichDatSan
@MaLich int, @ThoiGianBD DateTime, @ThoiGianKT DateTime, @MaKH int, @MaSan int
AS
BEGIN
	UPDATE LICHDATSAN
	SET ThoiGianBD = @ThoiGianBD, ThoiGianKT = @ThoiGianKT, MaKH = @MaKH, MaSan = @MaSan
	WHERE MaLich = @MaLich
END
GO

-- Bảo
-- Stored Procedures SP_GetListLoaiSan
GO
CREATE PROC SP_GetListDichVu
AS
BEGIN
	SELECT * FROM DICHVU
END
GO

-- Stored Procedures SP_KiemTraTrungDichVu
GO
CREATE PROC SP_KiemTraTrungTenDichVu
@TenDV nvarchar(100)
AS 
BEGIN
	SELECT * FROM DICHVU WHERE TENDV = @TenDV
END
GO

-- Stored Procedures SP_ThemDichVu
GO
CREATE PROC SP_ThemDichVu
@TenDV nvarchar(100), @Gia float
AS
BEGIN
	INSERT INTO DICHVU
	VALUES (@TenDV, @Gia)
END
GO
-- Stored Procedures SP_XoaDV
GO
CREATE PROC SP_XoaDV
@TenDV nvarchar(200)
AS
BEGIN
	DELETE FROM DICHVU WHERE TenDV = @TenDV
END
go

CREATE PROC SP_SuaDichVu
@MaDV int, @TenDV nvarchar(100), @Gia int
AS
BEGIN
	UPDATE DICHVU
	SET TenDV = @TenDV, DonGia = @Gia
	WHERE DICHVU.MaDV = @MaDV
END

--Stored Procedures SP_TimDV
GO
CREATE PROC SP_TimDV
@str nvarchar(100)
AS
BEGIN
	SELECT * FROM DICHVU
	WHERE TenDV LIKE '%' + @str + '%' OR MaDV LIKE '%' + @str + '%' OR DonGia LIKE '%' + @str + '%';
END
GO

-- Trí
-- Stored Procedures SP_GetListHoaDon
GO
CREATE PROC SP_GetListHoaDon
AS
BEGIN
	SELECT * FROM HOADON
END
GO
 
 GO
CREATE PROC SP_SuaHoaDon
    @MaHD INT,
    @NgayTao DATETIME,
    @TongTien DECIMAL(18, 0),
    @MaKH INT,
	@MaSan INT,
	@TongPhut INT
AS
BEGIN
    UPDATE HOADON
    SET NgayTao = @NgayTao,
        TongTien = @TongTien,
        MaKH = @MaKH,
		MaSan = @MaSan,
		TongPhut = @TongPhut
    WHERE MaHD = @MaHD;
END
GO

GO
CREATE PROC SP_XoaHoaDon 
@MaHD int
AS
BEGIN
	DELETE FROM HOADON WHERE MaHD = @MaHD
END


GO
CREATE PROC SP_CanEditMaHD
    @MaHD INT
AS
BEGIN
    DECLARE @CanEdit BIT;

    SELECT @CanEdit = CASE
        WHEN @MaHD > 0 THEN 1
        ELSE 0
    END;

    SELECT @CanEdit AS CanEdit;
END

GO
CREATE PROC SP_GetListCTHoaDon
AS
BEGIN
	SELECT * FROM CHITIETHD
END
GO
--themCTHD
GO
CREATE PROC SP_ThemChiTietHoaDon
    @MaHD INT,
    @MaDV INT,
    @SoLuong INT
AS
BEGIN
    INSERT INTO CHITIETHD(MaHD, MaDV, SoLuong)
    VALUES (@MaHD, @MaDV, @SoLuong);

END

-- Bảo
--Stored Procedures KHACHHANG
--Stored Procedures Tìm kiếm khách hàng
GO
CREATE PROC SP_TimKH
@STR nvarchar(100)
AS
BEGIN
	SELECT * FROM KHACHHANG
	WHERE TenKH LIKE '%' + @STR + '%' OR MaKH LIKE '%' + @STR + '%' OR DiaChi LIKE '%' + @STR + '%' OR SDT LIKE '%' + @STR + '%';
END
GO

--Stored Procedures GetlistKhachHang
GO
CREATE PROC SP_GetListKhachHang	
AS
BEGIN
	SELECT * FROM KHACHHANG
END
GO

--Stored Procedures ThemKhachHang
GO
CREATE PROC SP_ThemKhachHang
@TenKH nvarchar(100), @DiaChi nvarchar(100), @SDT nvarchar(20)
AS
BEGIN
	INSERT INTO KHACHHANG
	VALUES (@TenKH, @DiaChi,@SDT)
END
GO

--Stored Procedures	SuaDanhSachKH
GO
CREATE PROC SP_SuaDanhSachKH
@TenKH nvarchar(100), @DiaChi nvarchar(100), @SDT varchar(20), @MaKH int
AS
BEGIN
	UPDATE KHACHHANG
	SET TenKH = @TenKH, DiaCHi = @DiaChi, SDT = @SDT
	WHERE MaKH = @MaKH
END
GO

--PHÂN QUYỀN

SP_ADDROLE NHANVIEN
go
SP_ADDROLE NguoiQuanTri
go
SP_ADDLOGIN 'admin', '123'
go
SP_ADDUSER 'admin','admin'

EXEC SP_ADDROLEMEMBER 'db_owner','admin'

--ROLE NHÂN VIÊN
GRANT INSERT, SELECT, UPDATE
ON KHACHHANG
TO NHANVIEN

GRANT SELECT, INSERT, DELETE, UPDATE
ON CHITIETHD
TO NHANVIEN

GRANT SELECT, INSERT, DELETE, UPDATE
ON DICHVU
TO NHANVIEN

GRANT SELECT, INSERT, UPDATE
ON HOADON
TO NHANVIEN

GRANT SELECT, INSERT, UPDATE
ON KHACHHANG
TO NHANVIEN

--ROLE ADMIN
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.ACCOUNT TO NguoiQuanTri;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.LOAISAN TO NguoiQuanTri;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.SANBONG TO NguoiQuanTri;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.KHACHHANG TO NguoiQuanTri;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.LICHDATSAN TO NguoiQuanTri;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.HOADON TO NguoiQuanTri;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.DICHVU TO NguoiQuanTri;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.CHITIETHD TO NguoiQuanTri;



