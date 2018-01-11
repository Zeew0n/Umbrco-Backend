using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class MasterBlogModel
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsPublic { get; set; }
        public int Category { get; set; }
        public string Description { get; set; }
    }
}