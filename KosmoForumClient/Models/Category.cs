using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Category name is too long...")]
        [MinLength(2, ErrorMessage = "Category name doesn't have enough char")]
        [DisplayName("Tytuł")]
        public string Title { get; set; }
        [DisplayName("Opis")]
        public string Description { get; set; }
        [DisplayName("Logo")]
        public byte[] Image { get; set; }

        [DisplayName("Data utworzenia")]
        public DateTime CreationDateTime { get; set; }

        [DisplayName("Posty")]
        public virtual List<ForumPost> ForumPosts { get; set; }
    }
}
