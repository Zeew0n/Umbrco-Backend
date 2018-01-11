using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class BlogFollowers
    {
        private DataHelper _dataHelper = new DataHelper();
        public bool Save(BlogFollowersModel model)
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

        private bool Add(BlogFollowersModel model)
        {
            try
            {
                string sql = string.Format(@"INSERT INTO [dbo].[uSomeBlogFollowers] ([blogId] ,[userId]) VALUES ('{0}','{1}')", model.BlogId, model.UserId);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch(Exception ex)
            {
                Log.ErrorLog("Error on adding blog followers ::" + ex.Message);
                return false;
            }
           
        }

        private bool Update(BlogFollowersModel model)
        {
            try
            {
                string sql = string.Format("UPDATE [dbo].[uSomeBlogFollowers] SET [follow] = '{0}' WHERE id='{1}' and blogId ='{1}' and userId ='{2}'",model.Follow,model.ID,model.BlogId,model.UserId);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on updating blog followers ::" + ex.Message);
                return false;
            }
           
        }
        public bool Delete(BlogFollowersModel model)
        {
            try
            {
                string sql = string.Format(@"DELETE FROM [dbo].[uSomeBlogFollowers] WHERE (blogId='{0}' and userId='{1}')", model.BlogId, model.UserId);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on deleting blog followers ::" + ex.Message);
                return false;
            }

        }
        bool CheckExisting(int id)
        {
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeBlogFollowers WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
        public IList<BlogFollowersModel> GetFollowers(string condition)
        {
            var sql = string.Format("SELECT [ID],[blogId],[userId],[followedDate],[follow] FROM [dbo].[uSomeBlogFollowers] WHERE {0}", condition);
            return GetBlogFollowers(sql);
        }

        public IList<BlogFollowersModel> GetFollowers()
        {
            var sql = "SELECT [ID],[blogId],[userId],[followedDate],[follow] FROM [dbo].[uSomeBlogFollowers]";
            return GetBlogFollowers(sql);
        }

        private IList<BlogFollowersModel> GetBlogFollowers(string sqlText)
        {
            var followerList = new List<BlogFollowersModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var follower = new BlogFollowersModel();
                    follower.ID = dr.GetInt("id");
                    follower.UserId = dr.GetInt("userId");
                    follower.BlogId = dr.GetInt("blogId");
                    follower.Follow = dr.GetBoolean("follow");
                    follower.FollowedDate = dr.GetDateTime("followedDate");
                    followerList.Add(follower);
                }
            }
            return followerList;
        }

    }
}