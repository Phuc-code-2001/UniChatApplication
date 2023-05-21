

using System.Linq;
using UniChatApplication.Models;
using UniChatApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace UniChatApplication.Daos
{

    public class GroupManageDAOs
    {
        /// <summary>
        /// Get All Group Data
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of GroupManage</returns>
        public static IQueryable<GroupManage> getAllGroupData(UniChatDbContext context)
        {
            return context.GroupManages
                            .Include(m => m.GroupChat)
                            .Include(m => m.GroupChat.GroupManages)
                            .Include(m => m.StudentProfile);
        }

    }

}