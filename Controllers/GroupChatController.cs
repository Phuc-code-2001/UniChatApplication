using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniChatApplication.Models;
using UniChatApplication.Daos;
using UniChatApplication.Data;

namespace UniChatApplication.Controllers
{
    public class GroupChatController : Controller
    {

        readonly UniChatDbContext _context;

        public GroupChatController(UniChatDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Mapping to Create GroupChat View
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns>View Create of GroupChat</returns>
        public async Task<IActionResult> Create(int roomId)
        {
            if (HttpContext.Session.GetString("Role") != "Student") return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == roomId);
            if (roomChat == null) return NotFound();

            // Check RoomChat if it includes LoginUser
            if (!roomChat.Class.StudentProfiles.Any(s => s.AccountID == LoginUser.Id)) return BadRequest();
            ViewData["RoomChat"] = roomChat;

            return View();
        }

        /// <summary>
        /// Create of GroupChat
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="groupChat"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Create(int roomId, GroupChat groupChat)
        {
            if (HttpContext.Session.GetString("Role") != "Student") return Redirect("/Home/");

            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            RoomChat roomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == roomId);
            if (roomChat == null) return NotFound();
            ViewData["RoomChat"] = roomChat;

            // Check RoomChat if it includes LoginUser
            if (!roomChat.Class.StudentProfiles.Any(s => s.AccountID == LoginUser.Id)) return BadRequest();

            if (string.IsNullOrEmpty(groupChat.Name))
            {
                ViewData["ErrorMessage"] = $"GroupChat's name can not be blank.";
                return View();
            }

            List<GroupChat> groupChatOfRooms = roomChat.GroupChats.OrderBy(g => g.Order).ToList();
            if (groupChatOfRooms.Any(g => g.Name == groupChat.Name))
            {
                ViewData["ErrorMessage"] = $"GroupChat with name '{groupChat.Name}' existed. Try another.";
                return View();
            }

            // Set Group Order
            groupChat.RoomID = roomChat.Id;
            int groupOrder = 1;
            foreach (GroupChat item in groupChatOfRooms)
            {
                if (groupOrder == item.Order) groupOrder++;
                else break;
            }
            groupChat.Order = groupOrder;

            // Initialize leader for group
            StudentProfile LoginProfile = (StudentProfile) await ProfileDAOs.GetProfile(_context, LoginUser);

            if (GroupManageDAOs.getAllGroupData(_context).Any(m => m.StudentId == LoginProfile.Id && m.GroupChat.RoomID == roomChat.Id))
            {
                ViewData["ErrorMessage"] = $"You had a group in this room and can not create new group.";
                return View();
            }

            GroupManage groupData = new GroupManage()
            {
                GroupChat = groupChat,
                StudentProfile = LoginProfile,
                Role = true
            };

            _context.GroupManages.Add(groupData);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "Box", new { id = groupChat.Id });
        }

        /// <summary>
        /// Mapping Deatils of GroupChat
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View Detail of GroupChat</returns>
        public async Task<IActionResult> Details(int id)
        {

            if (HttpContext.Session.GetString("Role") == null) return Redirect("/Home/");
            GroupChat groupChat = GroupChatDAOs.getAllGroupChats(_context).FirstOrDefault(g => g.Id == id);
            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);

            if (groupChat == null) return NotFound();

            ViewData["LoginUser"] = LoginUser;
            ViewData["GroupDataList"] = GroupManageDAOs.getAllGroupData(_context).Where(d => d.GroupId == id);
            ViewData["RoomChat"] = groupChat.RoomChat;

            return View(groupChat);

        }


        /// <summary>
        /// Add more student into Group of LoginUser
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>View of Setting of GroupChat</returns>
        public async Task<IActionResult> Setting(int groupId)
        {

            if (HttpContext.Session.GetString("Role") != "Student") return Redirect("/Home/");
            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            StudentProfile LoginProfile = (StudentProfile) await ProfileDAOs.GetProfile(_context, LoginUser);

            // Get main GroupChat which is selected
            GroupChat GroupChat = GroupChatDAOs.getAllGroupChats(_context).FirstOrDefault(g => g.Id == groupId);
            if (GroupChat == null) return NotFound();
            // Check groupChat if it contains LoginUser
            GroupManage groupData = GroupChat.GroupManages.FirstOrDefault(m => m.StudentId == LoginProfile.Id && m.RoleText == "Leader");
            if (groupData == null) return NotFound();

            RoomChat RoomChat = RoomChatDAOs.getAllRoomChats(_context).FirstOrDefault(r => r.Id == GroupChat.RoomID);
            // Check RoomChat belong to LoginUser
            if (!RoomChat.Class.StudentProfiles.Any(s => s.AccountID == LoginUser.Id)) return BadRequest();

            IEnumerable<GroupManage> GroupDataList = GroupManageDAOs.getAllGroupData(_context)
                                        .Where(d => d.GroupId == groupId).OrderByDescending(d => d.Role);
            ViewData["GroupDataList"] = GroupDataList;

            IEnumerable<StudentProfile> StudentRestListOfRoom = RoomChat.Class.StudentProfiles
                                                .Where(st => !GroupDataList.Any(d => st.Id == d.StudentId));

            IEnumerable<GroupManage> GroupDataStudentRestList = GroupManageDAOs.getAllGroupData(_context)
                                                                .Where(d => d.GroupChat.RoomID == RoomChat.Id);

            StudentRestListOfRoom = StudentRestListOfRoom.Where(st => !GroupDataStudentRestList.Any(d => d.StudentId == st.Id));

            ViewData["StudentRestListOfRoom"] = StudentRestListOfRoom;

            ViewData["RoomChat"] = RoomChat;

            return View(GroupChat);
        }

        /// <summary>
        /// Add student into GroupChat
        /// </summary>
        /// <param name="GroupChatId"></param>
        /// <param name="StudentId"></param>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> Add(int GroupChatId, int StudentId)
        {

            if (HttpContext.Session.GetString("Role") != "Student") return BadRequest();
            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            StudentProfile LoginProfile = (StudentProfile) await ProfileDAOs.GetProfile(_context, LoginUser);

            GroupChat GroupChat = GroupChatDAOs.getAllGroupChats(_context).FirstOrDefault(g => g.Id == GroupChatId);
            if (GroupChat == null) return BadRequest();

            StudentProfile student = ProfileDAOs.getAllStudents(_context).FirstOrDefault(p => p.Id == StudentId);
            if (student == null) return BadRequest();

            //Check LoginUser if he/she is Leader of Group
            bool CheckLeader = GroupChat.GroupManages.Any(d => d.StudentId == LoginProfile.Id && d.RoleText == "Leader");
            if (CheckLeader == false) return BadRequest();

            RoomChat RoomChat = GroupChat.RoomChat;
            // Check student belong to RoomChat
            bool CheckStudentBelongToRoom = student.ClassID == RoomChat.ClassId;
            if (CheckStudentBelongToRoom == false) return BadRequest();

            IEnumerable<GroupChat> AllGroupOfRoom = RoomChat.GroupChats;

            bool CheckStudentHadGroup = AllGroupOfRoom.Any(g => g.GroupManages.Any(d => d.StudentId == StudentId));

            if (CheckStudentHadGroup) return BadRequest();

            // Add new Data into GroupManage
            _context.GroupManages.Add(new GroupManage()
            {
                StudentId = StudentId,
                GroupId = GroupChatId,
                Role = false,
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("Setting", new { groupId = GroupChatId });
        }

        /// <summary>
        /// Remove student out a Group
        /// </summary>
        /// <param name="GroupManageId"></param>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> Remove(int GroupManageId)
        {

            if (HttpContext.Session.GetString("Role") != "Student") return BadRequest();
            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            StudentProfile LoginProfile = (StudentProfile) await ProfileDAOs.GetProfile(_context, LoginUser);

            GroupManage GroupData = GroupManageDAOs.getAllGroupData(_context).FirstOrDefault(d => d.Id == GroupManageId);
            if (GroupData == null) return BadRequest();

            GroupChat GroupChat = GroupData.GroupChat;

            // Check LoginUser is Leader
            bool CheckLeader = GroupChat.GroupManages.Any(d => d.StudentId == LoginProfile.Id && d.RoleText == "Leader");
            // Check LoginUser left group
            bool CheckStudentSelfLeft = GroupData.StudentId == LoginProfile.Id;

            if (CheckLeader || CheckStudentSelfLeft)
            {
                _context.GroupManages.Remove(GroupData);
                await _context.SaveChangesAsync();
                return RedirectToAction("Setting", new { groupId = GroupChat.Id });
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Allow student leave a group
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> Left(int GroupId)
        {
            if (HttpContext.Session.GetString("Role") != "Student") return BadRequest();
            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            StudentProfile LoginProfile = (StudentProfile) await ProfileDAOs.GetProfile(_context, LoginUser);

            GroupManage GroupData = _context.GroupManages.FirstOrDefault(d => d.StudentId == LoginProfile.Id && d.GroupId == GroupId);
            if (GroupData == null) return BadRequest();

            if (GroupData.RoleText != "Member") return BadRequest();

            _context.GroupManages.Remove(GroupData);
            await _context.SaveChangesAsync();

            return Redirect("/Home/");

        }
        /// <summary>
        /// Destroy a GroupChat
        /// </summary>
        /// <param name="GroupChatId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Destroy(int GroupChatId)
        {

            if (HttpContext.Session.GetString("Role") != "Student") return BadRequest();
            Account LoginUser = await AccountDAOs.getLoginAccount(_context, HttpContext.Session);
            StudentProfile LoginProfile = (StudentProfile) await ProfileDAOs.GetProfile(_context, LoginUser);

            GroupChat GroupChat = GroupChatDAOs.getAllGroupChats(_context).FirstOrDefault(g => g.Id == GroupChatId);
            if (GroupChat == null) return BadRequest();

            //Check LoginUser if he/she is Leader of Group
            bool CheckLeader = GroupChat.GroupManages.Any(d => d.StudentId == LoginProfile.Id && d.RoleText == "Leader");
            if (CheckLeader == false) return BadRequest();

            _context.GroupManages.RemoveRange(GroupChat.GroupManages);
            _context.GroupChats.Remove(GroupChat);

            await _context.SaveChangesAsync();

            return Redirect("/Home/");

        }

    }
}