using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{

    abstract public class Profile
    {

        //defaultAvatar Property
        public static string defaultAvatar = "/media/profiles/default.png";
        //ID Property
        public virtual int Id { get; set; }
        //FullName Property
        public virtual string FullName { get; set; }
        //Avatar Property
        public virtual string Avatar { get; set; }
        //Email Property
        public virtual string Email { get; set; }
        //Phone Property
        public virtual string Phone { get; set; }
        //Gender Property
        public virtual bool Gender { get; set; }
        //AccountID Property
        public virtual int AccountID { get; set; }
        //Get and Set Account
        public virtual Account Account { get; set; }

    }
}
