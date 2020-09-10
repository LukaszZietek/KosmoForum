using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class ForumPostUpdateDto
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MinLength(3, ErrorMessage = "Nazwa postu jest za krótka")]
        [MaxLength(256, ErrorMessage = "Nazwa postu jest za długa")]
        public string Title { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Za krótki opis zabiegu")]
        public string Content { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<Image> Images { get; set; }

    }
}
