using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KosmoForumClient.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Zdjęcie")]
        public byte[] Picture { get; set; }

        [DisplayName("Data dodania")]
        public DateTime AddTime { get; set; }


        [ForeignKey("ForumPostId")]
        [DisplayName("Post")]
        public virtual ForumPost ForumPost { get; set; }

        public int ForumPostId { get; set; }

        [ForeignKey("UserId")]
        [DisplayName("Użytkownik")]
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
