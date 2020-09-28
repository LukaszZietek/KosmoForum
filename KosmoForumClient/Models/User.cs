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

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Bad format of e-mail address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Role { get; set; }

        public byte[] Avatar { get; set; }

        public string Token { get; set; }
    }
}
