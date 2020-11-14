using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KosmoForumClient.Models
{
    public class ForumPost
    {
        public ForumPost()
        {
            Images = new List<Image>();
            Opinions = new List<Opinion>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Title isn't enough long")]
        [MaxLength(256, ErrorMessage = "Title is too long")]
        [DisplayName("Tytuł")]
        public string Title { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Content isn't enough long")]
        [DisplayName("Opis")]
        public string Content { get; set; }

        [DisplayName("Data utworzenia")]
        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        [DisplayName("Użytkownik")]
        public virtual User User { get; set; }
        [Required]
        public int UserId { get; set; }



        [ForeignKey("CategoryId")]
        [DisplayName("Kategoria")]
        public virtual Category Category { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [DisplayName("Opinie")]
        public virtual List<Opinion> Opinions { get; set; }

        [DisplayName("Zdjęcia")]
        public virtual List<Image> Images { get; set; }
    }
}
