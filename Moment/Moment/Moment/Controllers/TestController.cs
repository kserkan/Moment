using Microsoft.AspNetCore.Mvc;
using Moment.Data;

namespace Moment.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Veritabanı bağlantısını test et
        public IActionResult CheckDatabaseConnection()
        {
            try
            {
                if (_context.Database.CanConnect())
                {
                    return Content("Veritabanı bağlantısı başarılı.");
                }
                else
                {
                    return Content("Veritabanına bağlanılamadı.");
                }
            }
            catch (Exception ex)
            {
                return Content($"Hata: {ex.Message}");
            }
        }
    }
}
