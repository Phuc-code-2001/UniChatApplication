using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("account")]
    public class Account
    {

        //ID Property
        [Key]
        public int Id { get; set; }
        //UserName Property
        [Required]
        public string Username { get; set; }
        //PassWord Property
        public string Password { get; set; }
        //RoleID Property
        [Column("role_id")]
        public int RoleID { get; set; }

        // Get RoleName
        [NotMapped]
        public string RoleName
        {
            get
            {
                if (RoleID == 1) return "Student";
                if (RoleID == 2) return "Teacher";
                if (RoleID == 3) return "Admin";
                return "Unknown";
            }
        }

        //Get and Set StudentProfile
        public StudentProfile StudentProfile { get; set; }
        //Get and Set TeacherProfile
        public TeacherProfile TeacherProfile { get; set; }
        //Get and Set AdminProfile
        public AdminProfile AdminProfile { get; set; }
        //Get list RoomMessages
        [InverseProperty("Account")]
        public ICollection<RoomMessage> RoomMessages { get; set; }
        //Get list GroupMessage
        [InverseProperty("Account")]
        public ICollection<GroupMessage> GroupMessages { get; set; }

    }
}
