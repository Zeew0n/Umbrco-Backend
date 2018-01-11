using System;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using uSome.Membership.Models;
using umbraco.cms.businesslogic.member;
using System.Web.Security;
using Umbraco.Core.Services;
using uSome.Membership.Utilities;


namespace uSome.Membership.Controllers
{
    public class RegistrationSurfaceController : SurfaceController
    {
        private readonly ContentService contentService = new ContentService();

        private const string MembersType = "uSomeMember";
        private const string MembersGroup = "uSomeMember";

        [HttpPost]
        public ActionResult Register(RegisterModel registerModel, int parentEmailTemplateNodeId, int emailTemplateNodeId)
        {
            //registerModel.LoginName=registerModel.Email;
            try
            {
                if (Member.GetMemberFromLoginName(registerModel.LoginName) == null)
                {
                    if (Member.GetMemberFromEmail(registerModel.Email) == null)
                    {
                        if (registerModel.Password == registerModel.ConfirmPassword)
                        {

                            // Set the member type and group
                            var mt = MemberType.GetByAlias(MembersType);
                            var addToMemberGroup = MemberGroup.GetByName(MembersGroup);


                            var m = Member.MakeNew(registerModel.LoginName, mt, new umbraco.BusinessLogic.User(0));

                            m.Email = registerModel.Email;
                            m.LoginName = registerModel.LoginName;
                            m.Password = registerModel.Password;
                            m.getProperty("isVerified").Value = "No";

                            m.Save();
                            m.XmlGenerate(new System.Xml.XmlDocument());

                            this.SendRegisterConfirmEmail(registerModel, parentEmailTemplateNodeId, emailTemplateNodeId);

                            FormsAuthentication.SetAuthCookie(registerModel.LoginName, false);
                            TempData["SuccessMessage"] = umbraco.library.GetDictionaryItem("UMF_MemberCreatedSuccess");
                            return RedirectToCurrentUmbracoPage();
                        }
                        else
                        {
                            TempData["ErrorMessage"] = umbraco.library.GetDictionaryItem("UMF_PasswordMisMatch");
                            return CurrentUmbracoPage();
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = umbraco.library.GetDictionaryItem("UMF_UserExistWithEmail");
                        return CurrentUmbracoPage();
                    }
                    
                }
                else
                {
                    TempData["ErrorMessage"] = umbraco.library.GetDictionaryItem("UMF_UserExistWithUsername");
                    return CurrentUmbracoPage();
                }
               
            }
            catch (Exception)
            {
                
                TempData["ErrorMessage"] = umbraco.library.GetDictionaryItem("UMF_UserCreateFail");
                return CurrentUmbracoPage();
            }

        }

        [ChildActionOnly]
        public ActionResult SendRegisterConfirmEmail(RegisterModel registerModel, int parentEmailTemplateNodeId, int emailTemplateNodeId)
        {
            var errorMsg = "registration failed !!!";
            if (umbraco.library.GetDictionaryItem("") != null)
            {
                if (umbraco.library.GetDictionaryItem("") != "")
                {
                    errorMsg = umbraco.library.GetDictionaryItem("");
                }
            }
            try
            {

                var parentEmailTemplate = contentService.GetById(parentEmailTemplateNodeId);
                
                var fromEmail = parentEmailTemplate.Properties["fromEmail"].Value.ToString();
                var bccEmail = parentEmailTemplate.Properties["bCCEmail"].Value.ToString();
                var adminEmail = parentEmailTemplate.Properties["adminEmail"].Value.ToString();


                var emailTemplate = contentService.GetById(emailTemplateNodeId);
                var subject = emailTemplate.Properties["subject"].Value.ToString();
                var mailBody = emailTemplate.Properties["mailBody"].Value.ToString();

                mailBody.Replace("#email#", registerModel.Email);

                var isSuccess = new SendMail().Send(fromEmail,adminEmail,bccEmail,subject,mailBody);

                if (isSuccess)
                {
                    var successMsg = "registration is approved";
                    if (umbraco.library.GetDictionaryItem("")!=null)
                    {
                        if (umbraco.library.GetDictionaryItem("")!="")
                        {
                            successMsg = umbraco.library.GetDictionaryItem("");
                        }
                    }
                    if (emailTemplate.Properties["sendToVisitor"].Value.ToString() == "1")
                    {
                        var CompleteRegisterUrl= Request.Url.Host;
                        var encryptedMail = new EncryptedQueryString();
                        encryptedMail["EM"] =registerModel.Email;
                        var visitorSubject = emailTemplate.Properties["subjectToVisitor"].Value.ToString();
                        var visitorMail = emailTemplate.Properties["mailToVisitor"].Value.ToString();
                        visitorMail = visitorMail.Replace("#email#", registerModel.Email).Replace("#CompleteRegisterUrl#", CompleteRegisterUrl + "/register/register-confirm?" + "email=" + encryptedMail).Replace("#username#", registerModel.LoginName);

                        var isMailSend = new SendMail().Send(fromEmail,registerModel.Email,bccEmail,visitorSubject,visitorMail);
                        if (isMailSend)
                        {
                            return Json( new {result ="success",message= successMsg} );
                        }

                    }
                }
                return Json(new { result = "fail", message = errorMsg });

            }
            catch (Exception ex)
            {
                Helper.CreateErrorLogMessage(ex.Message + "with inner exception '" + ex.InnerException + "'");
                return Json(new { result = "fail", message = errorMsg });
            }
        }

    }
}
