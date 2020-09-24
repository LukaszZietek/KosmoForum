using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models.View
{
    public class ForumPostDetailsVM
    {
        public Category category { get; set; }

        public ForumPost forumPost { get; set; }
    }
}
