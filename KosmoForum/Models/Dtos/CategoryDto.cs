using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForum.Models.Dtos
{
    public class CategoryDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }
    }
}
