using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class Posts
    {
        private DataHelper _dataHelper = new DataHelper();

        public bool save(PostModel model,out int rowAffected)
        {
            if(CheckExisting(model.Id))
            {
                return Update(model, out rowAffected);
            }
            else
            {
                return Add(model, out rowAffected);
            }
        }
        private bool Add(PostModel model,out int rowAffected)
        {
            try
            {
                var sql = string.Format(@"INSERT INTO [dbo].[uSomePosts]([FromUserId],[ToUserId],[MessageBody])
                                    VALUES('{0}','{1}','{2}')", model.FromUserId, model.ToUserId, model.MessageBody);
               rowAffected= _dataHelper.ExecuteScalar(sql);
                return true;
            }
            catch (Exception ex)
            {
                rowAffected = 0;
                return false;
            }

        }
        bool Update(PostModel model, out int recentId)
        {
            recentId = model.Id;
            try
            {
                //var sql = string.Format(@"UPDATE [dbo].[uSomeForumReply] SET comment ='{0}', isPublic='{1}',markAsAnswer='{2}' WHERE ID ='{3}'", model.Comment, model.IsPublic, model.MarkAsAnswer, model.ID);
                //_dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        bool CheckExisting(int id)
        {
            string sql = string.Format("SELECT COUNT(ID) FROM uSomePosts WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
        public IList<PostModel> GetAllMessageWithCondition(string condition)
        {
            var sql = string.Format("SELECT [Id],[FromUserId]"+
                                    ",[ToUserId] ,[MessageBody] ,[SendDate] ,[ReadFlag],AppropriateFlag"+
                                     " FROM [DepnetCommunity].[dbo].[uSomePosts]  WHERE {0}", condition);
     
            return GetMessage(sql);
        }
        private IList<PostModel> GetMessage(string sqlText)
        {
            var contactList = new List<PostModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var contact = new PostModel();
                    contact.Id = dr.GetInt("Id");
                    contact.FromUserId = dr.GetInt("FromUserId");
                    contact.ToUserId = dr.GetInt("ToUserId");
                    contact.MessageBody = dr.GetString("MessageBody");
                    contact.SendDate = dr.GetDateTime("SendDate");
                    contact.ReadFlag = dr.GetBoolean("ReadFlag");
                    contact.AppropriateFlag = dr.GetBoolean("AppropriateFlag");
                    contactList.Add(contact);
                }
            }
            return contactList;
        }
        public IList<MemberListModel> GetAllContacts(string condition)
        {
            var sql = string.Format("  select c.requestFrom,c.requestTo,m.Email,c.[requestConfirmed] from [uSomeMemberContacts] as c inner join [cmsMember] as m on m.nodeId=c.requestTo  WHERE {0}", condition);
            return GetMemberContacts(sql);
        }

        private IList<MemberListModel> GetMemberContacts(string sqlText)
        {
            var contactList = new List<MemberListModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var contact = new MemberListModel();
                    contact.FromUserId = dr.GetInt("requestFrom");
                    
                    contact.ToUserId = dr.GetInt("requestTo");
                    contact.ToEmailId = dr.GetString("Email");
                    contact.IsConfirmed = dr.GetBoolean("requestConfirmed");
                   
                    contactList.Add(contact);
                }
            }
            return contactList;
        }
    }
}