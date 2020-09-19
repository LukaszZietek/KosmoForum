using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class ImageUpdateDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] Picture { get; set; }


    }
}
