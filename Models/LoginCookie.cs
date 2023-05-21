using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniChatApplication.Models
{
    [Table("login_cookie")]
    public class LoginCookie
    {
        //ID Property
        [Key]
        public int Id { get; set; }
        //Key Property
        [Column("login_key")]
        public string Key { get; set; }
        //AccountID Property
        [Column("account_id")]
        public int AccountID { get; set; }
        //ExpirationTime Property
        [Column("expiration_time")]
        public DateTime ExpirationTime { get; set; }

    }
}
