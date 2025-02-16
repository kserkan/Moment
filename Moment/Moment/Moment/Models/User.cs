using System;
using System.ComponentModel.DataAnnotations;

namespace Moment.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } // 🔴 Şifre için doğru alan ismi

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Kullanıcının kayıt tarihi

        // Navigation properties
        public ICollection<Follower>? Followers { get; set; } // Kullanıcının takipçileri
        public ICollection<Follower>? Following { get; set; } // Kullanıcının takip ettikleri
    
}
}
