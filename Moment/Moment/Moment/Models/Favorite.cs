using System;

namespace Moment.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PhotoId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Photo Photo { get; set; }
        public User User { get; set; }
    }
}
