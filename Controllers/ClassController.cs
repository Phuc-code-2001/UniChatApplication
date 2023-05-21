using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniChatApplication.Daos;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Controllers
{
    public class ClassController : Controller
    {
        private readonly UniChatDbContext _context;

        public ClassController(UniChatDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Mapping Index of ClassManagement Index
        /// </summary>
        /// <returns>View Index of Class</returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");

            IEnumerable<Class> classes = ClassDAOs.getAllClasses(_context).ToList();
            return View(classes);
        }

        /// <summary>
        /// Mapping to Details of Class
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View of Detail of Class</returns>
        public IActionResult Details(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            Class c = ClassDAOs.getAllClasses(_context).FirstOrDefault(c => c.Id == id);
            if (c == null) return Redirect("/Home/");

            ViewData["RoomChats"] = RoomChatDAOs.getAllRoomChats(_context).Where(r => r.ClassId == id);

            return View(c);
        }


        /// <summary>
        /// Mapping Create of ClassManagement
        /// </summary>
        /// <returns>View Create of Class</returns>
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");

            return View();
        }

        /// <summary>
        /// Get data from view and create Class
        /// </summary>
        /// <param name="c"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Class c)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");

            if (ClassDAOs.isExistedClass(_context, c.Name))
            {
                ViewData["Error"] = $"Class {c.Name} existed.";
                return View(c);
            }

            _context.Class.Add(c);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Mapping  Edit of ClassManagement
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Edit of Class</returns>
        public IActionResult Edit(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            Class c = ClassDAOs.getAllClasses(_context).FirstOrDefault(c => c.Id == id);
            if (c == null) return Redirect("/Home/");

            IEnumerable<StudentProfile> students = ProfileDAOs.getAllStudents(_context).Where(s => s.ClassID == null);
            ViewData["students"] = students;

            return View(c);
        }

        /// <summary>
        /// Use to add a student to a class
        /// </summary>
        /// <param name="stId"></param>
        /// <param name="classId"></param>
        [HttpPost]
        public void AddStudent(int? stId, int? classId)
        {

            if (stId == null || classId == null) return;

            StudentProfile student = ProfileDAOs.getAllStudents(_context).FirstOrDefault(s => s.Id == stId);
            Class c = _context.Class.FirstOrDefault(c => c.Id == classId);

            if (student != null && c != null && student.Class == null)
            {
                student.ClassID = classId;
                _context.StudentProfile.Update(student);
                _context.SaveChanges();
            }

            // System.Console.WriteLine($"AddStudent: {stId} -> {classId}");
        }

        /// <summary>
        /// Use to remove a student to a class
        /// </summary>
        /// <param name="stId"></param>
        /// <param name="classId"></param>
        [HttpPost]
        public void RemoveStudent(int? stId, int? classId)
        {
            if (stId == null || classId == null) return;

            StudentProfile student = ProfileDAOs.getAllStudents(_context).FirstOrDefault(s => s.Id == stId);
            Class c = _context.Class.FirstOrDefault(c => c.Id == classId);

            if (student != null && c != null && student.ClassID == classId)
            {
                student.ClassID = null;
                student.Class = null;
                _context.StudentProfile.Update(student);
                _context.SaveChanges();
            }

            // System.Console.WriteLine($"RemoveStudent: {stId} <- {classId}");
        }

        /// <summary>
        /// Mapping Delete of ClassManagment
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Delete of Class</returns>
        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            Class c = ClassDAOs.getAllClasses(_context).FirstOrDefault(c => c.Id == id);
            if (c == null) return Redirect("/Home/");

            ViewData["RoomChats"] = RoomChatDAOs.getAllRoomChats(_context).Where(r => r.ClassId == id);

            return View(c);
        }



        /// <summary>
        /// Use to delete a class
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            Class c = ClassDAOs.getAllClasses(_context).FirstOrDefault(c => c.Id == id);
            if (c == null) return Redirect("/Home/");

            _context.Class.Remove(c);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
