using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniChatApplication.Daos;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Controllers
{
    public class UserProfileController : Controller
    {

        readonly UniChatDbContext _context;

        public UserProfileController(UniChatDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Mapping Index of UserProfile
        /// </summary>
        /// <returns>View Index of UserProfile</returns>
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Role") != "Student"
            && HttpContext.Session.GetString("Role") != "Teacher") return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            Profile profile = await ProfileDAOs.GetProfile(_context, LoginUser);

            ViewData["LoginUser"] = LoginUser;


            return View(profile);
        }
        /// <summary>
        /// Use Edit Phone 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns>EditPhone UserProfile</returns>
        public async Task<IActionResult> EditPhone(string phone)
        {
            if (HttpContext.Session.GetString("Role") != "Student"
            && HttpContext.Session.GetString("Role") != "Teacher") return BadRequest();

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);

            if (phone != null && !Regex.IsMatch(phone, @"^[0-9]{10}$"))
            {
                return BadRequest();
            }

            if (LoginUser.RoleName == "Student")
            {
                LoginUser.StudentProfile.Phone = phone;
            }
            else
            {
                LoginUser.TeacherProfile.Phone = phone;
            }
            _context.Update(LoginUser);
            await _context.SaveChangesAsync();

            return Ok(new { phone = phone });
        }
        /// <summary>
        /// Use Edit BirthDay
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns>EditBirthDay UserProfile</returns>
        public async Task<IActionResult> EditBirthDay(string birthday)
        {
            if (HttpContext.Session.GetString("Role") != "Student"
            && HttpContext.Session.GetString("Role") != "Teacher") return BadRequest();

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);

            DateTime BirthDay = DateTime.Now;

            if (birthday != null && !DateTime.TryParse(birthday, out BirthDay))
            {
                return BadRequest();
            }

            if (LoginUser.RoleName == "Student")
            {
                LoginUser.StudentProfile.Birthday = BirthDay;
            }
            else
            {
                LoginUser.TeacherProfile.Birthday = BirthDay;
            }
            _context.Update(LoginUser);
            await _context.SaveChangesAsync();

            return Ok(new { birthday = BirthDay.ToShortDateString() });
        }
        
        /// <summary>
        /// UpdateAvatar of UserProfile
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(IFormFile imageFile)
        {
            if (HttpContext.Session.GetString("Role") == null) return Redirect("/Home/");
            if (imageFile == null) return BadRequest();
            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);

            if (LoginUser.RoleName == "Teacher")
            {
                TeacherProfile profile = (TeacherProfile) await ProfileDAOs.GetProfile(_context, LoginUser);

                string ImageName = $"id_{profile.Id}" + Path.GetExtension(imageFile.FileName);
                //Get url To Save
                string saveRelativePath = $"/media/profiles/teacherProfiles/";

                string savePath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot" + saveRelativePath + ImageName;

                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                profile.Avatar = saveRelativePath + ImageName;
                _context.TeacherProfile.Update(profile);
                await _context.SaveChangesAsync();

            }
            else if (LoginUser.RoleName == "Student")
            {
                StudentProfile profile = (StudentProfile) await ProfileDAOs.GetProfile(_context, LoginUser);

                string ImageName = $"id_{profile.Id}" + Path.GetExtension(imageFile.FileName);
                //Get url To Save
                string saveRelativePath = $"/media/profiles/studentProfiles/";

                string savePath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot" + saveRelativePath + ImageName;

                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                profile.Avatar = saveRelativePath + ImageName;
                _context.StudentProfile.Update(profile);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }

            return RedirectToAction("Index", new { id = LoginUser.Id });
        }
        /// <summary>
        /// Mapping UpdatePassword
        /// </summary>
        /// <returns>View UpdatePassword of UserProfile</returns>
        public async Task<IActionResult> UpdatePassword()
        {
            if (HttpContext.Session.GetString("Role") != "Student"
            && HttpContext.Session.GetString("Role") != "Teacher") return Redirect("/Home/");
            Account loginAccount = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);

            return View(loginAccount);
        }
        /// <summary>
        /// Mapping UpdatePassword
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns>IActionResult</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(string oldPassword, string newPassword)
        {
            if (HttpContext.Session.GetString("Role") != "Student"
            && HttpContext.Session.GetString("Role") != "Teacher") return Redirect("/Home/");
            Account loginAccount = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);

            if (newPassword == null || newPassword.Trim() == "") return BadRequest();

            Account temp = AccountDAOs.CreateAccount(loginAccount.Username, oldPassword, loginAccount.RoleID);

            if (temp.Password == loginAccount.Password)
            {
                Account newAccountInfo = AccountDAOs.CreateAccount(loginAccount.Username, newPassword, loginAccount.RoleID);
                if (newAccountInfo.Password != loginAccount.Password)
                {
                    loginAccount.Password = newAccountInfo.Password;
                    _context.Account.Update(loginAccount);
                    _context.SaveChanges();

                    TempData["UpdatePasswordStatus"] = true;
                    TempData["UpdatePasswordMessage"] = "Update Password Success.";
                }
                else
                {
                    TempData["UpdatePasswordStatus"] = false;
                    TempData["UpdatePasswordMessage"] = "New password is the same old password. Try again.";
                }
            }
            else
            {
                TempData["UpdatePasswordStatus"] = false;
                TempData["UpdatePasswordMessage"] = "Password input incorrect. Try again.";
            }

            return Redirect("UpdatePassword");
        }

    }
}