using System;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using umbraco.cms.businesslogic.member;
using System.Web.Security;
namespace uSome
{
    public class QuestionController : SurfaceController
    {
       
        public IMemberService _memberService = ApplicationContext.Current.Services.MemberService;
        private readonly ContentService contentService = new ContentService();
      
        private string feedbackMsg = "Error";
        private string result = "fail";

        /// <summary>
        /// Posting question on the forum
        /// </summary>
        /// <param name="model">model of the question</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostQuestion(QuestionModel model)
        {
              
           
            try
            {
                var _question = new Question();
                Member currentMember = Member.GetCurrentMember();
               model.Content= model.Content.Replace("'", "''");
               model.Title = model.Title.Replace("'", "''");
                model.UserId = currentMember.Id;
                var recentId = 0;
                if (_question.Save(model, out recentId))
                {
                    feedbackMsg = (umbraco.library.GetDictionaryItem("fbOnQuestionPostingInForum") != null && umbraco.library.GetDictionaryItem("fbOnQuestionPostingInForum") != "")
                                                    ? umbraco.library.GetDictionaryItem("fbOnQuestionPostingInForum")
                                                    : "Thank you for posting question";
                   
                    var logComment = model.Type;
                    if (model.ID != 0)
                    {
                        logComment = "Updated  " +logComment;
                    }
                    var logModel = new LogModel
                    {
                        UserId = int.Parse(currentMember.Id.ToString()),
                        NodeId = recentId,
                        LogHeader = "Forum Post",
                        LogComment =  "Created  " +logComment,
                        TableName = "uSomeForum"
                    };

                    new LogHelper().Save(logModel);
                    result = "success";
                    var member = _memberService.GetById(model.UserId);
                    int questionPoint;
                    _question.GetQuestionPoint(out questionPoint);
                    try
                    {
                        member.Properties["points"].Value = int.Parse(member.Properties["points"].Value.ToString()) + questionPoint;
                    }
               
                    catch (Exception ex)
                    {
                        member.Properties["points"].Value = 1;
                    }
                    _memberService.Save(member);
                    try
                    {
                        var parentEmailTemplateNodeId = Convert.ToInt16(Request.Form["parentEmailTemplateNodeId"].ToString());
                        var emailTemplateNodeId = Convert.ToInt16(Request.Form["emailTemplateNodeId"].ToString());
                        var parentEmailTemplate = contentService.GetById(parentEmailTemplateNodeId);
                        var ForumFollowerModel = new ForumFollowerMailModel();
                        ForumFollowerModel.FromEmail = parentEmailTemplate.Properties["fromEmail"].Value.ToString();
                        ForumFollowerModel.Bcc = parentEmailTemplate.Properties["bCCEmail"].Value.ToString();
                        //  var adminEmail = parentEmailTemplate.Properties["adminEmail"].Value.ToString();


                        var emailTemplate = contentService.GetById(emailTemplateNodeId);
                        var subject = emailTemplate.Properties["subject"].Value.ToString();
                        var mailBody = emailTemplate.Properties["mailBody"].Value.ToString();
                        ForumFollowerModel.MailSubject = subject.Replace("#type#", model.Type);
                        ForumFollowerModel.Message = mailBody.Replace("#type#", model.Type);
                        ForumFollowerModel.Message = ForumFollowerModel.Message.Replace("#title#",model.Title);
                        ForumFollowerModel.Message = ForumFollowerModel.Message.Replace("#content#", model.Content);


                        ForumFollowerModel.ForumId = model.GroupId;
                        var _forumFollower = new ForumFollowers();
                        _forumFollower.FollowersMailSend(ForumFollowerModel);
                    }
                    catch(Exception)
                    {}
                  

                }

                
            }
            catch (Exception ex)
            {
                feedbackMsg = (umbraco.library.GetDictionaryItem("errMsgOnQuestionPostingInForum") !=null &&umbraco.library.GetDictionaryItem("errMsgOnQuestionPostingInForum") != "")
                               ? umbraco.library.GetDictionaryItem("errMsgOnQuestionPostingInForum") : "Error on posting question. Please contact administrator for details";
                result = "fail";
                Log.ErrorLog("Error in saving forum question(" + ex.Message + ")");
            }
            return Json(new { result = result, message = feedbackMsg }); 
        }

        
	}
}