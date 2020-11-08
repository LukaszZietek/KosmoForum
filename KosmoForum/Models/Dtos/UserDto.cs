using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class UserDto
    {
        public string Username { get; set; }
        public byte[] Avatar { get; set; }
        public DateTime JoinDateTime { get; set; }
    }
}
