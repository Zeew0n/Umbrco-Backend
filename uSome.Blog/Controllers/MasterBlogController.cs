using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;


namespace uSome
{
    public class MasterBlogController : SurfaceController
    {
        MasterBlog mb = new MasterBlog();

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult MasterBlog(MasterBlogModel masterBlogModel, HttpPostedFileBase Image, int? memberId)
        {
            
            var vtourDirectoryPath = Server.MapPath(string.Format("/BlogMedia/{0}/", memberId));

            if (!Directory.Exists(vtourDirectoryPath))
            {
                Directory.CreateDirectory(vtourDirectoryPath);
            }


            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                       var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(vtourDirectoryPath, fileName);
                        file.SaveAs(path);
                        masterBlogModel.Image = string.Format("/BlogMedia/{0}/{1}", memberId, fileName);

                    }

                }
                try
                {
                    var logComment = "Created Blog Title";
                    masterBlogModel.UserId =Convert.ToInt32(memberId);
                    mb.Save(masterBlogModel);
                    if (masterBlogModel.ID != 0)
                    {
                        logComment = "Updated Blog Title";
                    }
                    var logModel = new LogModel
                    {
                        UserId = int.Parse(memberId.ToString()),
                        NodeId = new MasterBlog().GetLatestMasterBlogId(),
                        LogHeader = "Blog Post",
                        LogComment =logComment,
                        TableName ="uSomeMasterBlog"
                    };
                    new LogHelper().Save(logModel);  

                    TempData["MasterBlogCreationSuccessMessage"] = "Blog created Successfully";
                    return RedirectToCurrentUmbracoPage();
                }
                catch (Exception ex)
                {
                      Log.ErrorLog("Error on creating blog title :: " + ex.Message);
                    @TempData["ErrorMessage"] = "failed";
                    return CurrentUmbracoPage();
                }
            }
            return CurrentUmbracoPage();
        }

    }
}
