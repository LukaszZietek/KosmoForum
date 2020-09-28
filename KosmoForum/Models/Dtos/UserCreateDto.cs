using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models
{
    public class UserCreateDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Bad format of e-mail address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public DateTime JoinDateTime { get; set; }

        public byte[] Avatar { get; set; }
    }
}
