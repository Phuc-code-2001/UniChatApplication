using System.Collections.Generic;
using UniChatApplication.Models;
using UniChatApplication.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace UniChatApplication.Daos
{

    public class RoomMessageDAOs
    {

        /// <summary>
        /// Get All RoomMessage
        /// </summary>
        /// <param name="_context"></param>
        /// <returns>List of RoomMessage</returns>
        public static IQueryable<RoomMessage> getAll(UniChatDbContext _context)
        {
            return _context.RoomMessages
                            .Include(m => m.Account)
                            .Include(m => m.Account.StudentProfile)
                            .Include(m => m.Account.TeacherProfile)
                            .Include(m => m.RoomChat);
        }
        /// <summary>
        /// Get All Messages of Room
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="RoomID"></param>
        /// <returns>List of RoomMessage</returns>         
        public static IQueryable<RoomMessage> messagesOfRoom(UniChatDbContext _context, int RoomID)
        {
            return getAll(_context).Where(m => m.RoomID == RoomID).OrderBy(m => m.TimeMessage);
        }
        /// <summary>
        /// Take Some RoomMessage of Room
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RoomID"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns>List of RoomMessage</returns>
        public static IEnumerable<RoomMessage> Take(UniChatDbContext context, int RoomID, int start, int count)
        {
            return messagesOfRoom(context, RoomID).ToList().SkipLast(start).TakeLast(count);
        }

    }

}