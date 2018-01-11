using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class ForumFollowersModel
    {
        public int ID { get; set; }
        public int ForumId { get; set; }
        public int UserId { get; set; }
        public DateTime FollowedDate { get; set; }
        public bool Follow { get; set; }
        public string FeedbackMsg { get; set; }
        public string PostAction { get; set; }
    }
}