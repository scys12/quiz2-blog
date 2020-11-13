using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace blog.Models.POCO
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public virtual Post Post { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
    }
}