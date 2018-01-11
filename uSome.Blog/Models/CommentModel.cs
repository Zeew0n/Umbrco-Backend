using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class CommentModel
    {

        public int Id { get; set; }

        public int ContentId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }

        public bool IsPublic { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}