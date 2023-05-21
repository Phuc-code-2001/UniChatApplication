using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("student_profile")]
    public class StudentProfile : Profile
    {

        //Set default Avatar
        public StudentProfile()
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
        //Major Property
        public string Major { get; set; }

        //Birthday Property
        public DateTime Birthday { get; set; }
        //StudentCode Property
        [Column("student_code")]
        public string StudentCode { get; set; }

        //AccountID Property
        [Column("account_id")]
        [ForeignKey("Account")]
        public override int AccountID { get; set; }
        //Get and Set Account
        public override Account Account { get; set; }
        //ClassID Property
        [Column("class_id")]
        [ForeignKey("Class")]
        public int? ClassID { get; set; }
        //Get and Set Class
        public Class Class { get; set; }
        //Get getClassName
        public string getClassName() => this.Class?.Name;
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
        //Get list GroupManage
        [InverseProperty("StudentProfile")]
        public ICollection<GroupManage> GroupManages { get; set; }


    }
}
