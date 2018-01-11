using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using uSome.Membership.Models;
namespace uSome.Membership.Controllers
{
    public class FollowItemController : SurfaceController
    {
        //
        // GET: /FollowItem/
        private DataHelper _dataHelper = new DataHelper();
        public ActionResult UnfollowItems(string[] items)
        {
            foreach (var item in items)
            {
                try
                {
                    var id = item.Split(',')[0];
                    var type = item.Split(',')[1];
                    var sql = "";
                    if (type == "blog")
                    {
                        sql = string.Format("DELETE FROM [dbo].[uSomeBlogFollowers] WHERE id ='{0}'", id);
                    }
                    else
                    {
                        sql = string.Format("DELETE FROM [dbo].[uSomeForumFollowers] WHERE id ='{0}'", id);
                    }
                    _dataHelper.ExecuteNonQuery(sql);
                }
                catch (Exception ex) { }
            }
            return RedirectToCurrentUmbracoPage();
        }

    }
}
