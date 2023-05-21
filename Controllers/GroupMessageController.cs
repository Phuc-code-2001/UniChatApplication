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

    public class GroupMessageController : Controller
    {

        readonly UniChatDbContext _context;

        public GroupMessageController(UniChatDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Use to add new message into GroupChat
        /// </summary>
        /// <param name="GroupId">Id of GroupChat</param>
        /// <param name="Message">New Message</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Add(int GroupId, string Message)
        {
            if (HttpContext.Session.GetString("Role") != "Student") return BadRequest();

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            GroupChat groupChat = GroupChatDAOs.getAllGroupChats(_context).FirstOrDefault(r => r.Id == GroupId);

            if (groupChat.GroupManages.Any(d => d.StudentId == LoginUser.StudentProfile.Id))
            {
                GroupMessage NewMessage = new GroupMessage()
                {
                    GroupId = GroupId,
                    AccountId = LoginUser.Id,
                    Content = Message,
                    TimeMessage = DateTime.Now
                };

                groupChat.Messages.Add(NewMessage);
                await _context.SaveChangesAsync();

                var response = new
                {
                    id = NewMessage.Id,
                    GroupId = GroupId,
                    username = LoginUser.Username,
                    avatar = (await ProfileDAOs.GetProfile(_context, LoginUser)).Avatar,
                    message = Message,
                    time = NewMessage.TimeMessage.ToShortTimeString()
                };

                return Ok(response);
            }

            return BadRequest();

        }

        /// <summary>
        /// Use to load more messages in GroupChat
        /// </summary>
        /// <param name="GroupId">Id of GroupChat</param>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> LoadMoreGroupMessages(int GroupId)
        {
            if (HttpContext.Session.GetString("Role") != "Student") return BadRequest();

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            Profile LoginProfile = await ProfileDAOs.GetProfile(_context, LoginUser);
            GroupChat GroupChat = GroupChatDAOs.getAllGroupChats(_context).FirstOrDefault(r => r.Id == GroupId);
            if (GroupChat == null) return BadRequest();

            // Check GroupChat if it includes LoginUser
            bool checkLoginUserInGroup = _context.GroupManages
                                        .Any(d => d.StudentId == LoginProfile.Id && d.GroupId == GroupId);
            if (!checkLoginUserInGroup) return BadRequest();

            // Load more messages
            int NumberOfMessageSended = HttpContext.Session.GetInt32($"Group{GroupId}NumberOfMessageSended") ?? 0;

            IEnumerable<GroupMessage> GroupMessages = GroupMessageDAOs.Take(_context, GroupId, NumberOfMessageSended, BoxController.numberOfMessagesOnEachLoad).Reverse();

            List<object> messages = new List<object>();
            foreach (GroupMessage item in GroupMessages)
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

            HttpContext.Session.SetInt32($"Group{GroupId}NumberOfMessageSended", NumberOfMessageSended + GroupMessages.Count());

            return Ok(messages);

        }

    }
}