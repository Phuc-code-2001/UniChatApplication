using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace UniChatApplication.Models
{
    [Table("room_chat")]
    public class RoomChat
    {
        //ID Property
        [Key]
        public int Id { get; set; }
        //ClassId Property
        [Column("class_id")]
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        //Get and Set Class
        public Class Class { get; set; }
        //SubjectId Property
        [Column("subject_id")]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        //Get and Set Subject
        public Subject Subject { get; set; }
        //TeacherId Property
        [Column("teacher_id")]
        [ForeignKey("TeacherProfile")]
        public int TeacherId { get; set; }
        //Get and Set TeacherProfile
        public TeacherProfile TeacherProfile { get; set; }
        //Get list RoomMessage
        public ICollection<RoomMessage> RoomMessages { get; set; }
        //Get list RoomDeadLine
        public ICollection<RoomDeadLine> DeadLines { get; set; }
        //Get list GroupChat
        public ICollection<GroupChat> GroupChats { get; set; }

    }
}
