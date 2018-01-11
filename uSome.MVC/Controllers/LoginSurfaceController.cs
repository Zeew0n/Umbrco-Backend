using System;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using uSome.Membership.Models;
using System.Web.Security;
using uSome.Membership.CustomMemership;
using Umbraco.Core.Services;
using uSome.Membership.Utilities;
using Umbraco.Core.Models;
using System.Web.Helpers;



namespace uSome.Membership.Controllers
{
    public class LoginSurfaceController : SurfaceController
    {
        private readonly ContentService contentService = new ContentService();
        CustomMemberShipProvider membeshipProvider = new CustomMemberShipProvider();

        //[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginMember(LoginModel loginModel, string dashboardUrl)
        {
            if (ModelState.IsValid)
            {                
                try
                {
                    var member = Members.GetByUsername(loginModel.LoginName);
                    if (member != null)
                    {
                        if (member.GetProperty("isVerified").Value.ToString() != "No")
                        {
                            if (ValidationLogic(loginModel, dashboardUrl, member))
                            {
                                return Redirect(dashboardUrl);
                            }

                        }
                        else 
                        {
                            //Check If User registeration date exceeds 30 days
                            if (!IsMemberExpired(member))
                            {
                                if (ValidationLogic(loginModel, dashboardUrl, member))
                                {
                                    return Redirect(dashboardUrl);
                                }
                            }
                            TempData["ErrorMsg"] = umbraco.library.GetDictionaryItem("UMF_UserRegistrationDateExpired");
                            return CurrentUmbracoPage();
                        }
                    }
                    else
                    {
                       TempData["ErrorMsg"] = umbraco.library.GetDictionaryItem("UMF_InvalidCredential");
                       return CurrentUmbracoPage();
                    }
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = umbraco.library.GetDictionaryItem("UMF_InvalidCredential");
                    return CurrentUmbracoPage();
                }
            }

            TempData["ErrorMsg"] = umbraco.library.GetDictionaryItem("UMF_InvalidCredential");
            return CurrentUmbracoPage();
           
        }


        [HttpGet]
        public ActionResult LogOutMember()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Redirect("/");  /* "/" always represents homepage */
        }


        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model, int emailTemplateNodeId, int parentEmailTemplateNodeId)
        {
            var passMessage =Umbraco.GetDictionaryValue("UMF_PasswordResetMsg").ToString();
            var failMessage =Umbraco.GetDictionaryValue("UMF_EmailDoNotExists").ToString();
            try
            {
                    var member = Members.GetByEmail(model.Email);
                    if (member != null)
                    {
                        //Changed form ResetPassword to GenereatePassword()
                        var mem = umbraco.cms.businesslogic.member.Member.GetMemberFromEmail(model.Email);
                        var newpassword = System.Web.Security.Membership.GeneratePassword(4, 0);
                        mem.Password = newpassword;
                        mem.Save();
                        
                        var parentEmailTemplate = contentService.GetById(parentEmailTemplateNodeId);
                        var fromEmail = parentEmailTemplate.Properties["fromEmail"].Value.ToString();
                        var emailTemplate = contentService.GetById(emailTemplateNodeId);
                        if (emailTemplate.Properties["sendToVisitor"].Value.ToString() == "1")
                        {

                            var visitorSubject = emailTemplate.Properties["subjectToVisitor"].Value.ToString();
                            var visitorMail = emailTemplate.Properties["mailToVisitor"].Value.ToString()
                                                    .Replace("#email#", model.Email).Replace("#newpassword#", newpassword);

                          
                            var isMailSend = new SendMail().Send(fromEmail, model.Email, "", visitorSubject, visitorMail);


                            if (isMailSend)
                            {
                                return Json(new { result = "pass", msg = passMessage });
                            }
                        }
                    }
                
                return Json(new {result ="fail",msg = failMessage});
            }
            catch (Exception ex)
            {
                return Json(new { result = "fail", msg = failMessage });
            }
           
        }


        [NonAction]
        public bool ValidationLogic(LoginModel loginModel, string dashboardUrl, Umbraco.Core.Models.IPublishedContent member)
        {
            if (membeshipProvider.ValidateUser(loginModel.LoginName, loginModel.Password))
            {
                Session["AllowedMember"] = member.Name;
                FormsAuthentication.SetAuthCookie(loginModel.LoginName, false);
                return true;
            }
            return false;
        }




        [NonAction]
        public bool IsMemberExpired(Umbraco.Core.Models.IPublishedContent member)
        {
            bool memberExpired = false;
            if ((System.DateTime.Now.Date-member.CreateDate.Date).TotalDays > 30)
            {
                    memberExpired = true;
                    return memberExpired;
            }
            return memberExpired;
        }


        public bool SendPasswordRecoverMail(ForgotPasswordModel model, IPublishedContent member, int emailTemplateNodeId, int parentEmailTemplateNodeId)
        {
            
            try
            {
                    var u = System.Web.Security.Membership.GetUser(member.GetProperty("email").Value.ToString());
                    if (u == null)
                    {
                        return false;
                    }
                    var newpassword = u.ResetPassword();
                    var parentEmailTemplate = contentService.GetById(parentEmailTemplateNodeId);
                    var fromEmail = parentEmailTemplate.Properties["fromEmail"].Value.ToString();
                    var emailTemplate = contentService.GetById(emailTemplateNodeId);

                    if (emailTemplate.Properties["sendToVisitor"].Value.ToString() == "1")
                    {
                       
                        var visitorSubject = emailTemplate.Properties["subjectToVisitor"].Value.ToString();
                        var visitorMail = emailTemplate.Properties["mailToVisitor"].Value.ToString()
                                                .Replace("#email#", u.Email).Replace("#newpassword#", newpassword.ToString());
                        var isMailSend = new SendMail().Send(fromEmail, model.Email, "", visitorSubject, visitorMail);
                        if (isMailSend)
                        {
                            return true;
                        }
                    }
                    return false;
            }
            catch (Exception ex)
            {
                Helper.CreateErrorLogMessage(ex.Message + "with inner exception '" + ex.InnerException + "'");
                return false;
            }

        }
    }
}
