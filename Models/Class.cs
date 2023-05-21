using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace UniChatApplication.Models
{
    [Table("class")]
    public class Class
    {
        //ID Property
        [Key]
        public int Id { get; set; }
        //Name Property
        public string Name { get; set; }
        //Get list StudentProfile
        [InverseProperty("Class")]
        public ICollection<StudentProfile> StudentProfiles { get; set; }
        //Get list RoomChat
        [InverseProperty("Class")]
        public ICollection<RoomChat> RoomChats { get; set; }

    }
}
