using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Daos
{

    public class GroupMessagePinDAOs
    {
        /// <summary>
        /// Get All GroupPinMessage
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of GroupPinMessage</returns>
        public static IQueryable<GroupPinMessage> GetAll(UniChatDbContext context)
        {
            return context.GroupPinMessages
                    .Include(m => m.GroupMessage);
        }
        /// <summary>
        /// Get All MessagePin Of Group
        /// </summary>
        /// <param name="context"></param>
        /// <param name="GroupId"></param>
        /// <returns>List of GroupPinMessage</returns>
        public static IQueryable<GroupPinMessage> GetAllMessagePinOfGroup(UniChatDbContext context, int GroupId)
        {

            return GetAll(context).Where(m => m.GroupMessage.GroupId == GroupId);

        }
        /// <summary>
        /// Get Class MessagePin Of Group
        /// </summary>
        /// <param name="context"></param>
        /// <param name="GroupId"></param>
        /// <returns>GroupPinMessage</returns>
        public static GroupPinMessage GetMessagePinOfGroup(UniChatDbContext context, int GroupId)
        {
            return GetAllMessagePinOfGroup(context, GroupId)
                    .OrderBy(m => m.Time)
                    .LastOrDefault();
        }

    }

}