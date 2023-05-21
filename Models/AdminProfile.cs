using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("admin_profile")]
    public class AdminProfile : Profile
    {

        public AdminProfile()
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

    }
}
