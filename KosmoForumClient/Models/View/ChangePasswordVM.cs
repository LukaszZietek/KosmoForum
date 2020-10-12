using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models.View
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Podanie dotychczasowego hasła jest wymagane")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Podanie nowego hasła jest wymagane")]
        public string NewPassword { get; set; }
    }
}
