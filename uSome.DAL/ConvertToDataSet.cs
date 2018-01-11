using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using umbraco.DataLayer;

namespace uSome
{
    public class ConvertToDataSet
    {
        public DataSet FromIRecordReader(IRecordsReader recordsReader)
        {
            // Get the internal RawDataReader (obsolete)
            System.Data.SqlClient.SqlDataReader reader
                = ((umbraco.DataLayer.SqlHelpers.SqlServer.SqlServerDataReader)recordsReader).RawDataReader;
            DataSet dataSet = new DataSet();
            do
            {
                DataTable dataTable = new DataTable();
                DataTable schemaTable = reader.GetSchemaTable();

                if (schemaTable != null)
                {
                    // A query returning records was executed
                    foreach (DataRow dataRow in schemaTable.Rows)
                    {
                        // Create a column name that is unique in the data table
                        string columnName = (string)dataRow["ColumnName"];
                        // Add the column definition to the data table
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    // Fill the data table we just created
                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                            dataRow[i] = reader.GetValue(i);
                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records were returned, return number of rows affected
                    dataTable.Columns.Add(new DataColumn("RowsAffected"));
                    dataSet.Tables.Add(dataTable);
                    DataRow rowsAffectedRow = dataTable.NewRow();
                    rowsAffectedRow[0] = reader.RecordsAffected;
                    dataTable.Rows.Add(rowsAffectedRow);
                }
            } // Go trough all result sets
            while (reader.NextResult());

            // Close the data reader so the underlying connection is closed
            recordsReader.Close();

            return dataSet;
        }

        public DataTable LinqToDataTable<T>(IEnumerable<T> linqList)
        {
            var dtReturn = new DataTable();
            PropertyInfo[] columnNameList = null;

            if (linqList == null) return dtReturn;

            foreach (T t in linqList)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (columnNameList == null)
                {
                    columnNameList = t.GetType().GetProperties();

                    foreach (PropertyInfo columnName in columnNameList)
                    {
                        Type columnType = columnName.PropertyType;

                        if ((columnType.IsGenericType) && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            columnType = columnType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(columnName.Name, columnType));
                    }
                }

                DataRow dataRow = dtReturn.NewRow();

                foreach (PropertyInfo columnName in columnNameList)
                {
                    dataRow[columnName.Name] =
                        columnName.GetValue(t, null) == null ? DBNull.Value : columnName.GetValue(t, null);

                }

                dtReturn.Rows.Add(dataRow);
            }

            return dtReturn;
        }

        //Convert list to datatable
        public static DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            var table = new DataTable();

            // Get max columns.
            int columns = list.Select(array => array.Length).Concat(new[] { 0 }).Max();

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }

            // Add rows.
            foreach (object[] array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }

        //Convert Generic List to Datatable
        public static DataTable GenericListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();
            if (list.Count > 0)
            {
                Type listType = list.ElementAt(0).GetType();
                //Get element properties and add datatable columns  
                PropertyInfo[] properties = listType.GetProperties();
                foreach (PropertyInfo property in properties)
                    dt.Columns.Add(new DataColumn() { ColumnName = property.Name });
                foreach (object item in list)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn col in dt.Columns)
                        dr[col] = listType.GetProperty(col.ColumnName).GetValue(item, null);
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}