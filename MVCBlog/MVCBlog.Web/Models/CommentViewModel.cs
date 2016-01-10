using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Web.Models
{
    public class CommentViewModel
    {
        [Required]
        public int PostID { get; set; }
        [Required]
        public string CommentTitle { get; set; }
        [Required]
        public string CommentContent { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }

    }
}
