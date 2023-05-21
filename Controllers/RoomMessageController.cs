using System;
using UniChatApplication.Models;
using UniChatApplication.Data;
using UniChatApplication.Daos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace UniChatApplication.Controllers
{

    public class RoomMessageController : Controller
    {

        readonly UniChatDbContext _context;

        public RoomMessageController(UniChatDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Use to add new message into RoomChat
        /// </summary>
        /// <param name="RoomID">Id of RoomChat</param>
        /// <param name="Message">New Message</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Add(int RoomID, string Message)
        {
            if (HttpContext.Session.GetString("Role") != "Student"
            && HttpContext.Session.GetString("Role") != "Teacher") return BadRequest();

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            Profile LoginProfile = await ProfileDAOs.GetProfile(_context, LoginUser);
            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == RoomID);
            if (roomChat == null) return BadRequest();

            // Check RoomChat if it includes LoginUser
            if (roomChat.TeacherProfile.AccountID == LoginUser.Id
            || roomChat.ClassId == LoginUser.StudentProfile?.ClassID)
            {
                roomChat.RoomMessages.Add(new RoomMessage()
                {
                    RoomID = RoomID,
                    RoomChat = roomChat,
                    AccountID = LoginUser.Id,
                    Account = LoginUser,
                    Content = Message,
                    TimeMessage = DateTime.Now
                });
                await _context.SaveChangesAsync();
                RoomMessage roomMessage = roomChat.RoomMessages.OrderBy(r => r.TimeMessage).Last();

                var response = new
                {
                    id = roomMessage.Id,
                    roomId = roomChat.Id,
                    username = LoginUser.Username,
                    avatar = (await ProfileDAOs.GetProfile(_context, LoginUser)).Avatar,
                    message = roomMessage.Content,
                    time = roomMessage.TimeMessage.ToShortTimeString()
                };

                return Ok(response);
            }

            return BadRequest();

        }

        /// <summary>
        /// Use to load more messages in RoomChat
        /// </summary>
        /// <param name="RoomId">Id of RoomChat</param>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> LoadMoreRoomMessages(int RoomId)
        {
            if (HttpContext.Session.GetString("Role") != "Student"
            && HttpContext.Session.GetString("Role") != "Teacher") return BadRequest();

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            Profile LoginProfile = await ProfileDAOs.GetProfile(_context, LoginUser);
            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == RoomId);
            if (roomChat == null) return BadRequest();

            // Check RoomChat if it includes LoginUser
            bool checkLoginUserInRoom = roomChat.TeacherProfile.AccountID == LoginUser.Id
                                        || roomChat.ClassId == LoginUser.StudentProfile?.ClassID;
            if (!checkLoginUserInRoom) return BadRequest();

            // Load more messages
            int NumberOfMessageSended = HttpContext.Session.GetInt32($"Room{RoomId}NumberOfMessageSended") ?? 0;
            IEnumerable<RoomMessage> RoomMessages = RoomMessageDAOs.Take(_context, RoomId, NumberOfMessageSended, BoxController.numberOfMessagesOnEachLoad).Reverse();

            List<object> messages = new List<object>();
            foreach (RoomMessage item in RoomMessages)
            {
                // id, username, message, avatar, time
                object message = new
                {
                    id = item.Id,
                    username = item.Account.Username,
                    message = item.Content,
                    avatar = (await ProfileDAOs.GetProfile(_context, item.Account)).Avatar,
                    time = item.TimeMessage.ToShortTimeString()
                };

                messages.Add(message);
            }

            HttpContext.Session.SetInt32($"Room{RoomId}NumberOfMessageSended", NumberOfMessageSended + RoomMessages.Count());

            return Ok(messages);




        }

    }
}