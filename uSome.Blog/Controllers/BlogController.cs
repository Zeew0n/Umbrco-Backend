using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace uSome
{
    public class BlogController : SurfaceController
    {
        Blog blog = new Blog();

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBlog(BlogModel blogModel,  int? memberId)
        {
            if (ModelState.IsValid)
            {
               
                try
                {
                    var logComment = "Created Blog";
                    blogModel.UserId = Convert.ToInt32(memberId);
                    blogModel.ParentId = new MasterBlog().GetMasterBlogByUserId(blogModel.UserId).ID;
                    blog.Save(blogModel);
                    if (blogModel.Id != 0)
                    {
                        logComment = "Updated Blog";
                    }
                    var logModel = new LogModel
                    {
                        UserId = int.Parse(memberId.ToString()),
                         NodeId = new Blog().GetLatestBlogId(),
                        LogHeader ="Blog Post",
                        LogComment=logComment,
                        TableName="uSomeBlog"
                    };
                   
                    new LogHelper().Save(logModel);  

                    TempData["Result"] = "Success";
                    return RedirectToCurrentUmbracoPage();
                }
                catch (Exception ex)
                {
                    ViewData["Result"] = "failed";
                    Log.ErrorLog("Error on creating blog :: " + ex.Message);
                    return CurrentUmbracoPage();
                }
            }
            return CurrentUmbracoPage();
        }

        

    }
}
