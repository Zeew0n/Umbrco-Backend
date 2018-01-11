using System;
using System.Collections.Generic;

namespace uSome.Utilities
{
    public class Treatment
    {
        private DataHelper _dataHelper = new DataHelper();

        public bool Save(TreatmentModel model)
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

        bool Add(TreatmentModel model)
        {
            try
            {
                var sql = string.Format(@"INSERT INTO [dbo].[uSomeTreatment]
                                        ([UserId],[TreatmentTypeId],[HospitalId],[TreatmentDate]) 
                                        VALUES ('{0}','{1}','{2}','{3}')",
                                        model.UserId, model.TreatmentTypeId, model.HospitalId, model.TreatmentDate);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on adding treatment ::" + ex.Message);
                return false;
            }
        }

        bool Update(TreatmentModel model)
        {
            try
            {
                var sql = string.Format("UPDATE [dbo].[uSomeTreatment] SET  [TreatmentTypeId] = '{0}', [HospitalId] = '{1}', [TreatmentDate] = '{2}' WHERE id={3}",
                                model.TreatmentTypeId, model.HospitalId, model.TreatmentDate, model.Id);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on updating treatment ::" + ex.Message);
                return false;
            }
        }

        bool CheckExisting(int id)
        {
            string sql = string.Format("SELECT COUNT(ID) FROM uSomeTreatment WHERE id ='{0}'", id);
            if (_dataHelper.ExecuteScalar(sql) > 0)
            {
                return true;
            }
            return false;
        }
        public bool Delete(int id)
        {
            try
            {
                string sql = string.Format("DELETE FROM [dbo].[uSomeTreatment] WHERE id = '{0}'", id);
                _dataHelper.ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorLog("Error on deleting treatment ::" + ex.Message);
                return false;
            }
        }
        public IList<TreatmentModel> GetTreatmentById(int id)
        {
            var sql = string.Format("SELECT [Id],[userId],[treatmentTypeId],[hospitalId],[treatmentDate],[createdOn] FROM [dbo].[uSomeTreatment] WHERE id = '{0}'", id);
            return GetTreatment(sql);
        }
        public IList<TreatmentModel> GetTreatmentsByUserId(int userId)
        {
            var sql = string.Format(@"SELECT [Id],[userId],[treatmentTypeId],[hospitalId],[treatmentDate],
                                    [createdOn] FROM [dbo].[uSomeTreatment] 
                                    WHERE userId = '{0}'", userId);
            return GetTreatment(sql);
        }
        private IList<TreatmentModel> GetTreatment(string sqlText)
        {
            var treatmentList = new List<TreatmentModel>();

            var dr = _dataHelper.ExecuteReader(sqlText);
            if (dr.HasRecords)
            {
                while (dr.Read())
                {
                    var treatment = new TreatmentModel();
                    treatment.Id = dr.GetInt("Id");
                    treatment.UserId = dr.GetInt("userId");
                    treatment.TreatmentTypeId = dr.GetInt("treatmentTypeId");
                    treatment.HospitalId = dr.GetInt("hospitalId");
                    treatment.TreatmentDate = dr.GetDateTime("treatmentDate");
                    treatment.CreatedOn = dr.GetDateTime("createdOn");
                    treatmentList.Add(treatment);
                }
            }
            return treatmentList;
        }

    }
}