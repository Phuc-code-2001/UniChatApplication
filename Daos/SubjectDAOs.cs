using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Daos
{

    public class SubjectDAOs
    {

        /// <summary>
        /// Get All Subject
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of Subject</returns>
        public static IQueryable<Subject> getAllSubject(UniChatDbContext context)
        {

            return context.Subjects.Include(s => s.RoomChats);

        }
        /// <summary>
        /// Check Subject Code is Exited 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="code"></param>
        /// <returns>true if Subject Code is existed, else return false.</returns>
        public static bool isExitedSubject(UniChatDbContext context, string code)
        {
            return context.Subjects.Any(s => s.SubjectCode == code);
        }

    }

}