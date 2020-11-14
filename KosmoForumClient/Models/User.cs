using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models
{
    public class User
    {

        [Required(ErrorMessage = "Login jest wymagany")]
        [DisplayName("Nazwa użytkownika")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DisplayName("Hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage="E-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Zły format adresu e-mail")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [DisplayName("Data dołączenia")]
        public DateTime JoinDateTime { get; set; }

        [DisplayName("Rola")]
        public string Role { get; set; }

        public byte[] Avatar { get; set; }

        public string Token { get; set; }
    }
}
