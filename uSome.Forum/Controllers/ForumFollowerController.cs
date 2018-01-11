using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using uSome.Forum.Models;
namespace uSome
{
    public class ForumFollowerController :SurfaceController
    {
        // GET api/<controller>
        public JsonResult AddFollowers(string blogId, string userId, string cmd)
        {
            var list = new List<ForumFollowersModel>();
            var model = new ForumFollowersModel();
            model.ForumId = int.Parse(blogId);
            model.UserId = int.Parse(userId);

            try
            {
                if (cmd == "follow")
                {
                    new ForumFollowers().Save(model);
                    model.PostAction = "unfollow";
                    model.FeedbackMsg = Umbraco.GetDictionaryValue("UnFollowThisBlog");
                }
                else
                {
                    new ForumFollowers().Delete(model);
                    model.PostAction = "follow";
                    model.FeedbackMsg = Umbraco.GetDictionaryValue("FollowThisBlog");
                }
                var logModel = new LogModel
                {
                    NodeId = model.ForumId,
                    UserId = model.UserId,
                    LogHeader = "Forum Post",
                    LogComment = "Followed Forum",
                    TableName = "uSomeForumFollowers"
                };
                new LogHelper().Save(logModel);
            }
            catch (Exception ex)
            {
                model.FeedbackMsg = Umbraco.GetDictionaryValue("FollowThisBlog");
                Log.ErrorLog("Error in adding followers :: " + ex.Message);
            }


            list.Add(model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

       
    }
}