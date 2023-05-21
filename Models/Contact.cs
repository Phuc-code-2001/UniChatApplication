using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("contact")]
    public class Contact
    {
        //contactId Property
        [Key]
        [Display(Name = "contactId")]
        [Required(ErrorMessage = "Contact Id cannot  be blank")]
        public int contactId { get; set; }
        //contactTitle Property
        [Display(Name = "contactTitle")]
        [Required(ErrorMessage = "Contact Title cannot  be blank")]
        public string contactTitle { get; set; }
        //contactMes Property
        [Display(Name = "contactMes")]
        [Required(ErrorMessage = "Contact Mes cannot  be blank")]
        public string contactMes { get; set; }
        //contactProgress Property
        [Display(Name = "contactProgress")]
        [Required(ErrorMessage = "Contact Progress cannot  be blank")]
        public bool contactProgress { get; set; }
        //contactCustomerPhone Property
        [Display(Name = "contactCustomerPhone")]
        [Required(ErrorMessage = "Contact Customer Phone cannot  be blank")]
        public string contactCustomerPhone { get; set; }

        //contactCustomerPhone Property
        [Display(Name = "contactCustomerEmail")]
        [Required(ErrorMessage = "Contact Customer Email cannot  be blank")]
        public string contactCustomerEmail { get; set; }
    }
}
