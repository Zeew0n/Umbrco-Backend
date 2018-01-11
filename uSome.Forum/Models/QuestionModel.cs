using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace uSome
{
    public class QuestionModel
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Type { get; set; }

        public int GroupId { get; set; }
    }
}