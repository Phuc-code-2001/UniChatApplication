using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Daos
{
    public class RoomChatDAOs
    {
        /// <summary>
        /// Get All RoomChats
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of RoomChat</returns>
        public static IQueryable<RoomChat> getAllRoomChats(UniChatDbContext context)
        {
            return context.RoomChats
                    .Include(r => r.Class)
                    .Include(r => r.Class.StudentProfiles)
                    .Include(r => r.Subject)
                    .Include(r => r.TeacherProfile)
                    .Include(r => r.RoomMessages)
                    .Include(r => r.GroupChats);
        }
        /// <summary>
        /// Check RoomChat Is Existed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="classId"></param>
        /// <param name="SubjectId"></param>
        /// <returns>true if RoomChat is existed, else return false.</returns>
        public static bool RoomChatExists(UniChatDbContext context, int classId, int SubjectId)
        {
            return context.RoomChats.Any(r => r.ClassId == classId && r.SubjectId == SubjectId);
        }

    }
}