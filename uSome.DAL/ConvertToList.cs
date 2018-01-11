using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace uSome
{
    public class ConvertToList
    {
        public List<T> ToList<T>(DataTable table)
        {
            List<T> list = new List<T>();

            T item;
            Type listItemType = typeof(T);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                item = (T)Activator.CreateInstance(listItemType);
                mapRow(item, table, listItemType, i);
                list.Add(item);
            }
            return list;
        }

        private void mapRow(object vOb, System.Data.DataTable table, Type type, int row)
        {
            for (int col = 0; col < table.Columns.Count; col++)
            {
                var columnName = table.Columns[col].ColumnName;
                var prop = type.GetProperty(columnName);
                object data = table.Rows[row][col];
                prop.SetValue(vOb, data, null);
            }
        }
    }
}