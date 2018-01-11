using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class BlogComment
    {
        private DataHelper _dataHelper = new DataHelper();

        public bool Save(CommentModel model)
        {
            if (CheckExisting(model.Id))
            {
                return Update(model);
            }
            else
            {
                return Add(model);
            }
        }

        public IList<CommentModel> GetBlogComment(string condition)
        {
            var sql = string.Format("SELECT [id],[contentId],[userId],[comment],[isPublic],[createdOn] FROM [dbo].[uSomeBlogComment] WHERE {0}",condition);
            return GetBlog(sql);
        }

        public IList<CommentModel> GetBlogComment()
        {
            var sql = "SELECT [id],[contentId],[userId],[comment],[isPublic],[createdOn] FROM [dbo].[uSomeBlogComment]";
            return GetBlog(sql);
        }

        private IList<CommentModel> GetBlog(string sqlText)
        {
            var commentList = new List<CommentModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var comment = new CommentModel();
                    comment.Id = dr.GetInt("id");
                    comment.UserId = dr.GetInt("userId");
                    comment.Comment = dr.GetString("comment");
                    comment.IsPublic = dr.GetBoolean("isPublic");
                    comment.CreatedOn = dr.GetDateTime("createdOn");
                    commentList.Add(comment);
                }
            }
            return commentList;
        }


        bool Add(CommentModel model)
        {
            try
            {
                var sql = string.Format("INSERT INTO [dbo].[uSomeBlogComment] ([contentId],[userId],[comment],[isPublic]) VALUES('{0}','{1}','{2}','{3}')",
                                 model.ContentId, model.UserId, model.Comment,model.IsPublic);

                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        bool Update(CommentModel model)
        {
            try
            {
                var sql = string.Format("UPDATE [dbo].[uSomeBlog] SET [comment]='{0}', [isPublic] = '{1}' WHERE id={2}",
                                model.Comment, model.IsPublic,model.Id);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var sql = string.Format("UPDATE [dbo].[uSomeBlog] SET [isPublish] = '0' WHERE id='{0}'", id);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                //uPollUtilities.CreateErrorLogMessage("Error in deleting poll due to " + ex.Message);
                return false;
            }
        }


        bool CheckExisting(int id)
        {
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeBlogComment WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
    }
}