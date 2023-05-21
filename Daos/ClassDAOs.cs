using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Daos
{

    public class ClassDAOs
    {
        /// <summary>
        /// Get all classes
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of class</returns>
        public static IQueryable<Class> getAllClasses(UniChatDbContext context)
        {
            return context.Class.Include(c => c.StudentProfiles)
                                .Include(c => c.RoomChats);
        }
        /// <summary>
        /// Check class is existed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns>Check class if it is existed</returns>
        public static bool isExistedClass(UniChatDbContext context, string name)
        {
            return context.Class.Any(c => c.Name == name);
        }

    }

}