using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.DataLayer;

namespace uSome
{
    public class DataHelper
    {
        private static ISqlHelper SqlHelper
        {
            get { return umbraco.BusinessLogic.Application.SqlHelper; }
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commandText">Transact-SQL statement or stored procedure to execute at the data source.</param>
        /// <returns>The number of rows affected</returns>
        public int ExecuteNonQuery(string commandText)
        {
            return SqlHelper.ExecuteNonQuery(commandText);
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <param name="commantText">Transact-SQL statement or stored procedure to execute at the data source.</param>
        /// <param name="parameters">Collection of the IParameter</param>
        /// <returns>The number of rows affected</returns>
        public int ExecuteNonQuery(string commandText, IParameter[] parameters)
        {
            return SqlHelper.ExecuteNonQuery(commandText, parameters);
        }

        /// <summary>
        /// Sends the CommandText to the Connection and builds a IrecordsReader
        /// </summary>
        /// <param name="commandText">Transact-SQL statement or stored procedure to execute at the data source.</param>
        /// <returns>IRecordsReader</returns> 
        public IRecordsReader ExecuteReader(string commandText)
        {
            return SqlHelper.ExecuteReader(commandText);
        }

        public int ExecuteScalar(string commandText)
        {
            return SqlHelper.ExecuteScalar<int>(commandText);
        }

        /// <summary>
        /// Creates a new instance of a IParameter object
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="value">Value that belongs to the given parameter.</param>
        /// <returns>IParameter</returns>
        public IParameter CreateParameter(string parameterName, object value)
        {
            return SqlHelper.CreateParameter(parameterName, value);
        }
    }
}