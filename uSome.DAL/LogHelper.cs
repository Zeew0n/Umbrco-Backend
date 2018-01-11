using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class LogHelper
    {
        private DataHelper _dataHelper = new DataHelper();
        public void Save(LogModel model)
        {
            var sql = string.Format(@"INSERT INTO [dbo].[uSomeLog]([userId],[NodeId],[logHeader],[logComment],[tableName])
                                    VALUES('{0}', '{1}', '{2}','{3}','{4}')", 
                                   model.UserId, model.NodeId, model.LogHeader, model.LogComment, model.TableName);
            _dataHelper.ExecuteNonQuery(sql);
        }
        public IList<LogModel> GetLogs(string condition)
        {
            var sql = string.Format("SELECT [id],[userId],[NodeId],[Datestamp],[logHeader],[logComment],[tableName] FROM [dbo].[uSomeLog] WHERE {0}", condition);
            return Logs(sql);
        }

        public IList<LogModel> GetLogs()
        {
            var sql = "SELECT [id],[userId],[NodeId],[Datestamp],[logHeader],[logComment],[tableName] FROM [dbo].[uSomeLog]";
            return Logs(sql);
        }

        private IList<LogModel> Logs(string sqlText)
        {
            var logList = new List<LogModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var log = new LogModel();
                    log.Id = dr.GetInt("id");
                    log.UserId = dr.GetInt("userId");
                    log.NodeId = dr.GetInt("nodeId");
                    log.Datestamp = dr.GetDateTime("datestamp");
                    log.LogHeader = dr.GetString("logheader");
                    log.LogComment = dr.GetString("logcomment");
                    log.TableName = dr.GetString("tableName");
                    logList.Add(log);
                }
            }
            return logList;
        }
    }
    public class LogModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NodeId { get; set; }
        public DateTime Datestamp { get; set; }
        public string LogHeader { get; set; }
        public string LogComment { get; set; }
        public string TableName { get; set; }

    }
}