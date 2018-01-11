using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class MemberListModel
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string ToEmailId { get; set; }
        public Boolean IsConfirmed { get; set; }
        
    }
}