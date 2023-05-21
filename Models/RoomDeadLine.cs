

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("room_dealine")]
    public class RoomDeadLine
    {
        //ID Property
        [Key]
        public int Id { get; set; }
        //RoomId Property
        [Column("room_id")]
        [ForeignKey("RoomChat")]
        public int RoomId { get; set; }
        //Get and Set RoomChat
        public RoomChat RoomChat { get; set; }
        //Content Property
        [MaxLength(500)]
        public string Content { get; set; }
        //ExpirationTime Property
        [Column("expiration_time")]
        public DateTime ExpirationTime { get; set; }

    }

}