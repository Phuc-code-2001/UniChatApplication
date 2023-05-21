using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{

    [Table("room_message")]
    public class RoomMessage
    {

        //ID Property
        [Key]
        public int Id { get; set; }

        //AccountID Property
        [Column("account_id")]
        [ForeignKey("Account")]
        public int AccountID { get; set; }
        //Get and Set Account
        public Account Account { get; set; }
        //RoomID Property
        [Column("room_id")]
        [ForeignKey("RoomChat")]
        public int? RoomID { get; set; }
        //Get and Set RoomChat
        [ForeignKey("RoomID")]
        public RoomChat RoomChat { get; set; }
        //Content Property
        public string Content { get; set; }
        //TimeMessage Property
        [Column("time_message")]
        public DateTime TimeMessage { get; set; }

    }

}