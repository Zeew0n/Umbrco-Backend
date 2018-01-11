using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uSome.Membership.Models;
namespace uSome
{
    public class MemberContacts
    {
        private DataHelper _dataHelper = new DataHelper();
        public bool Save(MemberContactsModel model)
        {
               var logModel = new LogModel
                {
                    NodeId = model.RequestTo,
                    UserId = model.RequestFrom,
                    LogHeader ="Member Contact",
                    TableName = "uSomeMemberContacts"
                };
                
            if (CheckExisting(model.ID))
            {
                logModel.LogComment=  "Send Contact Request";
                return Update(model);
            }
            else
            {
                 logModel.LogComment=  "Accept Contact Request";
                return Add(model);
            }
             new LogHelper().Save(logModel);
        }

        private bool Add(MemberContactsModel model)
        {
            try
            {
                var sql = string.Format(@"INSERT INTO [dbo].[uSomeMemberContacts]([requestFrom],[requestTo],[requestMessage],[requestDate],[requestConfirmed],[confirmDate])
                                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}')", model.RequestFrom, model.RequestTo, model.RequestMessage, model.RequestDate,model.RequestConfirmed, model.ConfirmDate);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        private bool Update(MemberContactsModel model)
        {
            try
            {
                var sql = string.Format(@"UPDATE [dbo].[uSomeMemberContacts]  
                                SET 
                                [requestFrom] = '{0}',[requestTo] = '{1}' , [requestConfirmed] = '{2}',[requestMessage] = '{3}' ,[confirmDate] = '{4}' WHERE id='{5}'",
                                               model.RequestFrom, model.RequestTo, model.RequestConfirmed, model.RequestMessage, model.ConfirmDate, model.ID);
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
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeMemberContacts WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
        public MemberContactsModel GetMemberContact(string condition)
        {
            var model = new MemberContactsModel();
            var sql = string.Format("SELECT  [ID],[requestFrom],[requestTo],[requestConfirmed],[requestMessage],[requestDate],[confirmDate] FROM [uSomeMemberContacts] WHERE {0}", condition);
            foreach (var contact in GetMemberContacts(sql))
            {
                model.ID = contact.ID;
                model.RequestFrom =contact.RequestFrom;
                model.RequestTo = contact.RequestTo;
                model.RequestConfirmed =  contact.RequestConfirmed;
                model.RequestDate = contact.RequestDate;
                model.ConfirmDate =  contact.ConfirmDate;
                model.RequestMessage = contact.RequestMessage;
            }
            return model;
        }
        public IList<MemberContactsModel> GetContacts(string condition)
        {
            var sql = string.Format("SELECT  [ID],[requestFrom],[requestTo],[requestConfirmed],[requestMessage],[requestDate],[confirmDate] FROM [uSomeMemberContacts] WHERE {0}", condition);
            return GetMemberContacts(sql);
        }
        public IList<MemberContactsModel> GetContacts()
        {
            var sql = "SELECT  [ID],[requestFrom],[requestTo],[requestConfirmed],[requestMessage],[requestDate],[confirmDate] FROM [uSomeMemberContacts]";
            return GetMemberContacts(sql);
        }
        private IList<MemberContactsModel> GetMemberContacts(string sqlText)
        {
            var contactList = new List<MemberContactsModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var contact = new MemberContactsModel();
                    contact.ID = dr.GetInt("id");
                    contact.RequestFrom = dr.GetInt("requestFrom");
                    contact.RequestTo = dr.GetInt("requestTo");
                    contact.RequestConfirmed = dr.GetObject("requestConfirmed");
                    contact.RequestDate = dr.GetObject("requestDate");
                    contact.ConfirmDate =dr.GetObject("confirmDate");
                    contact.RequestMessage = dr.GetString("requestMessage");
                    contactList.Add(contact);
                }
            }
            return contactList;
        }
    }
}