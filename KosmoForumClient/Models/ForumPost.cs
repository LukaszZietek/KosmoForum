using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models
{
    public class ForumPost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Title isn't enough long")]
        [MaxLength(256, ErrorMessage = "Title is too long")]
        public string Title { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Content isn't enough long")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Required]
        public int UserId { get; set; }



        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        public int CategoryId { get; set; }


        public virtual List<Opinion> Opinions { get; set; }

        public virtual List<Image> Images { get; set; }
    }
}
