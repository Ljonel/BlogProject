using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;

namespace Blog.ViewModels
{
    public class IndexPageModel
    {
        public IEnumerable<Post> Posts {get; set;}
        public int PageNumber { get; set; }
        public bool NextPage { get; set; }
        public int PageCount { get; set; }
    }
}
