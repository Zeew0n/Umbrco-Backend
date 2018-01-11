using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uSome
{
    public class FollowedItemModel
    {
        public int ID { get; set; }
        public int NodeId { get; set; }
        public int UserId { get; set; }
        public DateTime FollowedDate { get; set; }
        public bool IsFollow { get; set; }
        public string Type { get; set; }
    }
}
