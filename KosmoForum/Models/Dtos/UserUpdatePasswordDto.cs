using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class UserUpdatePasswordDto
    {
        [Required] 
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
