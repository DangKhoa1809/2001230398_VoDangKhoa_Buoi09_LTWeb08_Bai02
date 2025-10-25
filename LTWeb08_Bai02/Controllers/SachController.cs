using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTWeb08_Bai02.Models;
using System.Data.Entity;
using System.Net;

namespace LTWeb08_Bai02.Controllers
{
    public class SachController : Controller
    {
        QL_BookStoreEntities data = new QL_BookStoreEntities();

        public ActionResult DMSach()
        {
            var dsSach = data.SACHes.ToList();
            return View(dsSach);
        }

        [HttpGet]
        public ActionResult ThemMoiSach()
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.OrderBy(c => c.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            var sach = new SACH
            {
                NgayCapNhat = DateTime.Now,
            };
            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ThemMoiSach(SACH sach, HttpPostedFileBase ImageFile)
        {
            if (string.IsNullOrWhiteSpace(sach.TenSach))
                ModelState.AddModelError("TenSach", "Vui lòng nhập tên sách.");

            if (!sach.GiaBan.HasValue)
                ModelState.AddModelError("GiaBan", "Vui lòng nhập giá bán.");
            else if (sach.GiaBan <= 0)
                ModelState.AddModelError("GiaBan", "Giá bán phải lớn hơn 0.");

            if (!sach.SoLuongTon.HasValue)
                ModelState.AddModelError("SoLuongTon", "Vui lòng nhập số lượng tồn.");
            else if (sach.SoLuongTon < 0)
                ModelState.AddModelError("SoLuongTon", "Số lượng tồn phải >= 0.");

            if (string.IsNullOrWhiteSpace(sach.MaCD))
                ModelState.AddModelError("MaCD", "Vui lòng chọn chủ đề.");

            if (string.IsNullOrWhiteSpace(sach.MaNXB))
                ModelState.AddModelError("MaNXB", "Vui lòng chọn nhà xuất bản.");

            if (ImageFile == null || ImageFile.ContentLength == 0)
                ModelState.AddModelError("AnhBia", "Vui lòng chọn ảnh bìa.");

            if (ModelState.IsValid)
            {
                try
                {
                    string folder = Server.MapPath("~/Images");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string path = Path.Combine(folder, fileName);
                    ImageFile.SaveAs(path);
                    sach.AnhBia = fileName;

                    if (!sach.NgayCapNhat.HasValue)
                        sach.NgayCapNhat = DateTime.Now;

                    var lastSach = data.SACHes.OrderByDescending(s => s.MaSach).FirstOrDefault();
                    int nextId = 1;
                    if (lastSach != null && lastSach.MaSach.Length > 1)
                    {
                        int.TryParse(lastSach.MaSach.Substring(1), out nextId);
                        nextId++;
                    }
                    sach.MaSach = "S" + nextId.ToString("D2");

                    data.SACHes.Add(sach);
                    data.SaveChanges();

                    return RedirectToAction("DMSach");
                }
                catch (Exception ex)
                {
                    string err = ex.InnerException?.InnerException?.Message ?? ex.Message;
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + err);
                }
            }

            ViewBag.MaCD = new SelectList(data.CHUDEs.OrderBy(c => c.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }

        public ActionResult ChiTietSach(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sach = data.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN).FirstOrDefault(s => s.MaSach == id);

            if (sach == null)
            {
                return HttpNotFound();
            }

            return View(sach);
        }

        [HttpGet]
        public ActionResult XoaSach(string id)
        {
            if (id == null)
                return HttpNotFound();

            var sach = data.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN).SingleOrDefault(s => s.MaSach == id);

            if (sach == null)
                return HttpNotFound();

            return View(sach);
        }

        [HttpPost, ActionName("XoaSach")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            var sach = data.SACHes.SingleOrDefault(s => s.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            data.SACHes.Remove(sach);
            data.SaveChanges();

            return RedirectToAction("DMSach");
        }

        [HttpGet]
        public ActionResult SuaSach(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sach = data.SACHes.SingleOrDefault(s => s.MaSach == id);
            if (sach == null)
                return HttpNotFound();

            ViewBag.MaCD = new SelectList(data.CHUDEs.OrderBy(cd => cd.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.OrderBy(nxb => nxb.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SuaSach(SACH sach, HttpPostedFileBase fileUpload, string AnhBiaCu)
        {
            if (string.IsNullOrWhiteSpace(sach.MaCD))
                ModelState.AddModelError("MaCD", "Vui lòng chọn chủ đề.");

            if (string.IsNullOrWhiteSpace(sach.MaNXB))
                ModelState.AddModelError("MaNXB", "Vui lòng chọn nhà xuất bản.");

            if (!sach.NgayCapNhat.HasValue)
                ModelState.AddModelError("NgayCapNhat", "Vui lòng chọn ngày cập nhật.");

            if (!sach.SoLuongTon.HasValue)
                ModelState.AddModelError("SoLuongTon", "Vui lòng nhập số lượng tồn.");
            else if (sach.SoLuongTon < 0)
                ModelState.AddModelError("SoLuongTon", "Số lượng tồn phải >= 0.");

            if (!ModelState.IsValid)
            {
                ViewBag.MaCD = new SelectList(data.CHUDEs.OrderBy(cd => cd.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
                ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.OrderBy(nxb => nxb.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
                sach.AnhBia = AnhBiaCu;
                return View(sach);
            }

            var s = data.SACHes.SingleOrDefault(x => x.MaSach == sach.MaSach);
            if (s == null)
                return HttpNotFound();

            s.MaCD = sach.MaCD;
            s.MaNXB = sach.MaNXB;
            s.SoLuongTon = sach.SoLuongTon;
            s.NgayCapNhat = sach.NgayCapNhat ?? DateTime.Now;

            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                string folder = Server.MapPath("~/Images");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Path.GetFileName(fileUpload.FileName);
                string path = Path.Combine(folder, fileName);

                if (!System.IO.File.Exists(path))
                    fileUpload.SaveAs(path);

                s.AnhBia = fileName;
            }
            else
            {
                s.AnhBia = AnhBiaCu;
            }

            data.SaveChanges();

            return RedirectToAction("DMSach");
        }
    }
}
