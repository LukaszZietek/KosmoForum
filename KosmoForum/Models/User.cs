using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KosmoForum.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public string Role { get; set; }

        public byte[] Avatar { get; set; }

        public virtual List<Image> Images { get; set; }
        public virtual List<ForumPost> ForumPosts { get; set; }
        public virtual List<Opinion> Opinions { get; set; }

        [NotMapped]
        public string Token { get; set; }
    }
}
