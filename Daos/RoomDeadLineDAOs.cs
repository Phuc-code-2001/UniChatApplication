using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Daos
{
    public class RoomDeadLineDAOs
    {

        /// <summary>
        /// Get All RoomDeadLine
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of RoomDeadLine</returns>
        public static IEnumerable<RoomDeadLine> GetAll(UniChatDbContext context)
        {
            return context.RoomDeadLines.Include(d => d.RoomChat)
                                        .Include(d => d.RoomChat.Class)
                                        .Include(d => d.RoomChat.Subject)
                                        .Include(d => d.RoomChat.TeacherProfile);
        }

        /// <summary>
        /// Get All RoomDeadLine of Room 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RoomId"></param>
        /// <returns>List of RoomDeadLine</returns>
        public static IEnumerable<RoomDeadLine> GetAllOfRoom(UniChatDbContext context, int RoomId)
        {
            return GetAll(context).Where(d => d.RoomId == RoomId);
        }
        /// <summary>
        /// Get LastDeadLine of Room
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RoomId"></param>
        /// <returns>RoomDeadLine</returns>
        public static RoomDeadLine GetLastOfRoom(UniChatDbContext context, int RoomId)
        {
            return GetAllOfRoom(context, RoomId).OrderBy(d => d.Id).LastOrDefault();
        }

    }
}