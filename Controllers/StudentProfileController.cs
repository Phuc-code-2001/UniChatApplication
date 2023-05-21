using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniChatApplication.Data;
using UniChatApplication.Models;
using UniChatApplication.Daos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ExcelDataReader;

namespace UniChatApplication.Controllers
{
    public class StudentProfileController : Controller
    {
        private readonly UniChatDbContext _context;

        public StudentProfileController(UniChatDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Mapping to Index view of Student Management
        /// </summary>
        /// <returns>View Index of StudentProfile</returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            IEnumerable<StudentProfile> students = ProfileDAOs.getAllStudents(_context).ToList();
            return View(students);
        }

        /// <summary>
        /// Mapping to detail page of Student Management
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Detail of StudentProfile</returns>
        public IActionResult Details(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            StudentProfile student = ProfileDAOs.getAllStudents(_context).FirstOrDefault(m => m.Id == id);
            if (student == null) return Redirect("/Home/");

            return View(student);
        }

        /// <summary>
        /// Mapping to Create View of Student Management
        /// </summary>
        /// <returns>View Create of StudentProfile</returns>
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            return View();
        }

        /// <summary>
        /// Use to get data from create view to create student account and profile
        /// </summary>
        /// <param name="student"></param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Create(StudentProfile student)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");


            student.Email = student.Email.ToLower();
            string username = AccountDAOs.getUsenameFromEmail(student.Email);

            if (AccountDAOs.AccountIsExisted(_context, username))
            {
                ViewData["Error"] = $"Account {username} existed...";
                return View(student);
            }

            if (_context.StudentProfile.Any(s => s.StudentCode == student.StudentCode))
            {
                ViewData["Error"] = $"StudentCode {student.StudentCode} existed...";
                return View(student);
            }

            student.Birthday = DateTime.Now.Date;

            student.Account = AccountDAOs.CreateAccount(username, AccountDAOs.DefaultPassword, 1);
            _context.Add(student);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Mapping to Edit View of Student Management
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Edit of StudentProfile</returns>
        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            StudentProfile studentProfile = ProfileDAOs.getAllStudents(_context).FirstOrDefault(p => p.Id == id);
            if (studentProfile == null) return Redirect("/Home/");

            return View(studentProfile);
        }

        /// <summary>
        /// Use to get data from edit view to edit information for a student
        /// </summary>
        /// <param name="id"></param>
        /// <param name="student"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, StudentProfile student)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            StudentProfile StudentCheckExisted = _context.StudentProfile.FirstOrDefault(s => s.StudentCode == student.StudentCode);
            if (StudentCheckExisted != null && StudentCheckExisted.Id != student.Id)
            {
                ViewData["Error"] = $"StudentCode {student.StudentCode} existed...";
                return View(student);
            }

            StudentProfile profile = ProfileDAOs.getAllStudents(_context).FirstOrDefault(p => p.Id == id);
            if (profile == null) return Redirect("/Home/");
            profile.FullName = student.FullName;
            profile.StudentCode = student.StudentCode;
            profile.Major = student.Major;
            profile.Gender = student.Gender;

            _context.Update(profile);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Mapping to delete view, show information to confirm delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Delete of StudentProfile</returns>
        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            StudentProfile studentProfile = ProfileDAOs.getAllStudents(_context).FirstOrDefault(p => p.Id == id);
            if (studentProfile == null) return Redirect("/Home/");

            return View(studentProfile);
        }

        /// <summary>
        /// Confirm delete student
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            StudentProfile studentProfile = ProfileDAOs.getAllStudents(_context).FirstOrDefault(p => p.Id == id);
            _context.Account.Remove(studentProfile.Account);
            _context.StudentProfile.Remove(studentProfile);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        /// <summary>
        /// Create Student By File
        /// </summary>
        /// <returns>Status of list status</returns>
        public IActionResult CreateByFile()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");

            ViewData["Result"] = TempData["Result"] ?? new List<string>();

            return View();
        }
        /// <summary>
        /// Create By File Student
        /// </summary>
        /// <param name="file"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult CreateByFile(IFormFile file)
        {
            if (file == null) return RedirectToAction("Index");
            if (!file.FileName.EndsWith(".xlsx")) return BadRequest();

            //Get url To Save
            string saveRelativePath = $"/files/";

            string savePath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot" + saveRelativePath + file.FileName;

            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            List<string> result = ProcessingData(file.FileName);
            TempData["Result"] = result;

            return RedirectToAction("CreateByFile");
        }
        /// <summary>
        /// Processing Data for Add StudentProfile
        /// </summary>
        /// <param name="fName"></param>
        /// <returns>List of Status Message</returns>
        private List<string> ProcessingData(string fName)
        {

            List<string> result = new List<string>();

            string fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (FileStream stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read(); // title
                    while (reader.Read())
                    {
                        try
                        {

                            string FullName = reader.GetValue(0).ToString();
                            string Email = reader.GetValue(1).ToString().ToLower();
                            string Gender = reader.GetValue(2).ToString();
                            string Major = reader.GetValue(3).ToString();
                            string StudentCode = reader.GetValue(4).ToString();
                            string ClassName = reader.GetValue(5).ToString();

                            string username = AccountDAOs.getUsenameFromEmail(Email);

                            if (AccountDAOs.AccountIsExisted(_context, username))
                            {
                                result.Add($"Error: Account {username} existed...");
                                continue;
                            }

                            if (_context.StudentProfile.Any(s => s.StudentCode == StudentCode))
                            {
                                result.Add($"Error: StudentCode {StudentCode} existed...");
                                continue;
                            }

                            StudentProfile st = new StudentProfile()
                            {
                                FullName = FullName,
                                Email = Email,
                                Gender = Gender.ToLower() == "male",
                                Major = Major,
                                StudentCode = StudentCode,
                                Birthday = DateTime.Now
                            };

                            st.Account = AccountDAOs.CreateAccount(username, AccountDAOs.DefaultPassword, 1);
                            st.Class = _context.Class.FirstOrDefault(c => c.Name == ClassName) ?? new Class() { Name = ClassName };

                            _context.Add(st);
                            _context.SaveChanges();

                            result.Add($"Success: Add student '{FullName}' successfully.");

                        }
                        catch (Exception)
                        {
                            // End of file
                            break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
