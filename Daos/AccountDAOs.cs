using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniChatApplication.Models;
using UniChatApplication.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace UniChatApplication.Daos
{
    public class AccountDAOs
    {
        //DefaultPassword Property
        public static string DefaultPassword = "123456";
        /// <summary>
        /// Create New Account
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns>Account with hashed password</returns>
        public static Account CreateAccount(string username, string password, int role)
        {

            var md5Hash = MD5.Create();

            // Byte array representation of source string
            var sourceBytes = Encoding.UTF8.GetBytes(password);

            // Generate hash value(Byte Array) for input data
            var hashBytes = md5Hash.ComputeHash(sourceBytes);

            // Convert hash byte array to string
            var hashed = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

            return new Account() { Username = username, Password = hashed, RoleID = role };

        }
        /// <summary>
        /// Check Account Is Existed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <returns>true if username is existed, else return false</returns>
        public static bool AccountIsExisted(UniChatDbContext context, string username)
        {
            return context.Account.Any(a => a.Username == username);
        }
        /// <summary>
        /// Check Account Validate
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>All messages validation</returns>
        public static Dictionary<string, string> AccountValidate(string username, string password)
        {

            var result = new Dictionary<string, string>();
            result["UsernameMessage"] = "";
            result["PasswordMessage"] = "";

            if (username == null || username.Trim().Length == 0) result["UsernameMessage"] = "Username can not be blank.";
            if (password == null || password.Trim().Length == 0) result["PasswordMessage"] = "Password can not be blank.";

            return result;

        }
        /// <summary>
        /// Get Login Account
        /// </summary>
        /// <param name="context"></param>
        /// <param name="session"></param>
        /// <returns>Account is logging</returns>
        public static async Task<Account> getLoginAccount(UniChatDbContext context, ISession session)
        {

            string username = session.GetString("username");

            if (username != null && username != "")
            {
                Account account = await context.Account
                                    .Include(m => m.AdminProfile)
                                    .Include(m => m.StudentProfile)
                                    .Include(m => m.TeacherProfile)
                                    .FirstOrDefaultAsync(a => a.Username == username);
                return account;
            }
            
            return null;
        }
        /// <summary>
        /// Get username from email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Username</returns>
        public static string getUsenameFromEmail(string email)
        {
            string username = "";
            for (int i = 0; i < email.Length; i++)
            {
                if (email[i] != '@')
                {
                    username += email[i];
                }
                else break;
            }
            return username;
        }
        /// <summary>
        /// Get All Student Account
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of student</returns>
        public static IQueryable<Account> getAllStudentAccount(UniChatDbContext context)
        {
            return context.Account
                    .Include(a => a.RoomMessages)
                    .Where(a => a.RoleName == "Student");
        }
        /// <summary>
        /// Get All Teacher Account
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of teacher</returns>
        public static IQueryable<Account> getAllTeacherAccount(UniChatDbContext context)
        {
            return context.Account
                    .Include(a => a.RoomMessages)
                    .Where(a => a.RoleName == "Teacher");
        }

    }
}
