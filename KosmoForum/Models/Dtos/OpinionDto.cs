﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class OpinionDto
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public DateTime CreationDateTime { get; set; }


        public Opinion.MarksType Marks { get; set; }

        public int UserId { get; set; }

        public UserDto User { get; set; }

        public int ForumPostId { get; set; }
    }
}
