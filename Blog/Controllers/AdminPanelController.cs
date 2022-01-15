using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public AdminPanelController(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
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
                    CurrentImage = post.Image,//pobieram dane z posta ktory zostal klikniety
                    Created = post.Created,
                    ApplicationUserName = post.ApplicationUserName,
                    ApplicationUserId = post.ApplicationUserId,

                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel postViewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var userName = User.Identity.Name;

            var post = new Post();

            if (postViewModel.Id > 0)
            {

                 post = new Post
                {
                    Id = postViewModel.Id,
                    Title = postViewModel.Title,
                    Body = postViewModel.Body,
                    Created = postViewModel.Created,
                    ApplicationUserId = postViewModel.ApplicationUserId,
                    ApplicationUserName = postViewModel.ApplicationUserName,
                };
                /*postViewModel.ApplicationUserName = post.ApplicationUserName;*/
                _repository.UpdatePost(post);           //jesli edytujemy, id zawsze jest > 0
            }
            else
            {
                post = new Post
                {
                    Id = postViewModel.Id,
                    Title = postViewModel.Title,
                    Body = postViewModel.Body,
                    ApplicationUserName = userName,
                    ApplicationUserId = userId,
                };


                postViewModel.ApplicationUserId = userId;
                postViewModel.ApplicationUserName = userName;
                _repository.AddPost(post);      //jesli klikniete bedzie "Create Post" ukryte id=0,
            }


           
            if(postViewModel.Image == null)
            {
                post.Image = postViewModel.CurrentImage;
            }
            else
            {
                post.Image = await _fileManager.SaveImage(postViewModel.Image);
            }

           
            if (await _repository.SaveChanges())
                return RedirectToAction("Index", "AdminPanel");
            else
                return View(post);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            _repository.RemovePost(id);
            _repository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
