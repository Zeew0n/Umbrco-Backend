using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class AnswerModel
    {
        public int ID { get; set; }
        public int ContentId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Comment { get; set; }
       
        public bool IsPublic { get; set; }
        public bool MarkAsAnswer { get; set; }
        public int likeCount { get; set; }
        public string LikeUsers { get; set; }
    }
}