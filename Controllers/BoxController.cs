using Microsoft.AspNetCore.Mvc;
using UniChatApplication.Data;
using UniChatApplication.Daos;
using UniChatApplication.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace UniChatApplication.Controllers
{

    public class BoxController : Controller
    {
        readonly UniChatDbContext _context;
        public static readonly int numberOfMessagesOnEachLoad = 5;
        public static readonly int numberOfMessagesOnStartLoad = 10;

        public BoxController(UniChatDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Mapping Index
        /// </summary>
        /// <returns>View Index of Box</returns>
        public async Task<IActionResult> Index()
        {

            if (HttpContext.Session.GetString("Role") != "Student"
            && HttpContext.Session.GetString("Role") != "Teacher") return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            Profile LoginProfile = await ProfileDAOs.GetProfile(_context, LoginUser);

            IEnumerable<RoomChat> RoomChats = RoomChatDAOs.getAllRoomChats(_context)
                                                .Where(room => (room.TeacherProfile.AccountID == LoginUser.Id) || (room.Class.StudentProfiles.Any(student => student.AccountID == LoginUser.Id)));

            IEnumerable<RoomDeadLine> roomDeadLineList = RoomDeadLineDAOs.GetAll(_context)
                                                            .Where(r => RoomChats.Any(rc => rc.Id == r.RoomId))
                                                            .OrderByDescending(r => r.ExpirationTime).Take(8);

            if (LoginUser.RoleName == "Student")
            {
                // Get GroupChat List if LoginUser is student

                List<GroupChat> GroupChats = new List<GroupChat>();
                IEnumerable<GroupManage> groupDatas = GroupManageDAOs.getAllGroupData(_context)
                .Where(d => d.StudentId == LoginProfile.Id && d.GroupChat.RoomChat.ClassId == ((StudentProfile)LoginProfile).ClassID);
                foreach (GroupManage item in groupDatas)
                {
                    GroupChats.Add(item.GroupChat);
                }

                ViewData["GroupChats"] = GroupChats;
            }

            ViewData["RoomChats"] = RoomChats;
            ViewData["LoginUser"] = LoginUser;
            ViewData["RoomDeadLineList"] = roomDeadLineList;

            return View(LoginProfile);
        }


        /// <summary>
        /// Mapping to RoomChat, send data to RoomChat
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View RoomChat of Box</returns>
        public async Task<IActionResult> RoomChat(int? id)
        {

            if (HttpContext.Session.GetString("Role") != "Student"
            && HttpContext.Session.GetString("Role") != "Teacher") return Redirect("/Home/");
            if (id == null) return NotFound();

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            Profile LoginProfile = await ProfileDAOs.GetProfile(_context, LoginUser);

            // Get RoomChat List
            IEnumerable<RoomChat> RoomChats = RoomChatDAOs.getAllRoomChats(_context)
                                                .Where(room => (room.TeacherProfile.AccountID == LoginUser.Id) || (room.Class.StudentProfiles.Any(student => student.AccountID == LoginUser.Id)));

            // Get main RoomChat which is selected
            RoomChat RoomChat = RoomChats.FirstOrDefault(room => room.Id == id);
            if (RoomChat == null) return Redirect("/Home/");


            if (LoginUser.RoleName == "Student")
            {
                // Get GroupChat List if LoginUser is student

                List<GroupChat> GroupChats = new List<GroupChat>();
                IEnumerable<GroupManage> groupDatas = GroupManageDAOs.getAllGroupData(_context)
                .Where(d => d.StudentId == LoginProfile.Id && d.GroupChat.RoomChat.ClassId == ((StudentProfile)LoginProfile).ClassID);
                foreach (GroupManage item in groupDatas)
                {
                    GroupChats.Add(item.GroupChat);
                }

                ViewData["GroupChats"] = GroupChats;
            }
            else
            {
                // Get GroupChat List if LoginUser is Teacher
                IEnumerable<GroupChat> GroupChats = GroupChatDAOs.getAllGroupChats(_context)
                                                    .Where(g => g.RoomID == RoomChat.Id).OrderBy(g => g.Order)
                                                    .ToList();
                ViewData["GroupChats"] = GroupChats;
            }

            if (LoginUser.RoleName == "Student") ViewData["LoginProfile"] = (StudentProfile)LoginProfile;
            if (LoginUser.RoleName == "Teacher") ViewData["LoginProfile"] = (TeacherProfile)LoginProfile;

            ViewData["LoginUser"] = LoginUser;
            ViewData["RoomChats"] = RoomChats;
            ViewData["MessagePin"] = RoomMessagePinDAOs.GetMessagePinOfRoom(_context, RoomChat.Id);

            ViewData["Messages"] = RoomMessageDAOs.Take(_context, RoomChat.Id, 0, numberOfMessagesOnStartLoad);
            HttpContext.Session.SetInt32($"Room{RoomChat.Id}NumberOfMessageSended", numberOfMessagesOnStartLoad);


            return View(RoomChat);
        }


        /// <summary>
        /// Use to pin message into RoomChat
        /// </summary>
        /// <param name="roomMessageId"></param>
        /// <returns>View of PinMessage of Box</returns>
        public async Task<IActionResult> PinMessage(int roomMessageId)
        {

            RoomMessage message = RoomMessageDAOs.getAll(_context).FirstOrDefault(m => m.Id == roomMessageId);
            if (message == null) return BadRequest();

            bool CheckMessageBelongGroupOfUser = false;

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            Profile LoginProfile = await ProfileDAOs.GetProfile(_context, LoginUser);

            if (LoginUser.RoleName == "Teacher")
            {
                CheckMessageBelongGroupOfUser = ((TeacherProfile)LoginProfile).RoomChats
                .Any(room => room.Id == message.RoomID);
            }
            // RoleName = "Student"
            else
            {
                CheckMessageBelongGroupOfUser = ((StudentProfile)LoginProfile).Class.RoomChats
                .Any(room => room.Id == message.RoomID);
            }

            if (!CheckMessageBelongGroupOfUser) return BadRequest();


            RoomMessagePin messagePin = RoomMessagePinDAOs.GetAllMessagePinOfRoom(_context, message.RoomID.Value).FirstOrDefault(m => m.RoomMessage.Id == roomMessageId);

            if (messagePin == null)
            {
                messagePin = new RoomMessagePin()
                {
                    RoomMessageId = roomMessageId,
                    Time = DateTime.Now
                };
                _context.RoomMessagePins.Add(messagePin);
                _context.SaveChanges();
            }
            else
            {
                messagePin.Time = DateTime.Now;
                _context.RoomMessagePins.Update(messagePin);
                _context.SaveChanges();
            }

            return Ok(new { Content = message.Content, Time = messagePin.Time.ToShortTimeString() + " " + messagePin.Time.ToShortDateString() });
        }


        /// <summary>
        /// Mapping to GroupChat, send data to GroupChat
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View GroupChat of Box</returns>
        public async Task<IActionResult> GroupChat(int? id)
        {

            if (HttpContext.Session.GetString("Role") != "Student") return Redirect("/Home/");
            if (id == null) return NotFound();

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            Profile LoginProfile = await ProfileDAOs.GetProfile(_context, LoginUser);

            // Get RoomChat List
            IEnumerable<RoomChat> RoomChats = RoomChatDAOs.getAllRoomChats(_context)
                                                .Where(room => room.Class.StudentProfiles.Any(student => student.AccountID == LoginUser.Id));

            // Get GroupChat List
            IEnumerable<GroupChat> GroupChats = GroupChatDAOs.getAllGroupChats(_context)
                                                .Where(g => g.GroupManages.Any(m => m.StudentId == LoginProfile.Id))
                                                .ToList();
            ViewData["GroupChats"] = GroupChats;

            // Get main GroupChat which is selected
            GroupChat GroupChat = GroupChats.FirstOrDefault(g => g.Id == id);
            if (GroupChat == null) return NotFound();
            // Check groupChat if it contains LoginUser
            GroupManage groupData = GroupChat.GroupManages.FirstOrDefault(m => m.StudentId == LoginProfile.Id);
            if (groupData == null) return NotFound();

            ViewData["GroupRole"] = groupData.RoleText;

            if (LoginUser.RoleName == "Student") ViewData["LoginProfile"] = (StudentProfile)LoginProfile;
            if (LoginUser.RoleName == "Teacher") ViewData["LoginProfile"] = (TeacherProfile)LoginProfile;

            ViewData["LoginUser"] = LoginUser;
            ViewData["RoomChats"] = RoomChats;
            ViewData["MessagePin"] = GroupMessagePinDAOs.GetMessagePinOfGroup(_context, GroupChat.Id);
            // Load Messages
            ViewData["Messages"] = GroupMessageDAOs.Take(_context, GroupChat.Id, 0, numberOfMessagesOnStartLoad);
            HttpContext.Session.SetInt32($"Group{GroupChat.Id}NumberOfMessageSended", numberOfMessagesOnStartLoad);

            return View(GroupChat);
        }

        /// <summary>
        /// Use to pin message into GroupChat
        /// </summary>
        /// <param name="groupMessageId"></param>
        /// <returns>View of PinGroupMessage of Box</returns>
        public async Task<IActionResult> PinGroupMessage(int groupMessageId)
        {

            GroupMessage message = GroupMessageDAOs.getAll(_context).FirstOrDefault(m => m.Id == groupMessageId);
            if (message == null) return BadRequest();


            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            Profile LoginProfile = await ProfileDAOs.GetProfile(_context, LoginUser);

            bool CheckMessageBelongGroupOfUser = message.GroupChat.GroupManages.Any(d => d.StudentId == LoginProfile.Id);

            if (!CheckMessageBelongGroupOfUser) return BadRequest();


            GroupPinMessage messagePin = GroupMessagePinDAOs.GetAllMessagePinOfGroup(_context, message.GroupId.Value).FirstOrDefault(m => m.GroupMessage.Id == groupMessageId);

            if (messagePin == null)
            {
                messagePin = new GroupPinMessage()
                {
                    GroupMessageId = groupMessageId,
                    Time = DateTime.Now
                };
                _context.GroupPinMessages.Add(messagePin);
                _context.SaveChanges();
            }
            else
            {
                messagePin.Time = DateTime.Now;
                _context.GroupPinMessages.Update(messagePin);
                _context.SaveChanges();
            }

            return Ok(new { Content = message.Content, Time = messagePin.Time.ToShortTimeString() + " " + messagePin.Time.ToShortDateString() });
        }




    }

}