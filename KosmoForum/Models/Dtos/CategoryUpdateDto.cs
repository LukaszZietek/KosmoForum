using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class CategoryUpdateDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256, ErrorMessage = "Category name is too long...")]
        [MinLength(2, ErrorMessage = "Category name doesn't have enough char")]
        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public DateTime CreationDateTime { get; set; }
    }
}
