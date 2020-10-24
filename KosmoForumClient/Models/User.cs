using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models
{
    public class User
    {

        [Required(ErrorMessage = "Login jest wymagany")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; }

        [Required(ErrorMessage="E-mail jest wymagany")]
        [EmailAddress(ErrorMessage = "Zły format adresu e-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Role { get; set; }

        public byte[] Avatar { get; set; }

        public string Token { get; set; }
    }
}
