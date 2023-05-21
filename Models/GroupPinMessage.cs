
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("group_marked_message")]
    public class GroupPinMessage
    {

        //ID Property
        [Key]
        public int Id { get; set; }
        //GroupMessageId Property
        [Column("group_message_id")]
        [ForeignKey("GroupMessage")]
        public int GroupMessageId { get; set; }
        //Get and Set GroupMessage
        public GroupMessage GroupMessage { get; set; }
        //Time Property
        [Column("time_marked")]
        public DateTime Time { get; set; }

    }
}