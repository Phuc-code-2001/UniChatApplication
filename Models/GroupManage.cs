

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniChatApplication.Data;

namespace UniChatApplication.Models
{
    [Table("group_manage")]
    public class GroupManage
    {
        //ID Property
        [Key]
        public int Id { get; set; } = 0;
        //StudentId Property
        [Column("student_id")]
        [ForeignKey("StudentProfile")]
        public int? StudentId { get; set; }
        //Get and Set StudentProfile
        public StudentProfile StudentProfile { get; set; }
        //GroupId Property
        [Column("group_id")]
        [ForeignKey("GroupChat")]
        public int GroupId { get; set; }
        //Get and Set GroupChat
        public GroupChat GroupChat { get; set; }
        //Role Property
        [Column("role")]
        public bool Role { get; set; } = false;
        //Get RoleText
        [NotMapped]
        public string RoleText
        {
            get
            {
                if (Role) return "Leader";
                else return "Member";
            }
        }


    }

}