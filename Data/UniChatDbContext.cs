using Microsoft.EntityFrameworkCore;
using UniChatApplication.Models;

namespace UniChatApplication.Data
{
    //Database Context For UniChat System
    public class UniChatDbContext : DbContext
    {
        public UniChatDbContext(DbContextOptions<UniChatDbContext> options) : base(options) { }

        public DbSet<Account> Account { get; set; }

        public DbSet<Class> Class { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<AdminProfile> AdminProfile { get; set; }

        public DbSet<TeacherProfile> TeacherProfile { get; set; }

        public DbSet<StudentProfile> StudentProfile { get; set; }

        public DbSet<LoginCookie> LoginCookies { get; set; }

        public DbSet<RoomChat> RoomChats { get; set; }

        public DbSet<RoomMessage> RoomMessages { get; set; }

        public DbSet<RoomMessagePin> RoomMessagePins { get; set; }

        public DbSet<RoomDeadLine> RoomDeadLines { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<GroupChat> GroupChats { get; set; }

        public DbSet<GroupMessage> GroupMessages { get; set; }

        public DbSet<GroupPinMessage> GroupPinMessages { get; set; }

        public DbSet<GroupManage> GroupManages { get; set; }

    }
}
