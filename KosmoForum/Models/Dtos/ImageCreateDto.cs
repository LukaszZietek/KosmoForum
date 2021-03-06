﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class ImageCreateDto
    {
        [Required]
        public byte[] Picture { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
