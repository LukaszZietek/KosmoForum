using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KosmoForumClient.Models.View
{
    public class ForumPostVM
    {
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public ForumPost ForumPostModel { get; set; }
    }
}
