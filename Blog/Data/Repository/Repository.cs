using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Blog.ViewModels;

namespace Blog.Data.Repository
{
    public class Repository : IRepository
    {
        private AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
        }
        public List<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }
        public IndexPageModel GetAllPosts(int pageNumber, string search)
        {
            //Pagination
            int pageSize = 3;
            int skip = pageSize * (pageNumber - 1);
            int postsCount = _context.Posts.Count();
            /* int cap = skip + pageSize;*/

            var query = _context.Posts.AsQueryable();

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Title.Contains(search) || x.Body.Contains(search));
            }

            return new IndexPageModel
            {
                PageNumber = pageNumber,
                
                PageCount = (int)Math.Ceiling(postsCount * 1.0 / pageSize),
                NextPage = postsCount > skip + pageSize,
                Posts = _context.Posts
                .Skip(skip)
                .Take(pageSize)
                .ToList()
            };
        }

        public Post GetPost(int id)
        {
            return _context.Posts.FirstOrDefault(post => post.Id == id);
        }

        public void RemovePost(int id)
        {
            _context.Posts.Remove(GetPost(id));
        }

        public void UpdatePost(Post post)
        {
            _context.Posts.Update(post);
        }
        public async Task<bool> SaveChanges()
        {
            if(_context.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
