using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uSome.Forum.Models;

namespace uSome
{
    public class Question
    {
        private DataHelper _dataHelper = new DataHelper();
        public bool Save(QuestionModel model,out int recentId)
        {
            if (CheckExisting(model.ID))
            {
                return Update(model,out recentId);
            }
            else
            {
                return Add(model, out recentId);
            }
        }

        public void GetQuestionPoint(out int Point)
        {
            try {
            var queryString="SELECT [Question],[Answer],[MarkAsAnwer],[HiFive] FROM [dbo].[uSomeForumPoints] ";
                Point=_dataHelper.ExecuteScalar(queryString);
            }
            catch(Exception ex)
            {
                Point = 1;
            }

        }
        public void GetAnswerPoint(out int Point)
        {
            try {
            var queryString="SELECT [Question] FROM [dbo].[uSomeForumPoints] ";
            Point = _dataHelper.ExecuteScalar(queryString);
            }
            catch (Exception ex)
            {
                Point = 1;
            }

        }
         public void GetMarkAsAnswerPoint(out int Point)
        {
            try {
            var queryString="SELECT [MarkAsAnwer] FROM [dbo].[uSomeForumPoints] ";
            Point = _dataHelper.ExecuteScalar(queryString);
            }
            catch (Exception ex)
            {
                Point = 1;
            }

        }
          public void GetHiFivePoint(out int Point)
        {
            try {
            var queryString="SELECT [HiFive] FROM [dbo].[uSomeForumPoints] ";
            Point = _dataHelper.ExecuteScalar(queryString);
            }
            catch (Exception ex)
            {
                Point = 1;
            }

        }
        bool Add(QuestionModel model, out int recentId)
        {
            try
            {
                var sql = string.Format(@"INSERT INTO [dbo].[uSomeForum] ([userId],[title],[content],[isPublic],[type],[groupId]) VALUES
                                        ('{0}','{1}','{2}','{3}','{4}','{5}');select Scope_Identity()", model.UserId, model.Title, model.Content, model.IsPublic,model.Type,model.GroupId);
                recentId = _dataHelper.ExecuteScalar(sql);

                return true;
            }
            catch (Exception ex)
            {
                recentId = -1;
                return false;
            }
           
        }
        bool Update(QuestionModel model,out int recendId)
        {
            recendId = model.ID;
            try
            {
                var sql = string.Format(@"UPDATE [dbo].[uSomeForum] SET title ='{0}',content ='{1}', isPublic='{2}'", model.Title, model.Content, model.IsPublic);
                _dataHelper.ExecuteNonQuery(sql);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
          
        }
        bool CheckExisting(int id)
        {
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeForum WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
        public QuestionModel GetQuestionModel(string condition)
        {
            var questionList = GetQuestionsByCondition(condition);
            var questionModel = new QuestionModel();
            foreach (var question in questionList)
            {
                questionModel.ID = question.ID;
                questionModel.Title = question.Title;
                questionModel.UserId = question.UserId;
                questionModel.Content = question.Content;
                questionModel.CreatedOn = question.CreatedOn;
                questionModel.IsPublic = question.IsPublic;
                questionModel.GroupId = question.GroupId;
            }
            return questionModel;
        }
        public IList<QuestionModel> GetQuestionsByCondition(string condition)
        {
            string sql = string.Format("SELECT [id],[title],[userId],[createdOn],[content],[isPublic],[type],[groupId] FROM [dbo].[uSomeForum] WHERE {0}", condition);
            return GetQuestion(sql);
        }

        public IList<AllUserModel> GetAllUserByCondition(string condition)
        {
            string sql = string.Format("select f.userId,f.groupId,f.forumid,f.replyid from totalForumrecord as f union all " +
                                        "select r.replyuser,r.groupId,r.forumid,r.replyid from totalForumrecord as r WHERE {0}", condition);
            return GetUser(sql);
        }
        IList<QuestionModel> GetQuestion(string sql)
        {
            var questionList = new List<QuestionModel>();
            var dr = _dataHelper.ExecuteReader(sql);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var questionModel = new QuestionModel
                    {
                        ID = dr.GetInt("ID"),
                        Title = dr.GetString("Title"),
                        UserId = dr.GetInt("UserId"),
                        CreatedOn = dr.GetDateTime("CreatedOn"),
                        Content = dr.GetString("Content"),
                        IsPublic = dr.GetBoolean("IsPublic"),
                        Type = dr.GetString("type"),
                        GroupId = dr.GetInt("groupId")
                    };
                    questionList.Add(questionModel);
                }
            }
            return questionList;
        }
        IList<AllUserModel> GetUser(string sql)
        {
            var userList = new List<AllUserModel>();
            var dr = _dataHelper.ExecuteReader(sql);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var usermodel = new AllUserModel
                    {
                        userId = dr.GetInt("userId"),
                        groupId = dr.GetInt("groupId"),
                        forumId = dr.GetInt("forumid"),
                        replyid = dr.GetInt("replyid"),
                    
                    };
                    userList.Add(usermodel);
                }
            }
            return userList;
        }

    }
}