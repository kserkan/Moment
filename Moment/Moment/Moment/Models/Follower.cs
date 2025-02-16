using Moment.Models;

public class Follower
{
    public int Id { get; set; } // Follower tablosu için birincil anahtar
    public int FollowerId { get; set; } // Takip eden kullanıcının ID'si
    public int FollowedId { get; set; } // Takip edilen kullanıcının ID'si

    public User Followers { get; set; } // Navigation property for Follower
    public User Followed { get; set; } // Navigation property for Followed
}
