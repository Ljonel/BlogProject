using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.ViewModels
{
    public class PostViewModel
    {
       public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public string ApplicationUserId { get; set; } = "";
        public string ApplicationUserName { get; set; } = "";
        public string CurrentImage { get; set; } = ""; //this is for when editing post without adding new image

        public IFormFile Image { get; set; }
       

    }
}
