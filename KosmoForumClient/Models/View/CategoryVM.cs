using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models.View
{
    public class CategoryVM
    {
        [DisplayName("Lista kategorii")]
        public IEnumerable<Category> CategoriesList { get; set; }
    }
}
