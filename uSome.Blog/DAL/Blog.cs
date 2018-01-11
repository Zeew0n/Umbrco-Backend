using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class Blog
    {
        private DataHelper _dataHelper = new DataHelper();

        public bool Save(BlogModel blog)
        {
            if (CheckExisting(blog.Id))
            {
                return Update(blog);
            }
            else
            {
                return Add(blog);
            }
        }

        public IList<BlogModel> GetPublishedBlogByUser(int userId, int parentId)
        {
            var sql = "SELECT [id],[userId],[title],[content],[isPublic],[createdOn],[isPublish],[parentId] FROM [dbo].[uSomeBlog] WHERE isPublish = '1' and [isPrivate] = '1'";
            return GetBlog(sql);
        }

        public IList<BlogModel> GetPrivateBlog(int parentId)
        {
            var sql = "SELECT [id],[userId],[title],[content],[isPublic],[createdOn],[isPublish],[parentId] FROM [dbo].[uSomeBlog] WHERE isPublish = '1' and [isPublic] = '0'";
            return GetBlog(sql);
        }

        public IList<BlogModel> GetPublicBlogs(int parentId)
        {
            var sql = "SELECT [id],[userId],[title],[content],[isPublic],[createdOn],[isPublish],[parentId] FROM [dbo].[uSomeBlog] WHERE isPublish = '1' and [isPublic] = '1'";
            return GetBlog(sql);
        }

        public IList<BlogModel> GetAllBlog(string condition)
        {
            var sql = string.Format("SELECT [id],[userId],[title],[content],[isPublic],[createdOn],[isPublish],[parentId] FROM [dbo].[uSomeBlog] WHERE {0}",condition);
            return GetBlog(sql);
        }
        public IList<BlogModel> GetAllBlog()
        {
            var sql = "SELECT [id],[userId],[title],[content],[isPublic],[createdOn],[isPublish],[parentId] FROM [dbo].[uSomeBlog]";
            return GetBlog(sql);
        }
        public BlogModel GetBlogById(string blogId)
        {
            var model = new BlogModel();
            var sql = "SELECT [id],[userId],[title],[content],[isPublic],[createdOn],[isPublish],[parentId] FROM [dbo].[uSomeBlog] WHERE id ='" + blogId + "'";
            foreach (var blog in GetBlog(sql))
            {

                model.Id = blog.Id;
                model.UserId = blog.UserId;
                model.Title = blog.Title;
                model.ParentId = blog.ParentId;
                model.Content = blog.Content;
                model.IsPublic = blog.IsPublic;
                model.IsPublished = blog.IsPublished;
                model.CreatedOn = blog.CreatedOn;
            }
            return model;
        }
        public int GetLatestBlogId()
        {
            var sql = "SELECT [id] FROM [dbo].[uSomeBlog] order by id desc";
            return _dataHelper.ExecuteScalar(sql);
        }
        private IList<BlogModel> GetBlog(string sqlText)
        {
            var blogList = new List<BlogModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var blog = new BlogModel();
                    blog.Id = dr.GetInt("id");
                    blog.UserId = dr.GetInt("userId");
                    blog.Title = dr.GetString("title");
                    blog.ParentId = dr.GetInt("parentId");
                    blog.Content = dr.GetString("content");
                    blog.IsPublic = dr.GetBoolean("isPublic");
                    blog.IsPublished = dr.GetBoolean("isPublish");
                    blog.CreatedOn = dr.GetDateTime("createdOn");

                    blogList.Add(blog);
                }
            }
            return blogList;
        }


        bool Add(BlogModel blogModel)
        {
            try
            {
                var sql = string.Format("INSERT INTO [dbo].[uSomeBlog]([userId],[title],[parentId],[content],[isPublic],[isPublish]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                 blogModel.UserId, blogModel.Title, blogModel.ParentId, blogModel.Content, blogModel.IsPublic, blogModel.IsPublished);

                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        bool Update(BlogModel blogModel)
        {
            try
            {
                var sql = string.Format("UPDATE [dbo].[uSomeBlog] SET [title] = '{0}', [parentId] = '{1}', [content] = '{2}', [isPublic] = '{3}', [isPublish] = '{4}' WHERE id={5}",
                                 blogModel.Title, blogModel.ParentId, blogModel.Content, blogModel.IsPublic, blogModel.IsPublished, blogModel.Id);
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
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeBlog WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
    }
}