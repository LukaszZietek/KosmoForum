using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models
{
    public class Opinion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3,ErrorMessage = "Za krótka opinia")]
        public string Content { get; set; }


        public DateTime CreationDateTime { get; set; }

        public enum MarksType { Bad, Unsatisfactory, Sufficient, Good, VeryGood }

        public MarksType Marks { get; set; }

        [ForeignKey("ForumPostId")]
        public int ForumPostId { get; set; }

        //[ForeignKey("ForumPostId")]
        //public virtual ForumPost ForumPost { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public int UserId { get; set; }


    }
}
