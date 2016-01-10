﻿using MVCBlog.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCBlog.Web.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "文章标题不能为空")]
        [Display(Name = "文章标题")]
        public string Title { get; set; }


        [Required(ErrorMessage = "文章内容不能为空")]
        [Display(Name = "文章内容")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "分类目录")]
        public int CategoryID { get; set; }

        public virtual UserInfo PostAuthor { get; set; }
    }
}