using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("group_message")]
    public class GroupMessage
    { //ID Proterty
        [Key]
        public int Id { get; set; }
        //AccountId Proterty
        [Column("account_id")]
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        //Get and Set Account
        public Account Account { get; set; }
        //GroupId Property
        [Column("group_id")]
        [ForeignKey("GroupChat")]
        public int? GroupId { get; set; }
        //Get and Set GroupChat
        public GroupChat GroupChat { get; set; }
        //Content Property
        [MaxLength(500)]
        public string Content { get; set; }
        //TimeMessage Property
        [Column("time_message")]
        public DateTime TimeMessage { get; set; }
        //Get list GroupPinMessage
        [InverseProperty("GroupMessage")]
        public ICollection<GroupPinMessage> AllPinMessage { get; set; }
    }
}