using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace uSome
{
    public class BlogFollowerController : SurfaceController
    {
        //
        // GET: /BlogFollower/

        public JsonResult AddFollowers(string blogId,string userId,string cmd)
        {
            var list = new List<BlogFollowersModel>();
            var model = new BlogFollowersModel();
            model.BlogId = int.Parse(blogId);
            model.UserId = int.Parse(userId);
           
            try
            {
                if (cmd == "follow")
                {
                    new BlogFollowers().Save(model);
                    model.PostAction = "unfollow";
                    model.FeedbackMsg = Umbraco.GetDictionaryValue("UnFollowThisBlog");
                }
                else
                {
                    new BlogFollowers().Delete(model);
                    model.PostAction = "follow";
                    model.FeedbackMsg = Umbraco.GetDictionaryValue("FollowThisBlog");
                }
                var logModel = new LogModel
                {
                    NodeId = model.BlogId,
                    UserId = model.UserId,
                    LogHeader ="Blog Post",
                    LogComment = "Followed Blog",
                    TableName = "uSomeBlogFollowers"
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
