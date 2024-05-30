using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ManagerDoctors.Data;


namespace ManagerDoctors.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly AppointmentContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public DoctorsController(AppointmentContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [TempData]
        public string StatusMessage { set; get; }
        public string ErrorMessage { set; get; }
        public IFormFile? uploadAvatar { set; get; }
		
		// GET: Doctors
		public async Task<IActionResult> Index(string searchString, int? page)
        {
            ViewData["CurrentFilter"] = searchString;

            var doctors = from doctor in _context.Doctors
                            select doctor;

            if (!string.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(sv => sv.Id.Contains(searchString));
            }

            int pageSize = 5; // Số lượng mục trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại (nếu không được chỉ định, mặc định là trang 1)

            var pagedDoctors = doctors.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = (int)Math.Ceiling((double)doctors.Count() / pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(pagedDoctors);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Date,Gender,Phone,Address,Avatar")] Doctor doctor, IFormFile? uploadAvatar)
        {
            if (ModelState.IsValid)
            {
				_context.Add(doctor);
				await _context.SaveChangesAsync();
                    if (uploadAvatar != null && uploadAvatar.Length > 0)
                    {
                        // Tạo tên tệp duy nhất bằng cách sử dụng Id của doctor
                        string uniqueFileName = "doctor" + doctor.Id + Path.GetExtension(uploadAvatar.FileName);

                        // Tạo đường dẫn lưu trữ tệp trong thư mục wwwroot/Upload/img
                        string path = Path.Combine(_webHostEnvironment.WebRootPath, "Upload/img", uniqueFileName);

                        // Lưu tệp tải lên vào đường dẫn trên
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await uploadAvatar.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn tệp vào thuộc tính Avatar của doctor
                        doctor.Avatar = "Upload/img/" + uniqueFileName;

                        // Lưu thay đổi cập nhật đường dẫn Avatar vào cơ sở dữ liệu
                        _context.Update(doctor);
                        await _context.SaveChangesAsync();
                    }
				StatusMessage = "Add success";
                return Redirect(Url.Action("Index", "Doctors"));
            }
            return View(doctor);
        }


        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,Email,Date,Gender,Phone,Address,Avatar")] Doctor doctor, IFormFile? uploadAvatar)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadAvatar != null && uploadAvatar.Length > 0)
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(doctor.Avatar))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, doctor.Avatar);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Tạo tên tệp duy nhất bằng cách sử dụng Id của doctor
                        string uniqueFileName = "doctor" + doctor.Id + Path.GetExtension(uploadAvatar.FileName);

                        // Tạo đường dẫn lưu trữ tệp trong thư mục wwwroot/Upload/img
                        string path = Path.Combine(_webHostEnvironment.WebRootPath, "Upload/img", uniqueFileName);

                        // Lưu tệp tải lên vào đường dẫn trên
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await uploadAvatar.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn tệp vào thuộc tính Avatar của doctor
                        doctor.Avatar = "Upload/img/" + uniqueFileName;
                    }

                    // Cập nhật thông tin khác của Doctor
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();

                    StatusMessage = "Edit success";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        ErrorMessage = "Edit failed";
                        return NotFound();
                    }
                    else
                    {
                        ErrorMessage = "Edit failed";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
				if (!string.IsNullOrEmpty(doctor.Avatar))
				{
					string filePath = Path.Combine(_webHostEnvironment.WebRootPath, doctor.Avatar);
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
				}
				_context.Doctors.Remove(doctor);
                StatusMessage = "Delete success";
            }

            await _context.SaveChangesAsync();
            return Redirect(Url.Action("Index", "Doctors"));
        }

        private bool DoctorExists(string id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
