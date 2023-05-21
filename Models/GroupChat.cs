

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("group_chat")]
    public class GroupChat
    {

        //ID Property
        [Key]
        public int Id { get; set; }
        //Name Property
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        //Order Property
        [Column("_index")]
        public int Order { get; set; } = 0;
        //RoomID Property
        [Column("room_id")]
        [ForeignKey("RoomChat")]
        public int RoomID { get; set; }
        //Get and Set RoomChat
        public RoomChat RoomChat { get; set; }
        //Get list GroupMessage
        public ICollection<GroupMessage> Messages { get; set; }
        //Get list GroupDeadLine
        public ICollection<GroupDeadLine> DeadLines { get; set; }
        //Get list GroupManage
        public ICollection<GroupManage> GroupManages { get; set; }


    }
}