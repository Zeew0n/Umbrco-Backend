using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class ForumFollowerMailModel
    {
        public int ForumId { get; set; }
        public string FromEmail { get; set; }
        public string Message { get; set; }
        public string MailSubject { get; set; }
        public string Bcc { get; set; }
        public string TransType { get; set; }
        public string AdminEmail { get; set; }
        public int DiscussionId { get; set; }
    }
}