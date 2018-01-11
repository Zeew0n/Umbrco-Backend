using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class PostModel
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }

        public string MessageBody { get; set; }
        public DateTime SendDate { get; set; }
        public Boolean ReadFlag { get; set; }
        public int Id { get; set; }
        public Boolean AppropriateFlag { get; set; }
    }
}