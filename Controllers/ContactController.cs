using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UniChatApplication.Models;
using UniChatApplication.Data;
using Microsoft.AspNetCore.Http;

namespace UniChatApplication.Controllers
{
    public class ContactController : Controller
    {
        private readonly UniChatDbContext _logger;

        public ContactController(UniChatDbContext logger)
        {
            this._logger = logger;
        }
        /// <summary>
        /// Mapping Index of Contact
        /// </summary>
        /// <returns>View Index of Contact</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// add new request for contact
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Contact obj)
        {
            if (ModelState.IsValid)
            {
                this._logger.Contacts.Add(obj);
                this._logger.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        /// <summary>
        /// show list of contact
        /// </summary>
        /// <returns>View ContactMangeMent</returns>
        public IActionResult ContactManagement()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            IEnumerable<Contact> contactList = this._logger.Contacts;
            return View(contactList);
        }

        /// <summary>
        /// Mapping to Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Delete of Contact</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == 0)
            {
                return NotFound();
            }
            var obj = this._logger.Contacts.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        /// <summary>
        /// Delete a Contact
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteContact(int contactId)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            Contact obj = this._logger.Contacts.Find(contactId);
            if (obj == null) return NotFound();
            this._logger.Contacts.Remove(obj);
            this._logger.SaveChanges();
            return RedirectToAction("ContactManagement");
        }
        /// <summary>
        /// Mapping Update
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Update of Contact</returns>
        public IActionResult Update(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (id == 0)
            {
                return NotFound();
            }
            var obj = this._logger.Contacts.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        /// <summary>
        /// Update a Contact
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateContact(Contact obj)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Redirect("/Home/");
            if (ModelState.IsValid)
            {
                this._logger.Contacts.Update(obj);
                this._logger.SaveChanges();
                return RedirectToAction("ContactManagement");
            }
            return View(obj);
        }
    }
}