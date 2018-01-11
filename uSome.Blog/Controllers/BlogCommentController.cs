using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace uSome
{
    public class BlogCommentController : SurfaceController
    {
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult PostComment(CommentModel commentModel, int memberId, int bid, string blogUrl)
        {
                
                try
                {
                    commentModel.ContentId = bid;
                    commentModel.UserId = memberId;
                    new BlogComment().Save(commentModel);
                    TempData["success"] = "Success";
                    return Redirect(blogUrl);
                    var logModel = new LogModel
                    {
                        NodeId = bid,
                        UserId = memberId,
                        LogHeader= "Blog Post",
                        LogComment="Posted Comment",
                        TableName="uSomeBlogComment"
                    };
                      new LogHelper().Save(logModel);  
                }
                catch (Exception ex)
                {
                      Log.ErrorLog("Error in blog comment :: " + ex.Message);
                     ViewData["fail"] = "Error" + ex.Message;
                    return CurrentUmbracoPage();
                }
             
           
        }

    }
}
