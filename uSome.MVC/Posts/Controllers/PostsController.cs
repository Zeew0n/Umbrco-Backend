using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using umbraco.NodeFactory;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

namespace uSome
{
    public class PostsController : SurfaceController
    {
        //
        // GET: /Posts/
        private readonly ContentService contentService = new ContentService();
        private string feedbackMsg = "Error";
        private string result = "fail";
        public ActionResult SendPost(PostModel model)
        {
            try
            {
                var rowaffected = 0;
                new Posts().save(model, out rowaffected);
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "fail due to" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PostsReceived()
        {
            return PartialView("_PostsReceived");
        }
        public ActionResult PostsSend(string Userid, string currentCulture)
        {
            var SendMessage = new List<PostModel>();
            try
            {
                var rootNode = contentService.GetContentOfContentType(13503).FirstOrDefault().Id;
                var personNodeid = contentService.GetContentOfContentType(12502).FirstOrDefault().Id;
                ViewData["personNodeid"] = personNodeid;
                ViewData["currentCulture"] = currentCulture;
                ViewData["homepage"] = rootNode;
                var _userid = Convert.ToInt16(Userid);

                if (_userid == 0)
                    return PartialView("_PostsSend", SendMessage);
                SendMessage = new Posts().GetAllMessageWithCondition(string.Format("FromUserId='{0}'", _userid)).ToList();
                return PartialView("_PostsSend", SendMessage);
            }
            catch(Exception ex)
            {
                return PartialView("_PostsSend", SendMessage);
              
            }
            
        }
    }
}
