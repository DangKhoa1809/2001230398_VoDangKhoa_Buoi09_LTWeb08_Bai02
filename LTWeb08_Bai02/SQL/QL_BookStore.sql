--TẠO DATABASE QL_BookStore--
CREATE DATABASE QL_BookStore;

--SỬ DỤNG DATABASE--
USE QL_BookStore;

--TẠO BẢNG TACGIA--
CREATE TABLE TACGIA (
    MaTG VARCHAR(10) PRIMARY KEY,
    TenTG NVARCHAR(100),
    DiaChi NVARCHAR(200),
    TieuSu NVARCHAR(MAX),
    DienThoai NVARCHAR(20)
);

--TẠO BẢNG NHAXUATBAN--
CREATE TABLE NHAXUATBAN (
    MaNXB VARCHAR(10) PRIMARY KEY,
    TenNXB NVARCHAR(100),
    Diachi NVARCHAR(200),
    DienThoai NVARCHAR(20)
);

--TẠO BẢNG CHUDE--
CREATE TABLE CHUDE (
    MaCD VARCHAR(10) PRIMARY KEY,
    TenChuDe NVARCHAR(100)
);

--TẠO BẢNG SACH--
CREATE TABLE SACH (
    MaSach VARCHAR(10) PRIMARY KEY,
    TenSach NVARCHAR(100),
    GiaBan DECIMAL(18,2),
    MoTa NVARCHAR(MAX),
    AnhBia NVARCHAR(200),
    NgayCapNhat DATETIME,
    SoLuongTon INT,
    MaCD VARCHAR(10),
    MaNXB VARCHAR(10),
    CONSTRAINT FK_S_CD FOREIGN KEY (MaCD) REFERENCES CHUDE(MaCD),
    CONSTRAINT FK_S_NXB FOREIGN KEY (MaNXB) REFERENCES NHAXUATBAN(MaNXB)
);

--TẠO BẢNG KHACHHANG--
CREATE TABLE KHACHHANG (
    MaKH VARCHAR(10) PRIMARY KEY,
    HoTen NVARCHAR(100),
    TaiKhoan NVARCHAR(50),
    MatKhau NVARCHAR(50),
	Email NVARCHAR(100),
    DiaChiKH NVARCHAR(200),
    DienThoaiKH NVARCHAR(20),
    NgaySinh DATE
);

--TẠO BẢNG DONDATHANG--
CREATE TABLE DONDATHANG (
    MaDH VARCHAR(10) PRIMARY KEY,
    NgayDat DATETIME,
    NgayGiao DATETIME,
    TinhTrangGiaoHang NVARCHAR(50),
    DaThanhToan BIT,
    MaKH VARCHAR(10),
    CONSTRAINT FK_DDH_KH FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH)
);

--TẠO BẢNG CHITIETDONHANG--
CREATE TABLE CHITIETDONHANG (
    MaDH VARCHAR(10),
    MaSach VARCHAR(10),
    SoLuong INT,
    DonGia DECIMAL(18, 2),
    PRIMARY KEY (MaDH, MaSach),
    CONSTRAINT FK_CTDH_DDH FOREIGN KEY (MaDH) REFERENCES DONDATHANG(MaDH),
    CONSTRAINT FK_CTDH_S FOREIGN KEY (MaSach) REFERENCES SACH(MaSach)
);

--TẠO BẢNG VIETSACH--
CREATE TABLE VIETSACH (
    MaTG VARCHAR(10),
    MaSach VARCHAR(10),
	ViTri NVARCHAR(100),
	GhiChu NVARCHAR(200),
    PRIMARY KEY (MaTG, MaSach),
    CONSTRAINT FK_VS_TG FOREIGN KEY (MaTG) REFERENCES TACGIA(MaTG),
    CONSTRAINT FK_VS_S FOREIGN KEY (MaSach) REFERENCES SACH(MaSach)
);

--NHẬP LIỆU BẢNG TACGIA--
INSERT INTO TACGIA VALUES 
('TG01', N'Nguyễn Nhật Ánh', N'TP.HCM', N'Tác giả văn học thiếu nhi nổi tiếng.', N'0909123456'),
('TG02', N'Trần Văn Hùng', N'Hà Nội', N'Giảng viên công nghệ thông tin.', N'0912345678'),
('TG03', N'Lê Thị Mai', N'TP.HCM', N'Chuyên gia tài chính – kinh tế.', N'0987123456');

SELECT*FROM TACGIA;

--NHẬP LIỆU BẢNG NHAXUATBAN--
INSERT INTO NHAXUATBAN VALUES 
('NXB01', N'NXB Giáo Dục', N'Hà Nội', N'024-38512345'),
('NXB02', N'NXB Trẻ', N'TP.HCM', N'028-38224567'),
('NXB03', N'NXB Lao Động', N'Đà Nẵng', N'0236-3881122');

SELECT*FROM NHAXUATBAN;

--NHẬP LIỆU BẢNG CHUDE--
INSERT INTO CHUDE VALUES 
('CD01', N'Tin học'),
('CD02', N'Văn học'),
('CD03', N'Kinh tế');

SELECT*FROM CHUDE;

--NHẬP LIỆU BẢNG SACH--
INSERT INTO SACH VALUES 
('S01', N'Tôi thấy hoa vàng trên cỏ xanh', 95000, N'Truyện dài nổi tiếng của Nguyễn Nhật Ánh.', N'hoavang.jpg', '2023-06-10', 120, 'CD02', 'NXB02'),
('S02', N'Lập trình C cơ bản', 120000, N'Giáo trình lập trình C cho sinh viên CNTT.', N'laptrinhc.jpg', '2023-04-12', 75, 'CD01', 'NXB01'),
('S03', N'Nguyên lý kinh tế học', 150000, N'Sách tham khảo cho sinh viên khối kinh tế.', N'kinhte.jpg', '2023-05-08', 60, 'CD03', 'NXB03');

SELECT*FROM SACH;

--NHẬP LIỆU BẢNG KHACHHANG--
INSERT INTO KHACHHANG VALUES 
('KH01', N'Lê Minh Tuấn', N'tuanle', N'123456', N'tuanle@gmail.com', N'Quận 1, TP.HCM', N'0909000111', '1999-06-15'),
('KH02', N'Nguyễn Thị Hoa', N'hoant', N'654321', N'hoant@gmail.com', N'Cầu Giấy, Hà Nội', N'0909888777', '2000-10-20');

SELECT*FROM KHACHHANG;

--NHẬP LIỆU BẢNG DONDATHANG--
INSERT INTO DONDATHANG VALUES 
('DH01', '2023-07-01', '2023-07-05', N'Đã giao', 1, 'KH01'),
('DH02', '2023-07-10', '2023-07-15', N'Chưa giao', 0, 'KH02');

SELECT*FROM DONDATHANG;

--NHẬP LIỆU BẢNG CHITIETDONHANG--
INSERT INTO CHITIETDONHANG VALUES 
('DH01', 'S01', 1, 95000),
('DH01', 'S02', 1, 120000),
('DH02', 'S03', 2, 150000);

SELECT*FROM CHITIETDONHANG;

--NHẬP LIỆU BẢNG VIETSACH--
INSERT INTO VIETSACH VALUES 
('TG01', 'S01', N'Tác giả chính', N'Tác giả viết toàn bộ nội dung truyện'),
('TG02', 'S02', N'Đồng tác giả', N'Biên soạn nội dung chương trình giảng dạy C cơ bản'),
('TG03', 'S03', N'Tác giả chính', N'Tác giả biên soạn nội dung chính sách và nguyên lý kinh tế');

SELECT*FROM VIETSACH;
