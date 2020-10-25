using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models.View
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Podanie dotychczasowego hasła jest wymagane")]
        [DisplayName("Stare hasło")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Podanie nowego hasła jest wymagane")]
        [DisplayName("Nowe hasło")]
        public string NewPassword { get; set; }
    }
}
