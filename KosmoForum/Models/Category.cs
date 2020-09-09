using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256,ErrorMessage = "Za długa nazwa kategorii")]
        [MinLength(2,ErrorMessage = "Za krótka nazwa kategorii")]
        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public DateTime CreationDateTime { get; set; }

        public virtual List<ForumPost> ForumPosts { get; set; }



    }

 
}
