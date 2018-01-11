using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using uSome.Membership.Models;
namespace uSome
{
    public class ContactController : SurfaceController
    {
        //
        // GET: /Contact/

        public ActionResult SendContactRequest(int requestFrom, int requestTo, string requestMsg)
        {
            try
            {
                var model = new MemberContactsModel{
                    RequestFrom = requestFrom,
                    RequestTo=requestTo,
                    RequestMessage =requestMsg,
                    RequestConfirmed = string.Empty,
                    RequestDate= System.DateTime.Now
                };
                new MemberContacts().Save(model);
               
                return Json(new {result="success"},JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
               return Json(new {result="fail due to" + ex.Message},JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ConfirmContactRequest(int requestFrom, int requestTo, int id)
        {
            try
            {
                var model = new MemberContactsModel
                {
                    RequestFrom = requestFrom,
                    RequestTo = requestTo,
                    ID = id,
                    ConfirmDate = System.DateTime.Now,
                    RequestConfirmed=true
                };
                new MemberContacts().Save(model);
                var modelTo = new MemberContactsModel
                {
                    RequestFrom = requestTo,
                    RequestTo = requestFrom,
                    ConfirmDate = System.DateTime.Now,
                    RequestConfirmed=true
                };
                string condition=string.Format("requestFrom = '{0}' and requestTo = '{1}'", requestTo, requestFrom);
                var contact = new MemberContacts().GetMemberContact(condition);
                if (contact.ID!=0)
                {
                    modelTo.ID = contact.ID;
                }
                new MemberContacts().Save(modelTo);
              
                return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
