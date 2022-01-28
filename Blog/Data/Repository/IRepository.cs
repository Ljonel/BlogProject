using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Blog.ViewModels;

namespace Blog.Data.Repository
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        IndexPageModel GetAllPosts(int pageNumber, string search);
        void AddPost(Post post);
        void RemovePost(int id);
        void UpdatePost(Post post);
        Task<bool> SaveChanges();
    }
}
