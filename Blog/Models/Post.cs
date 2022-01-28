
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Blog.Models
{
    public class Post       //view is for database, viewmodels are for controller
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
        public string ApplicationUserId { get; set; }
        public string ApplicationUserName{ get; set; }
        public string Image { get; set; }
        

    }
}
