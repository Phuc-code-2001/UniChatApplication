using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniChatApplication.Models;
using UniChatApplication.Data;
using UniChatApplication.Daos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace UniChatApplication.Controllers
{
    public class LoginController : Controller
    {

        readonly UniChatDbContext _context;
        public static ISession session;


        public LoginController(UniChatDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Mapping to Login View
        /// </summary>
        /// <returns>Login View</returns>
        public async Task<IActionResult> Index()
        {
            // Use session to detect login. Check Role
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                Account account = await _context.Account.FirstOrDefaultAsync(a => a.Username == username);

                LoginController.session = HttpContext.Session;

                if (account.RoleName == "Admin")
                {

                    HttpContext.Session.SetString("Role", "Admin");
                    return Redirect("/Admin/");
                }
                if (account.RoleName == "Teacher")
                {

                    HttpContext.Session.SetString("Role", "Teacher");
                    return Redirect("/Box/");
                }

                if (account.RoleName == "Student")
                {

                    HttpContext.Session.SetString("Role", "Student");
                    return Redirect("/Box/");
                }

            }
            else
            {

                //read cookie from Request object  
                string cookieValueFromReq = Request.Cookies["login"];
                if (cookieValueFromReq != null)
                {
                    LoginCookie cookie = _context.LoginCookies.FirstOrDefault(c => c.Key == cookieValueFromReq);
                    if (cookie != null)
                    {

                        if (cookie.ExpirationTime > DateTime.Now)
                        {

                            int userId = cookie.AccountID;
                            Account user = _context.Account.Find(userId);
                            if (user != null)
                            {
                                HttpContext.Session.SetString("username", user.Username);
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            DeleteLoginCookie();
                        }
                    }
                }

            }


            return View();
        }

        /// <summary>
        /// Get data from view to login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="remember"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password, bool remember)
        {

            var validator = AccountDAOs.AccountValidate(username, password);

            if (validator["UsernameMessage"] == string.Empty && validator["PasswordMessage"] == string.Empty)
            {

                Account LoginInfo = AccountDAOs.CreateAccount(username.Trim(), password.Trim(), -1);
                Account account = await _context.Account.FirstOrDefaultAsync(a => a.Username == LoginInfo.Username && a.Password == LoginInfo.Password);

                if (account != null)
                {
                    if (remember)
                    {
                        // Set login cookie
                        CookieOptions options = new CookieOptions();
                        options.Expires = DateTime.Now.AddMinutes(30);
                        string key;

                        while (true)
                        {
                            key = CookieDaos.CreateCookieLogin();
                            if (_context.LoginCookies.Count(c => c.Key == key) == 0)
                            {
                                break;
                            }
                        }

                        Response.Cookies.Append("login", key, options);
                        _context.LoginCookies.Add(new LoginCookie()
                        {
                            Key = key,
                            ExpirationTime = DateTime.Now.AddMinutes(30),
                            AccountID = account.Id
                        });
                        await _context.SaveChangesAsync();

                    }
                    else {
                        DeleteLoginCookie();
                    }

                    // Set login session
                    HttpContext.Session.SetString("username", LoginInfo.Username);
                    return RedirectToAction("Index");
                }
                else {
                    // Thông báo tên tài khoản hoặc mật khẩu k đúng. Quay về Login
                    ViewData["loginFailed"] = "Username or Password incorect..Try again.";

                }

            }
            else {
                // Đưa thông tin trong validator qua trang Login để thông báo
                ViewData["uerror"] = validator["UsernameMessage"];
                ViewData["perror"] = validator["PasswordMessage"];
            }

            return View("Index");

        }

        /// <summary>
        /// Logout function
        /// </summary>
        /// <returns>View Home</returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("Role");
            DeleteLoginCookie();
            return Redirect("/Home/");
        }

        /// <summary>
        /// Use to delete login cookie
        /// </summary>
        public void DeleteLoginCookie()
        {
            Response.Cookies.Delete("login");
        }

    }
}
