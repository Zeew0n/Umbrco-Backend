using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uSome.Forum.Models;

namespace uSome
{
    public class ForumFollowers
    {
        private DataHelper _dataHelper = new DataHelper();
        public bool Save(ForumFollowersModel model)
        {
            if (CheckExisting(model.ID))
            {
                return Update(model);
            }
            else
            {
                return Add(model);
            }
        }

        private bool Add(ForumFollowersModel model)
        {
            try
            {
                string sql = string.Format(@"INSERT INTO [dbo].[uSomeForumFollowers] ([forumId] ,[userId]) VALUES ('{0}','{1}')", model.ForumId, model.UserId);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on adding Forum followers ::" + ex.Message);
                return false;
            }

        }

        private bool Update(ForumFollowersModel model)
        {
            try
            {
                string sql = string.Format("UPDATE [dbo].[uSomeForumFollowers] SET [follow] = '{0}' WHERE id='{1}' and forumId ='{1}' and userId ='{2}'", model.Follow, model.ID, model.ForumId, model.UserId);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on updating forum followers ::" + ex.Message);
                return false;
            }

        }
        public bool Delete(ForumFollowersModel model)
        {
            try
            {
                string sql = string.Format(@"DELETE FROM [dbo].[uSomeForumFollowers] WHERE (forumId='{0}' and userId='{1}')", model.ForumId, model.UserId);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on deleting Forum followers ::" + ex.Message);
                return false;
            }

        }
        bool CheckExisting(int id)
        {
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeForumFollowers WHERE ID ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
        public IList<ForumFollowersModel> GetFollowers(string condition)
        {
            var sql = string.Format("SELECT [ID],[forumId],[userId],[followedDate],[follow] FROM [dbo].[uSomeForumFollowers] WHERE {0}", condition);
            return GetBlogFollowers(sql);
        }

        public IList<ForumFollowersModel> GetFollowers()
        {
            var sql = "SELECT [ID],[forumId],[userId],[followedDate],[follow] FROM [dbo].[uSomeForumFollowers]";
            return GetBlogFollowers(sql);
        }

        private IList<ForumFollowersModel> GetBlogFollowers(string sqlText)
        {
            var followerList = new List<ForumFollowersModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var follower = new ForumFollowersModel();
                    follower.ID = dr.GetInt("ID");
                    follower.UserId = dr.GetInt("userId");
                    follower.ForumId = dr.GetInt("forumId");
                    follower.Follow = dr.GetBoolean("follow");
                    follower.FollowedDate = dr.GetDateTime("followedDate");
                    followerList.Add(follower);
                }
            }
            return followerList;
        }

        public bool FollowersMailSend(ForumFollowerMailModel model)
        {
            bool flag = true;
            try
            {
                var toEmailList = new List<string>();
                
                if (model.TransType == "Ans")
                {
                    var answerModel = new Question().GetQuestionModel(string.Format("id='{0}'", model.ForumId));
                    var selectQuery = string.Format("select f.Id,f.forumid,f.userid,c.Email from uSomeForumFollowers as f INNER JOIN  [cmsMember] as c on c.nodeId=f.userId where f.forumId='{0}'", answerModel.GroupId);
                    var dr = _dataHelper.ExecuteReader(selectQuery);
                    if (dr.HasRecords)
                    {
                        while (dr.Read())
                        {
                            var Email = dr.GetString("Email");
                            model.Message = model.Message.Replace("#email#", Email);
                            model.Message = model.Message.Replace("#title#", answerModel.Title);
                            model.Message = model.Message.Replace("#content#", answerModel.Content);
                            new Utilities.AsyncMail().Send(model.Message, model.MailSubject, model.FromEmail, Email,model.Bcc);
                            //toEmailList.Add(Email);
                        }
                    }
                }
                else
                {
                    var selectQuery = string.Format("select f.Id,f.forumid,f.userid,c.Email from uSomeForumFollowers as f INNER JOIN  [cmsMember] as c on c.nodeId=f.userId where f.forumId='{0}'", model.ForumId);
                    var dr = _dataHelper.ExecuteReader(selectQuery);
                    if (dr.HasRecords)
                    {
                        while (dr.Read())
                        {
                            var Email = dr.GetString("Email");
                            model.Message = model.Message.Replace("#email#", Email);
                            new Utilities.AsyncMail().Send(model.Message, model.MailSubject, model.FromEmail, Email, model.Bcc);
                            //toEmailList.Add(Email);
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                flag = false;
            }
            return flag;
          
        }
    }
}