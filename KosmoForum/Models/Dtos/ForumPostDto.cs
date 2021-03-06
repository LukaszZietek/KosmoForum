﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class ForumPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }
        
        public int UserId { get; set; }

        public UserDto User { get; set; }

        public int CategoryId { get; set; }

        public  List<OpinionDto> Opinions { get; set; }
        public  List<ImageDto> Images { get; set; }


    }
}
