using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniChatApplication.Daos;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Controllers
{
    public class SubjectController : Controller
    {
        readonly UniChatDbContext _context;

        public SubjectController(UniChatDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Mapping Index View of Subject Management
        /// </summary>
        /// <returns>View Index of Subject</returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");

            IEnumerable<Subject> subjects = SubjectDAOs.getAllSubject(_context).ToList();
            return View(subjects);
        }

        /// <summary>
        /// Mapping Create View of Subject Management
        /// </summary>
        /// <returns>View Create of Subject</returns>
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            return View();
        }

        /// <summary>
        /// Use to get data from Create View to create a subject
        /// </summary>
        /// <param name="sb"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult Create(Subject sb)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (ModelState.IsValid)
            {
                if (SubjectDAOs.isExitedSubject(_context, sb.SubjectCode))
                {
                    ViewData["Error"] = $"SubjectCode {sb.SubjectCode} is existed.";
                    return View(sb);
                }
                _context.Subjects.Add(sb);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sb);
        }

        /// <summary>
        /// Mapping to Delete View to confirm delete a subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Delete of Subject</returns>
        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == null) return Redirect("/Home/");

            Subject sb = SubjectDAOs.getAllSubject(_context).FirstOrDefault(s => s.Id == id);
            if (sb == null) return Redirect("/Home/");

            ViewData["RoomChats"] = RoomChatDAOs.getAllRoomChats(_context).Where(r => r.SubjectId == sb.Id);

            return View(sb);
        }


        /// <summary>
        /// Confirm delete a subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null) return Redirect("/Home/");

            Subject sb = _context.Subjects.Find(id);
            if (sb == null) return Redirect("/Home/");
            _context.Subjects.Remove(sb);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
