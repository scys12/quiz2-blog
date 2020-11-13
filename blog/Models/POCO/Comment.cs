﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blog.Models.POCO
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public virtual Post Post { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public string Description { get; set; }
    }
}