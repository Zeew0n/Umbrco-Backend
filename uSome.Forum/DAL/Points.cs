using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSome
{
    public class Points
    {
        private DataHelper _dataHelper = new DataHelper();
        public void Save(PointsModel model)
        {
            if (CheckExisting(model.ID))
            {
                Update(model);
            }
            else
            {
                Add(model);
            }
        }

        private void Update(PointsModel model)
        {
            try
            {
                string sql = string.Format("UPDATE [dbo].[uSomeForumPoints] SET [Question] = '{0}',[Answer] = '{1}',[MarkAsAnwer] = '{2}',[HiFive] = '{3}' WHERE ID='{4}'",
                                      model.Question, model.Answer, model.MarkAsAnwer, model.HiFive, model.ID);
                _dataHelper.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on updating points :: " + ex.Message);
            }
        }

        private void Add(PointsModel model)
        {
            try
            {
                string sql = string.Format("INSERT INTO [dbo].[uSomeForumPoints] ([Question],[Answer],[MarkAsAnwer],[HiFive]) VALUES  ('{0}','{1}','{2}','{3}')",
                                      model.Question, model.Answer, model.MarkAsAnwer, model.HiFive);
                _dataHelper.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on adding points :: " + ex.Message);
            }
            
        }
        bool CheckExisting(int id)
        {
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeForumPoints WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
        public PointsModel GetPointsModel()
        {
            var points = GetPoints();
            var pointsModel = new PointsModel();
            foreach (var point in points)
            {
                pointsModel.ID = point.ID;
                pointsModel.Question=point.Question;
                pointsModel.Answer = point.Answer;
                pointsModel.MarkAsAnwer = point.MarkAsAnwer;
                pointsModel.HiFive = point.HiFive;
            }
            return pointsModel;
        }
        public IList<PointsModel> GetPoints()
        {
            string sql = string.Format("SELECT [ID],[Question],[Answer],[MarkAsAnwer],[HiFive] FROM [dbo].[uSomeForumPoints]");
            return GetPoints(sql);
        }
        IList<PointsModel> GetPoints(string sql)
        {
            var points = new List<PointsModel>();
            var dr = _dataHelper.ExecuteReader(sql);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var pointsModel = new PointsModel
                    {
                        ID = dr.GetInt("ID"),
                        Question = dr.GetInt("Question"),
                        Answer = dr.GetInt("Answer"),
                        MarkAsAnwer = dr.GetInt("MarkAsAnwer"),
                        HiFive = dr.GetInt("HiFive"),
                    };
                    points.Add(pointsModel);
                }
            }
            return points;
        }
        
    }
}