using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace UniChatApplication.Models
{
    [Table("subject")]
    public class Subject
    {
        //ID Property
        [Key]
        public int Id { get; set; }
        //SubjectCode Property
        [Column("code")]
        [Required(ErrorMessage = "Please enter Subject Code!")]
        [MaxLength(10)]
        public string SubjectCode { get; set; }
        //FullName Property
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        //Get list RoomChat
        [InverseProperty("Subject")]
        public ICollection<RoomChat> RoomChats { get; set; }


    }
}