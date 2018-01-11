using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class MasterBlog
    {
        private DataHelper _dataHelper = new DataHelper();

        public bool Save(MasterBlogModel blog)
        {
            if (CheckExisting(blog.ID))
            {
                return Update(blog);
            }
            else
            {
                return Add(blog);
            }
        }

        public MasterBlogModel GetMasterBlogById(int Id)
        {
            var model = new MasterBlogModel();
            var sql = "SELECT [id],[userId],[title],[image],[isPublic],[createdOn],[category],[description] FROM [dbo].[uSomeMasterBlog] WHERE Id ='" + Id + "'";
            foreach (var blog in GetMasterBlog(sql))
            {
                model.ID = Id;
                model.Image = blog.Image;
                model.IsPublic = blog.IsPublic;
                model.Title = blog.Title;
                model.CreatedOn = blog.CreatedOn;
                model.Category = blog.Category;
                model.UserId = blog.UserId;
                model.Description = blog.Description;
            }
            return model;
        }
        public MasterBlogModel GetMasterBlogByUserId(int userId)
        {
            var model = new MasterBlogModel();
            var sql = "SELECT [id],[userId],[title],[image],[isPublic],[createdOn],[category],[description] FROM [dbo].[uSomeMasterBlog] WHERE userId ='" + userId + "'";
            foreach (var blog in GetMasterBlog(sql))
            {
                model.ID = blog.ID;
                model.Image = blog.Image;
                model.IsPublic = blog.IsPublic;
                model.Title = blog.Title;
                model.CreatedOn = blog.CreatedOn;
                model.Category = blog.Category;
                model.UserId = blog.UserId;
                model.Description = blog.Description;
            }
            return model;
        }

        public IList<MasterBlogModel> GetPublishedBlogByUser(int userId)
        {
            var sql = "SELECT [id],[userId],[title],[image],[isPublic],[createdOn],[category],[description] FROM [dbo].[uSomeMasterBlog] WHERE [userId] = '" + userId + "'";
            return GetMasterBlog(sql);
        }

        public IList<MasterBlogModel> GetPrivateBlog()
        {
            var sql = "SELECT [id],[userId],[title],[image],[isPublic],[createdOn],[category],[description] FROM [dbo].[uSomeMasterBlog] WHERE [isPublic] = '0'";
            return GetMasterBlog(sql);
        }

        public IList<MasterBlogModel> GetPublicBlogs()
        {
            var sql = "SELECT [id],[userId],[title],[image],[isPublic],[createdOn],[category],[description] FROM [dbo].[uSomeMasterBlog] WHERE [isPublic] = '1'";
            return GetMasterBlog(sql);
        }

        public IList<MasterBlogModel> GetAllBlog()
        {
            var sql = "SELECT [id],[userId],[title],[image],[isPublic],[createdOn],[category],[description] FROM [dbo].[uSomeMasterBlog]";
            return GetMasterBlog(sql);
        }

        private IList<MasterBlogModel> GetMasterBlog(string sqlText)
        {
            var blogList = new List<MasterBlogModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var blog = new MasterBlogModel();
                    blog.ID = dr.GetInt("id");
                    blog.UserId = dr.GetInt("userId");
                    blog.Title = dr.GetString("title");
                    blog.Image = dr.GetString("image");
                    blog.IsPublic = dr.GetBoolean("isPublic");
                    blog.CreatedOn = dr.GetDateTime("createdOn");
                    blog.Category = dr.GetInt("category");
                    blog.Description = dr.GetString("description");
                    blogList.Add(blog);
                }
            }
            return blogList;
        }

        public int GetLatestMasterBlogId()
        {
            var sql = "SELECT [id] FROM [dbo].[uSomeMasterBlog] order by id desc";
            return _dataHelper.ExecuteScalar(sql);
        }
        bool Add(MasterBlogModel blogModel)
        {
            try
            {
                var sql = string.Format("INSERT INTO [dbo].[uSomeMasterBlog]([userId],[title],[image],[isPublic],[category],[description]) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')",
                                 blogModel.UserId, blogModel.Title, blogModel.Image, blogModel.IsPublic,blogModel.Category,blogModel.Description);

                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        bool Update(MasterBlogModel blogModel)
        {
            try
            {
                var sql = string.Format("UPDATE [dbo].[uSomeMasterBlog] SET [title] = '{0}', [image] = '{1}', [isPublic] = '{2}',[category] ='{3}',[description]='{4}' WHERE id={5}",
                                 blogModel.Title, blogModel.Image,blogModel.IsPublic,blogModel.Category,blogModel.Description,blogModel.ID);
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
                var sql = string.Format("UPDATE [dbo].[uSomeMasterBlog] SET [isPublic] = '0' WHERE id='{0}'", id);
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
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeMasterBlog WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
    }
}