using System.Collections.Generic;
using UniChatApplication.Models;
using UniChatApplication.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace UniChatApplication.Daos
{

    public class GroupMessageDAOs
    {

        /// <summary>
        /// Get All Group Message
        /// </summary>
        /// <param name="_context"></param>
        /// <returns>List of GroupMessage</returns>
        public static IQueryable<GroupMessage> getAll(UniChatDbContext _context)
        {
            return _context.GroupMessages
                            .Include(m => m.Account)
                            .Include(m => m.Account.StudentProfile)
                            .Include(m => m.Account.TeacherProfile)
                            .Include(m => m.GroupChat)
                            .Include(m => m.GroupChat.GroupManages);
        }
        /// <summary>
        /// Get All Messages in a group
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="GroupID"></param>
        /// <returns>List of GroupMessage</returns>
        public static IQueryable<GroupMessage> messagesOfGroup(UniChatDbContext _context, int GroupID)
        {
            return getAll(_context).Where(m => m.GroupId == GroupID).OrderBy(m => m.TimeMessage);
        }
        /// <summary>
        /// Take Some Messages in a group
        /// </summary>
        /// <param name="context"></param>
        /// <param name="GroupId"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns>List of GroupMessage</returns>
        public static IEnumerable<GroupMessage> Take(UniChatDbContext context, int GroupId, int start, int count)
        {
            return messagesOfGroup(context, GroupId).ToList().SkipLast(start).TakeLast(count);
        }

    }

}