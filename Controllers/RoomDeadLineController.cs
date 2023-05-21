using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniChatApplication.Models;
using UniChatApplication.Daos;
using UniChatApplication.Data;

namespace UniChatApplication.Controllers
{
    public class RoomDeadLineController : Controller
    {

        readonly UniChatDbContext _context;

        public RoomDeadLineController(UniChatDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// View All RoomDeadLine of a room
        /// </summary>
        /// <param name="RoomId"></param>
        /// <returns>View of RoomDeadLine</returns>
        public async Task<IActionResult> View(int RoomId)
        {
            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == RoomId);
            if (roomChat == null) return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            if (LoginUser == null) return Redirect("/Home/");

            bool CheckRoomOfUser = roomChat.TeacherProfile.AccountID == LoginUser.Id
                                || roomChat.Class.StudentProfiles.Any(s => s.AccountID == LoginUser.Id);

            if (!CheckRoomOfUser) return Redirect("/Home/");

            IEnumerable<RoomDeadLine> deadlineList = RoomDeadLineDAOs.GetAllOfRoom(_context, RoomId);
            IEnumerable<RoomDeadLine> newDeadLineList = deadlineList.Where(d => d.ExpirationTime > DateTime.Now);
            IEnumerable<RoomDeadLine> oldDeadLineList = deadlineList.Where(d => d.ExpirationTime <= DateTime.Now);

            ViewData["RoomChat"] = roomChat;
            ViewData["LoginUser"] = LoginUser;
            ViewData["OldDeadLineList"] = oldDeadLineList;

            return View(newDeadLineList);
        }

        /// <summary>
        /// Mapping to view that create a new room deadline
        /// </summary>
        /// <param name="RoomId"></param>
        /// <returns>View Create of RoomDeadLine</returns>
        public async Task<IActionResult> Create(int RoomId)
        {

            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == RoomId);
            if (roomChat == null) return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            if (LoginUser == null) return Redirect("/Home/");

            bool CheckRoomOfUser = roomChat.TeacherProfile.AccountID == LoginUser.Id
                                || roomChat.Class.StudentProfiles.Any(s => s.AccountID == LoginUser.Id);

            if (!CheckRoomOfUser) return Redirect("/Home/");

            RoomDeadLine newRoomDeadLine = new RoomDeadLine()
            {
                RoomId = RoomId,
                RoomChat = roomChat
            };

            ViewData["ExpirationTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm").Replace(' ', 'T');
            return View(newRoomDeadLine);
        }

        /// <summary>
        /// Receive data from create new deadline form and add data to database
        /// </summary>
        /// <param name="RoomId"></param>
        /// <param name="deadline"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int RoomId, RoomDeadLine deadline)
        {

            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == RoomId);
            if (roomChat == null) return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            if (LoginUser == null) return Redirect("/Home/");

            bool CheckRoomOfUser = roomChat.TeacherProfile.AccountID == LoginUser.Id
                                || roomChat.Class.StudentProfiles.Any(s => s.AccountID == LoginUser.Id);

            if (!CheckRoomOfUser) return Redirect("/Home/");

            deadline.RoomId = RoomId;

            if (deadline.Content != null && deadline.Content.Trim() != "")
            {

                deadline.Content = deadline.Content.Trim();
                _context.RoomDeadLines.Add(deadline);
                await _context.SaveChangesAsync();
                return Redirect($"/RoomDeadLine/View?RoomId={RoomId}");

            }
            ViewData["Error"] = "Content of deadline can not be blank...";
            ViewData["ExpirationTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm").Replace(' ', 'T');

            deadline.RoomChat = roomChat;
            return View(deadline);

        }

        /// <summary>
        /// Mapping Delete of RoomDeadLine
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Dedete of RoomDeadLine</returns>
        public async Task<IActionResult> Delete(int id)
        {

            RoomDeadLine roomDeadLine = RoomDeadLineDAOs.GetAll(_context).FirstOrDefault(r => r.Id == id);
            if (roomDeadLine == null) return Redirect("/Home/");

            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == roomDeadLine.RoomId);
            if (roomChat == null) return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            if (LoginUser == null) return Redirect("/Home/");

            bool CheckRoomOfUser = roomChat.TeacherProfile.AccountID == LoginUser.Id;

            if (!CheckRoomOfUser) return Redirect("/Home/");

            _context.RoomDeadLines.Remove(roomDeadLine);
            await _context.SaveChangesAsync();

            return RedirectToAction("View", new { RoomId = roomChat.Id });
        }
        /// <summary>
        /// Mapping Edit of RoomDeadLine
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Edit of RoomDeadLine</returns>
        public async Task<IActionResult> Edit(int id)
        {
            RoomDeadLine roomDeadLine = RoomDeadLineDAOs.GetAll(_context).FirstOrDefault(r => r.Id == id);
            if (roomDeadLine == null) return Redirect("/Home/");

            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == roomDeadLine.RoomId);
            if (roomChat == null) return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            if (LoginUser == null) return Redirect("/Home/");

            bool CheckRoomOfUser = roomChat.TeacherProfile.AccountID == LoginUser.Id;

            if (!CheckRoomOfUser) return Redirect("/Home/");

            ViewData["ExpirationTime"] = roomDeadLine.ExpirationTime.ToString("yyyy-MM-dd HH:mm").Replace(' ', 'T');
            return View(roomDeadLine);

        }
        /// <summary>
        /// Edit Confirm of RoomDeadLine
        /// </summary>
        /// <param name="deadline"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEdit(RoomDeadLine deadline)
        {

            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == deadline.RoomId);
            if (roomChat == null) return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            if (LoginUser == null) return Redirect("/Home/");

            bool CheckRoomOfUser = roomChat.TeacherProfile.AccountID == LoginUser.Id;

            if (!CheckRoomOfUser) return Redirect("/Home/");

            if (deadline.Content != null && deadline.Content.Trim() != "")
            {

                deadline.Content = deadline.Content.Trim();

                _context.RoomDeadLines.Update(deadline);
                await _context.SaveChangesAsync();
                return Redirect($"/RoomDeadLine/View?RoomId={deadline.RoomId}");

            }
            ViewData["Error"] = "Content of deadline can not be blank...";

            ViewData["ExpirationTime"] = deadline.ExpirationTime.ToString("yyyy-MM-dd HH:mm").Replace(' ', 'T');
            return View("Edit", deadline);

        }
    }
}