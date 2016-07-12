using System;
using System.Collections.Generic;
using System.Linq;
using Database; // This is my custom database dll for sqlite helper
using System.Data;
using System.Reflection;

namespace performORM.Entity
{
    public class Entity<T>
    {
        Database db;
        public List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }
        public Entity()
        {
            InitDB();
            foreach (var i in typeof(T).GetProperties())
            {
                MethodInfo method = GetType().GetMethod("ConvertToList").MakeGenericMethod(i.PropertyType.GenericTypeArguments);
                i.SetValue(this, method.Invoke(this, new object[] { db.tablogetir("select * from " + i.Name) }));
            }
        }
        void InitDB()
        {
            db = new Database();
            db.DbType = Database.DbType.SQLite;
            db.Connectionstring = "Data Source=sqlitedatabase.db" + ";Version=3;";
            db.Open();
        }
    }
}
