using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models.View
{
    public class ForumPostDetailsVM
    {
        [DisplayName("Kategoria")]
        public Category category { get; set; }

        [DisplayName("Post")]
        public ForumPost forumPost { get; set; }
    }
}
