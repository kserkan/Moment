using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moment.Data;
using Moment.Models;
using Moment.ViewModels;
using System.Security.Claims;

[Authorize] // 🔴 Giriş yapmayanlar için yetkilendirme
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(int page = 1)
    {
        int pageSize = 6; // Her sayfada kaç fotoğraf gösterilecek
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int? userId = null;
        if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int uid))
            userId = uid;

        var totalPhotos = _context.Photos.Count(); // Toplam fotoğraf sayısı
        var totalPages = (int)Math.Ceiling((double)totalPhotos / pageSize); // Kaç sayfa olacak

        var recentPhotos = _context.Photos
            .OrderByDescending(p => p.UploadDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PhotoViewModel
            {
                Id = p.Id,
                Title = p.Title,
                UploadDate = p.UploadDate
            })
            .ToList();

        // Kullanıcının bildirimleri (varsa)
        List<Notification> userNotifications = new List<Notification>();
        if (userId.HasValue)
        {
            userNotifications = _context.Notifications
                .Where(n => n.UserId == userId.Value)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToList();
        }

        var model = new HomeViewModel
        {
            RecentPhotos = recentPhotos,
            Notifications = userNotifications,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(model);
    }










    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction("Login", "Account"); // 🔴 Çıkış sonrası Login sayfasına git
    }
    [HttpGet]
    public IActionResult Profile()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return RedirectToAction("Login");
        }

        int userId = int.Parse(userIdClaim);
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return NotFound();
        }

        // Takip edilebilecek kullanıcılar sorgusu (örnek):
        var followableUsers = _context.Users
            .Where(u => u.Id != userId &&
                   !_context.Followers.Any(f => f.FollowerId == userId && f.FollowedId == u.Id))
            .ToList();

        // Takip ettiğiniz kullanıcıları ("Following") sorguluyoruz:
        var followingList = _context.Followers
            .Where(f => f.FollowerId == userId)    // Bu kullanıcı hangi kişileri takip ediyor?
            .Select(f => f.Followed)              // Followed, 'Followers' tablosunda .FollowedId ile ilişkilendirilmiş User'ı temsil ediyor
            .ToList();

        var viewModel = new ProfileViewModel
        {
            CurrentUser = user,
            FollowableUsers = followableUsers,
            Following = followingList
        };

        return View("Profile", viewModel);
    }




    [HttpPost]
    public IActionResult AddToFavorites(int photoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Zaten favorilerde mi kontrol et
        var existingFavorite = _context.Favorites.FirstOrDefault(f => f.UserId == userId && f.PhotoId == photoId);
        if (existingFavorite == null)
        {
            var favorite = new Favorite
            {
                UserId = userId,
                PhotoId = photoId
            };
            _context.Favorites.Add(favorite);
            _context.SaveChanges();
        }

        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public IActionResult Favorites()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var favoritePhotos = _context.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.Photo)
            .ToList();

        return View(favoritePhotos);
    }




    [HttpPost]
    public IActionResult RemoveFromFavorites(int photoId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var favorite = _context.Favorites.FirstOrDefault(f => f.UserId == userId && f.PhotoId == photoId);
        if (favorite != null)
        {
            _context.Favorites.Remove(favorite);
            _context.SaveChanges();
        }

        return RedirectToAction("Favorites", "Home");
    }
    [HttpPost]
    public IActionResult Follow(int followedId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return RedirectToAction("Login");
        }

        // Takip ekleme işlemi
        var newFollow = new Follower
        {
            FollowerId = userId,
            FollowedId = followedId
        };
        _context.Followers.Add(newFollow);
        _context.SaveChanges();

        // Profile sayfasına geri dön
        return RedirectToAction("Profile");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Unfollow(int followedId)
    {
        var followerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(followerIdClaim, out int followerId))
        {
            return RedirectToAction("Login", "Account");
        }

        var existingFollower = _context.Followers.FirstOrDefault(f => f.FollowerId == followerId && f.FollowedId == followedId);
        if (existingFollower != null)
        {
            _context.Followers.Remove(existingFollower);
            _context.SaveChanges();
        }

        return RedirectToAction("Index", "Home");
    }


    public IActionResult Feed()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Takip edilebilecek kullanıcıları listele
        var followableUsers = _context.Users
            .Where(u => u.Id != userId && !_context.Followers.Any(f => f.FollowerId == userId && f.FollowedId == u.Id))
            .ToList();

        // Takip edilen kullanıcıların fotoğraflarını listele
        // "p => _context.Followers.Any(...)" yöntemiyle follower kaydı var mı diye bakabilirsiniz.
        var followedPhotos = _context.Photos
         .Include(p => p.User) // ✅ Kullanıcı bilgilerini getir
         .Include(p => p.Comments)
             .ThenInclude(c => c.User) // Yorumu yapan kullanıcı bilgisi
         .Include(p => p.Likes)
             .ThenInclude(l => l.User) // Beğeni yapan kullanıcı bilgisi
         .Where(p => _context.Followers.Any(f => f.FollowerId == userId && f.FollowedId == p.UserId))
         .OrderByDescending(p => p.UploadDate)
         .ToList();

        var model = new FeedViewModel
        {
            FollowableUsers = followableUsers,
            FollowedPhotos = followedPhotos
        };

        return View(model);
    }


    public IActionResult Following()
    {
        var followerId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

        var following = _context.Followers
            .Where(f => f.FollowerId == followerId)
            .Select(f => f.Followed)
            .ToList();

        return View(following);
    }

    public IActionResult Users()
    {
        var users = _context.Users.ToList();
        return View(users);
    }


}
