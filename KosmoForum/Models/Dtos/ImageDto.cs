﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class ImageDto
    {
        public int Id { get; set; }
        public byte[] Picture { get; set; }
    }
}
