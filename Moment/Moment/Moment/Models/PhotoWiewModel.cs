using Microsoft.AspNetCore.Http;

namespace Moment.Models
{
	public class PhotoViewModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public IFormFile PhotoFile { get; set; }
        public int Id { get; set; }
       
        public DateTime UploadDate { get; set; }
        public List<Photo> recentPhotos { get; set; }
    }
}
