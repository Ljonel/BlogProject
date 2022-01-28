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
using Blog.Data.FileManager;
using Blog.ViewModels;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public HomeController(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }
        public IActionResult Index(int pageNumber, string search)
        {
            if(pageNumber < 1)
                return RedirectToAction("Index", new { pageNumber = 1});


            var vm = _repository.GetAllPosts(pageNumber, search);

            /*var posts = _repository.GetAllPosts(pageNumber);*/
            return View(vm);
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
                return View(new PostViewModel());
            }
            else
            {
                var post = _repository.GetPost((int)id); //this is short version of converting to int
                return View(new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    ApplicationUserName = post.ApplicationUserName,
                });

            }



           /* if (id == null)
            {
                return View(new Post());
            }
            else
            {
                var post = _repository.GetPost((int)id); //this is short version of converting to int
                return View(post);

            }*/
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel postViewModel)
        {
            if (postViewModel.Title == null )
            {
                ModelState.AddModelError(nameof(postViewModel.Title), "Title not found or null");
                return View();
            }
            if(postViewModel.Body == null)
            {
                ModelState.AddModelError(nameof(postViewModel.Body), "Body not found or null");
                return View();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var userName = User.Identity.Name;
            var post = new Post
            {
                Id = postViewModel.Id,
                Title = postViewModel.Title,
                Body = postViewModel.Body,
                Image = await _fileManager.SaveImage(postViewModel.Image),
                ApplicationUserName = userName,
                ApplicationUserId = userId,
            };

            if (post.Id > 0)
            {
                _repository.UpdatePost(post);           //jesli edytujemy id zawsze jest > 0
            }
            else
            {
                postViewModel.ApplicationUserId = post.ApplicationUserId;
                postViewModel.ApplicationUserName = post.ApplicationUserName;
                _repository.AddPost(post);      //jesli klikniete bedzie "Create Post" ukryte id=0,
            }
            if (await _repository.SaveChanges())
                return RedirectToAction("Index", "Home");
            else
                return View(post);




            /*
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
                return View(post);*/
        }



        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }
    }
}
