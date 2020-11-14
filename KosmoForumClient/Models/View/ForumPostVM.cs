using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KosmoForumClient.Models.View
{
    public class ForumPostVM
    {
        [DisplayName("Lista kategorii")]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [DisplayName("Post")]
        public ForumPost ForumPostModel { get; set; }
    }
}
