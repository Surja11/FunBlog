using System.ComponentModel.DataAnnotations;
using Blog.Models;

namespace Blog.ViewModels
{
    public class CreatePostVM
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ApplicatonUserId { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public IFormFile? Thumbnail { get; set; }

    }
}
