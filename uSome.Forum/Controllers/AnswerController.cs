using System;
using umbraco.BusinessLogic;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using umbraco.cms.businesslogic.member;
using System.Web.Mvc;
using System.Web.Security;
namespace uSome
{
    public class AnswerController : SurfaceController
    {

        public  IMemberService _memberService =  ApplicationContext.Current.Services.MemberService;
        private readonly ContentService contentService = new ContentService();
        private string feedbackMsg = "Error";
        private string result = "fail";
        public Answer _answer = new Answer();
        /// <summary>
        /// Posting answer for the question in forum
        /// </summary>
        /// <param name="model">model of the question</param>
        /// <returns></returns>
        public ActionResult PostForumAnswer(AnswerModel model)
        {
            try
            {
               // model.ContentId = int.Parse(questionId);
                Member currentMember = Member.GetCurrentMember();
                model.UserId = currentMember.Id;
                model.Comment = model.Comment.Replace("'", "''");
                var recendId = 0;
                if (new Answer().Save(model, out recendId))
                {
                    feedbackMsg = (umbraco.library.GetDictionaryItem("fbOnAnswerPostingInForum") != null && umbraco.library.GetDictionaryItem("fbOnAnswerPostingInForum") != "")
                                ? umbraco.library.GetDictionaryItem("fbOnAnswerPostingInForum")
                                : "Thank you for your answer";
                    var logComment = "Reply discussion";
                    if (model.ID != 0)
                    {
                        logComment = "Updated  " + logComment;
                    }
                    var logModel = new LogModel
                    {
                        UserId = int.Parse(currentMember.Id.ToString()),
                        NodeId = recendId,
                        LogHeader = "Forum Post",
                        LogComment = logComment,
                        TableName = "uSomeForumReply"
                    };

                    new LogHelper().Save(logModel);
                    result = "success";
                    var member = _memberService.GetById(model.UserId);
                    int questionPoint;
                    new Question().GetAnswerPoint(out questionPoint);
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
                        var bccEmail = parentEmailTemplate.Properties["bCCEmail"].Value.ToString();
                        var adminEmail = parentEmailTemplate.Properties["adminEmail"].Value.ToString();


                        var emailTemplate = contentService.GetById(emailTemplateNodeId);
                        ForumFollowerModel.MailSubject = emailTemplate.Properties["subject"].Value.ToString();
                        var mailBody = emailTemplate.Properties["mailBody"].Value.ToString();

                        ForumFollowerModel.Message = mailBody.Replace("#reply#", model.Comment);
                        ForumFollowerModel.ForumId = model.ContentId;
                        ForumFollowerModel.TransType = "Ans";
                        var _forumFollower = new ForumFollowers();
                        _forumFollower.FollowersMailSend(ForumFollowerModel);
                    }
                    catch (Exception)
                    { }
                }
            }
            catch (Exception ex)
            {
                feedbackMsg = umbraco.library.GetDictionaryItem("errMsgOnAnswerPostingInForum") != ""
                               ? umbraco.library.GetDictionaryItem("errMsgOnAnswerPostingInForum") : "Error on posting answer. Please Login For Posting Answer.";
                result = "fail";
                 Log.ErrorLog("Error in saving forum answer("+ ex.Message +")");
            }
            return Json(new { result = result, message = feedbackMsg }); 
        }

        /// <summary>
        /// Update the points of the member whose answere is set to mark as read
        /// </summary>
        /// <param name="userId">Id of the whose answer is marked as read </param>
        /// <returns></returns>
        public ActionResult MarkAsRead(string id, string userId)
        {
            try
            {

                var answerModel = _answer.GetAnswerModel(string.Format("id='{0}'", id));
                answerModel.MarkAsAnswer = true;
                _answer.MarkAsAnswer(answerModel);
                var member = _memberService.GetById(int.Parse(userId));
                int questionPoint;
                new Question().GetMarkAsAnswerPoint(out questionPoint);
                member.Properties["points"].Value = int.Parse(member.Properties["points"].Value.ToString()) + questionPoint;
                _memberService.Save(member);
                feedbackMsg = "Points updated";
                result = "success";
            }
            catch (Exception ex)
            {
                feedbackMsg = "Error in updating points";
                result = "fail";
                Log.ErrorLog("Error in updating points (" + ex.Message + ")");
            }
            return Json(new { result = result, message = feedbackMsg });
        }

        public ActionResult LikeAnswer(string id,string userId)
        {
            var totalLikeCount=0;
            try
            {
                Member currentMember = Member.GetCurrentMember();
                var answerModel = _answer.GetAnswerModel(string.Format("id='{0}'", id));
                answerModel.LikeUsers = answerModel.LikeUsers + currentMember.Id.ToString() + ",";
                 totalLikeCount=answerModel.likeCount+1;
                  _answer.LikeAnswer(answerModel,totalLikeCount);
                var member = _memberService.GetById(int.Parse(userId));
                int questionPoint;
                new Question().GetHiFivePoint(out questionPoint);
                member.Properties["points"].Value = int.Parse(member.Properties["points"].Value.ToString()) + questionPoint;
                _memberService.Save(member);
                feedbackMsg = "Points updated";
                result = "success";

               
            }
            catch(Exception ex)
            {
                feedbackMsg = "Error in updating points";
                result = "fail";
                Log.ErrorLog("Error in updating points (" + ex.Message + ")");
            }
              return Json(new { result = result, message = feedbackMsg,totallike=totalLikeCount });
        }
        
	}
}