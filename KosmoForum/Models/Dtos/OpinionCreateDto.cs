using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class OpinionCreateDto
    {

        [Required]
        [MinLength(3, ErrorMessage = "Za krótka opinia")]
        public string Content { get; set; }

        [Range(0,4,ErrorMessage = "Value have to be between 0 and 4")]
        public Opinion.MarksType Marks { get; set; }

        [Required]
        public int ForumPostId { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}
