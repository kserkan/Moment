using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moment.Data;
using Moment.Models;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moment.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhotoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile photo, string title, string description)
        {
            try
            {
                if (photo == null || photo.Length == 0)
                {
                    ModelState.AddModelError("", "Please select a photo to upload.");
                    return RedirectToAction("Index", "Home");
                }

                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    var newPhoto = new Photo
                    {
                        UserId = userId,
                        Title = title,
                        Description = description,
                        ImageData = memoryStream.ToArray(),
                        ContentType = photo.ContentType,
                        UploadDate = DateTime.Now
                    };

                    _context.Photos.Add(newPhoto);
                    await _context.SaveChangesAsync();
//Notifications
                    var followerIds = _context.Followers
                        .Where(f => f.FollowedId == userId)  // followedId = bu kullanıcı
                        .Select(f => f.FollowerId)
                        .ToList();

                    // 2) Mesajı hazırlayın
                    var message = $"Kullanıcı {userId}, yeni bir fotoğraf yükledi: '{title}'";

                    // 3) Her takipçiye notification kaydı ekleyin
                    foreach (var fid in followerIds)
                    {
                        var notification = new Notification
                        {
                            UserId = fid,
                            Message = message,
                            CreatedAt = DateTime.Now,
                            IsRead = false
                        };
                        _context.Notifications.Add(notification);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpGet]
        public IActionResult GetPhoto(int id)
        {
            var photo = _context.Photos.FirstOrDefault(p => p.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return File(photo.ImageData, photo.ContentType); // Binary veriyi döndür
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Veritabanından fotoğrafı getir
                var photo = _context.Photos.FirstOrDefault(p => p.Id == id);
                if (photo == null)
                {
                    return NotFound(); // Fotoğraf bulunamazsa 404 döndür
                }

                // Fotoğrafı veritabanından sil
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting the photo: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(int photoId, string content)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return Json(new { success = false, message = "Comment cannot be empty." });
            }

            var photo = _context.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo == null)
            {
                return Json(new { success = false, message = "Photo not found." });
            }

            var comment = new Comment
            {
                UserId = userId,
                PhotoId = photoId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Json(new { success = true, message = "Comment added." });
        }



        [HttpPost]
        public IActionResult LikePhoto(int photoId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            var existingLike = _context.Likes.FirstOrDefault(l => l.UserId == userId && l.PhotoId == photoId);
            if (existingLike == null)
            {
                _context.Likes.Add(new Like { UserId = userId, PhotoId = photoId, CreatedAt = DateTime.Now });
                _context.SaveChanges();
                return Json(new { success = true, message = "Liked", newLikeCount = _context.Likes.Count(l => l.PhotoId == photoId) });
            }
            else
            {
                _context.Likes.Remove(existingLike);
                _context.SaveChanges();
                return Json(new { success = true, message = "Unliked", newLikeCount = _context.Likes.Count(l => l.PhotoId == photoId) });
            }
        }









    }


}

