using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KosmoForum.Models
{
    public class ForumPost
    {

        //public ForumPost()
        //{
        //    this.Opinions = new HashSet<Opinion>();
        //    this.Images = new HashSet<Image>();
            
        //}
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


        public virtual ICollection<Opinion> Opinions { get; set; }

        public virtual ICollection<Image> Images { get; set; }


    }
}
