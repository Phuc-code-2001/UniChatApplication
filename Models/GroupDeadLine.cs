
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("group_dealine")]
    public class GroupDeadLine
    {
        //ID Property
        [Key]
        public int Id { get; set; }
        //GroupId Property
        [ForeignKey("GroupChat")]
        public int GroupId { get; set; }
        //GroupChat Property
        public GroupChat GroupChat { get; set; }
        //GroupChat Content
        [MaxLength(500)]
        [Required]
        public string Content { get; set; }
        //ExpirationTime Property
        [Column("expiration_time")]
        public DateTime ExpirationTime { get; set; }
    }
}