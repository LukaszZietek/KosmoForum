using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models
{
    public class Opinion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Opinion isn't long enough")]
        [DisplayName("Komentarz")]
        public string Content { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Data utworzenia")]
        public DateTime CreationDateTime { get; set; }

        public enum MarksType { Bad, Unsatisfactory, Sufficient, Good, VeryGood }

        [DisplayName("Ocena")]
        public MarksType Marks { get; set; }

        [Required]
        public int ForumPostId { get; set; }

        [ForeignKey("ForumPostId")]
        [DisplayName("Post")]
        public virtual ForumPost ForumPost { get; set; }

        [ForeignKey("UserId")]
        [DisplayName("Użytkownik")]
        public virtual User User { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
