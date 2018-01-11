using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace uSome
{
    public class MemberDetails
    {
        private DataHelper _dataHelper = new DataHelper();
        public IList<FollowedItemModel> GetFollowedItems(string condition)
        {
            var sql = string.Format("SELECT [ID],[NodeId],[UserId],[FollowedDate],[IsFollowed],[Type] FROM [dbo].[vw_ItemFollowed] WHERE {0} ORDER BY [FollowedDate] DESC", condition);
            return GetItems(sql);
        }
        public IList<FollowedItemModel> GetFollowedItems()
        {
            var sql = string.Format("SELECT [ID],[NodeId],[UserId],[FollowedDate],[IsFollowed],[Type] FROM [dbo].[vw_ItemFollowed] ORDER BY [FollowedDate] DESC");
           return GetItems(sql);
        }
        private IList<FollowedItemModel> GetItems(string sqlText)
        {
            var itemList = new List<FollowedItemModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var item = new FollowedItemModel();
                    item.ID = dr.GetInt("id");
                    item.UserId = dr.GetInt("userId");
                    item.NodeId = dr.GetInt("nodeId");
                    item.IsFollow = dr.GetBoolean("isFollowed");
                    item.Type = dr.GetString("type");
                    item.FollowedDate = dr.GetDateTime("FollowedDate");
                    itemList.Add(item);
                }
            }
            return itemList;
        }

    }
}