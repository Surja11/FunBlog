﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Blog.Data;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        public BlogController(ApplicationDbContext context, INotyfService notification)
        {
            _context = context;
            _notification = notification;
        }
        public IActionResult Post(string slug)
        {
            var post = _context.Posts!.Include(x => x.ApplicationUser).FirstOrDefault(x => x.Slug == slug);

            if (post == null)
            {
                _notification.Error("Post not found");
                return View();

            }

            var vm = new BlogPostVM()
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.ApplicationUser.FirstName + " " + post.ApplicationUser.LastName,
                CreatedDate = post.CreatedDate,
                ThumbnailUrl = post.ThumbnailUrl,
                Description = post.Description,
                ShortDescription = post.ShortDescription,
            };
            return View(vm);
        }
    }
}
