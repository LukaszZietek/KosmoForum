using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KosmoForum.Models
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
        public User User { get; set; }
        [Required]
        public int UserId { get; set; }



        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public int CategoryId { get; set; }


        public ICollection<Opinion> Opinions { get; set; }

        public ICollection<Image> Images { get; set; }


    }
}
