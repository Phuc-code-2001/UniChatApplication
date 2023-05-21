

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("room_marked_message")]
    public class RoomMessagePin
    {
        //ID Property
        [Key]
        public int Id { get; set; }
        //RoomMessageId Property
        [Column("room_message_id")]
        public int RoomMessageId { get; set; }
        //RoomMessage Property
        [ForeignKey("RoomMessageId")]
        public RoomMessage RoomMessage { get; set; }
        //Time Property
        [Column("time_marked")]
        public DateTime Time { get; set; }
    }

}