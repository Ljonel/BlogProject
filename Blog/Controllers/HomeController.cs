using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Repository;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;
        public HomeController(IRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var posts = _repository.GetAllPosts();
            return View(posts);
        }
        public IActionResult Post(int id)
        {
            var post = _repository.GetPost(id);
            return View(post);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View(new Post());
            }
            else
            {
                var post = _repository.GetPost((int)id); //this is short version of converting to int
                return View(post);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {

            if (post.Id > 0)
            {
                _repository.UpdatePost(post);           //jesli edytujemy id zawsze jest > 0
            }
            else
            {
                //post.ApplicationUserId = userId;
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
                var userName = User.Identity.Name;
                post.ApplicationUserId = userId;
                post.ApplicationUserName = userName;
                _repository.AddPost(post);      //jesli klikniete bedzie "Create Post" ukryte id=0,
            }
            if (await _repository.SaveChanges())
                return RedirectToAction("Index", "Home");
            else
                return View(post);
        }

    }
}
