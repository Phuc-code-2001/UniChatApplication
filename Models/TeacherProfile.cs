using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace UniChatApplication.Models
{
    [Table("teacher_profile")]
    public class TeacherProfile : Profile
    {

        //Set default Avatar
        public TeacherProfile()
        {
            this.Avatar = Profile.defaultAvatar;
        }

        //ID Property
        [Key]
        public override int Id { get; set; }
        //FullName Property
        public override string FullName { get; set; }
        //Avatar Property
        public override string Avatar { get; set; }
        //Email Property
        public override string Email { get; set; }
        //Phone Property
        public override string Phone { get; set; }
        //Gender Property
        public override bool Gender { get; set; }
        //Birthday Property
        public DateTime Birthday { get; set; }
        //TeacherCode Property
        [Column("teacher_code")]
        public string TeacherCode { get; set; }
        //AccountID Property
        [Column("account_id")]
        [ForeignKey("Account")]
        public override int AccountID { get; set; }
        //Get and Set Account
        public override Account Account { get; set; }

        //Get GenderText
        [NotMapped]
        public string GenderText
        {
            get
            {
                if (this.Gender) return "Male";
                if (!this.Gender) return "Female";
                return "Unknown";
            }
        }
        //Get list RoomChat
        [InverseProperty("TeacherProfile")]
        public ICollection<RoomChat> RoomChats { get; set; }


    }
}
