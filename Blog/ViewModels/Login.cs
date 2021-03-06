using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.ViewModels
{
    public class Login
    {
        [Required]
        [DataType(DataType.Text)]
        [MinLength(3, ErrorMessage = "The UserName must be more than 3!")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
