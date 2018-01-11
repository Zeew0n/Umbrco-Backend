using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class Answer
    {
        private DataHelper _dataHelper = new DataHelper();
        public bool Save(AnswerModel model,out int recendId)
        {
            if (CheckExisting(model.ID))
            {
                return Update(model, out recendId);
            }
            else
            {
                return Add(model, out recendId);
            }
        }
         public bool MarkAsAnswer(AnswerModel model)
        {
            try
            {
                var sql = string.Format(@"UPDATE [dbo].[uSomeForumReply] SET markAsAnswer='{0}' WHERE contentId ='{1}';UPDATE [dbo].[uSomeForumReply] SET markAsAnswer='{2}' WHERE id ='{3}'", false, model.ContentId,model.MarkAsAnswer,model.ID);
              _dataHelper.ExecuteNonQuery(sql);  
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool LikeAnswer(AnswerModel model,int countLike)
         {
             try
             {
                 var sql = string.Format(@"UPDATE [dbo].[uSomeForumReply] SET likeCount='{0}',LikeUsers='{2}' WHERE id ='{1}'", countLike, model.ID,model.LikeUsers);
                 _dataHelper.ExecuteNonQuery(sql);
                 return true;
             }
             catch (Exception ex)
             {
                 return false;
             }

         }
        bool Add(AnswerModel model,out int recendId)
        {
            try
            {
                var cmd = string.Format(@"INSERT INTO [dbo].[uSomeForumReply] ([contentId],[userId],[comment],[isPublic]) VALUES
                                        ('{0}','{1}','{2}','{3}');select Scope_Identity()", model.ContentId, model.UserId, model.Comment, model.IsPublic);
               recendId= _dataHelper.ExecuteScalar(cmd);
                return true;
            }
            catch (Exception ex)
            {
                recendId=-1;
                return false;
            }
           
        }
        bool Update(AnswerModel model,out int recentId)
        {
            recentId = model.ID;
            try
            {
                var sql = string.Format(@"UPDATE [dbo].[uSomeForumReply] SET comment ='{0}', isPublic='{1}',markAsAnswer='{2}' WHERE ID ='{3}'", model.Comment, model.IsPublic,model.MarkAsAnswer,model.ID);
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
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeForumReply WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }

        public AnswerModel GetAnswerModel(string condition)
        {
            var answerList = GetAnswersByCondition(condition);
            var answerModel = new AnswerModel();
            foreach (var answer in answerList)
            {
                answerModel.ID = answer.ID;
                answerModel.Comment = answer.Comment;
                answerModel.UserId = answer.UserId;
                answerModel.IsPublic = answer.IsPublic;
                answerModel.MarkAsAnswer = answer.MarkAsAnswer;
                answerModel.ContentId = answer.ContentId;
                answerModel.CreatedOn = answer.CreatedOn;
                answerModel.likeCount = answer.likeCount;
                answerModel.LikeUsers = answer.LikeUsers;
            }
            return answerModel;
        }
        public  IList<AnswerModel> GetAnswersByCondition(string condition)
        {
            string sql = string.Format("SELECT [id],[contentId],[userId],[createdOn],[comment],[isPublic],markAsAnswer,likeCount,LikeUsers FROM [dbo].[uSomeForumReply] WHERE {0}", condition);
            return GetAnswers(sql);
        }
         IList<AnswerModel> GetAnswers(string sql)
        {
            var answerList = new List<AnswerModel>();
            var dr = _dataHelper.ExecuteReader(sql);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var answerModel = new AnswerModel
                    {
                        ID = dr.GetInt("ID"),
                        ContentId = dr.GetInt("contentId"),
                        UserId = dr.GetInt("userId"),
                        CreatedOn = dr.GetDateTime("createdOn"),
                        Comment = dr.GetString("comment"),
                        IsPublic = dr.GetBoolean("isPublic"),
                        MarkAsAnswer = dr.GetBoolean("markAsAnswer"),
                        likeCount = dr.GetInt("likeCount"),
                        LikeUsers = dr.GetString("LikeUsers")
                        
                    };
                    answerList.Add(answerModel);
                }
            }
            return answerList;
        }
    }
}