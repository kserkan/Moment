using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moment.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public byte[] ImageData { get; set; } // Fotoğraf binary olarak saklanıyor

        public string ContentType { get; set; } // MIME türü

        public DateTime UploadDate { get; set; } = DateTime.Now; // Yükleme tarihi

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } // Kullanıcı ile ilişki
    }
}
