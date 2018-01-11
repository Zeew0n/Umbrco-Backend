using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class BlogModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsPublic { get; set; }

        public bool IsPublished { get; set; }

        public DateTime CreatedOn { get; set; }
        public int ParentId { get; set; }
    }
}