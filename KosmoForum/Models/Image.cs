using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KosmoForum.Models
{
    public class Image
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public byte[] Picture { get; set; }

        public DateTime AddTime { get; set; }


        [ForeignKey("ForumPostId")]
        public virtual ForumPost ForumPost { get; set; }

        [Required]
        public int ForumPostId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public int UserId { get; set; }



    }
}
