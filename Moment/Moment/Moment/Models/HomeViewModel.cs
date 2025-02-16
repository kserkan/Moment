using System.Collections.Generic;
using Moment.ViewModels; // ✅ Eğer `PhotoViewModel` burada tanımlıysa

namespace Moment.Models
{
    public class HomeViewModel
    {
        public List<Photo> Photos { get; set; }

        public User CurrentUser { get; set; }
        public List<PhotoViewModel> RecentPhotos { get; set; }

        public List<Notification> Notifications { get; set; }

        public int CurrentPage { get; set; } // Mevcut Sayfa
        public int TotalPages { get; set; }  // Toplam Sayfa

    }
}

