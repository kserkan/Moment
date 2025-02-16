namespace Moment.Models
{
    public class Notification
    {
        public int Id { get; set; }

        // Bildirimi kime göndereceğiz? (hedef kullanıcı)
        public int UserId { get; set; }
        public User User { get; set; }

        // Gösterilecek mesaj/olay
        public string Message { get; set; } = string.Empty; // NULL yerine boş string

        // Kaydedildiği anki zaman
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Kullanıcı bu bildirimi okudu mu?
        public bool IsRead { get; set; } = false;
    }
}
