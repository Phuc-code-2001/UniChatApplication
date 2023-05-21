using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Daos
{
    public class ProfileDAOs
    {
        /// <summary>
        /// Get Profile from Account 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="account"></param>
        /// <returns>Profile of given Account</returns>
        public static async Task<Profile> GetProfile(UniChatDbContext context, Account account)
        {
            if (account.RoleName == "Student")
            {
                StudentProfile profile = await context.StudentProfile
                                        .Include(p => p.Account)
                                        .Include(p => p.Class)
                                        .Include(p => p.Class.RoomChats)
                                        .Include(p => p.Class.StudentProfiles).OrderBy(p => p.Id)
                                        .FirstOrDefaultAsync(p => p.AccountID == account.Id);
                return profile;
            }
            if (account.RoleName == "Teacher")
            {
                TeacherProfile profile = await context.TeacherProfile
                                        .Include(p => p.Account)
                                        .Include(p => p.RoomChats).OrderBy(p => p.Id)
                                        .FirstOrDefaultAsync(p => p.AccountID == account.Id);
                return profile;
            }
            if (account.RoleName == "Admin")
            {
                AdminProfile profile = await context.AdminProfile
                        .Include(p => p.Account).OrderBy(p => p.Id)
                        .FirstOrDefaultAsync(p => p.AccountID == account.Id);
                return profile;
            }
            return null;
        }
        /// <summary>
        /// Get All Profile of Students
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of StudentProfile</returns>
        public static IQueryable<StudentProfile> getAllStudents(UniChatDbContext context)
        {

            return context.StudentProfile
                    .Include(p => p.Account)
                    .Include(p => p.Class)
                    .Include(p => p.GroupManages);
        }
        /// <summary>
        /// get All Profile of Teachers
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of TeacherProfile</returns>
        public static IQueryable<TeacherProfile> getAllTeachers(UniChatDbContext context)
        {

            return context.TeacherProfile
                    .Include(p => p.Account)
                    .Include(p => p.RoomChats);
        }

    }
}
