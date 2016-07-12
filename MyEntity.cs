using System;
using System.Collections.Generic;
using System.Linq;
using veritabanıdll;
using System.Data;
using System.Reflection;

namespace ConsoleApplication1.Entity
{
    public class Entity<T>
    {
        veritabanı db;
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
        public void SaveChanges()
        {
            foreach (var i in typeof(T).GetProperties())
            {
               dynamic r = i.GetValue(this);
               foreach (var w in r)
               {
                   foreach (var wq in w.GetType().GetProperties())
                   {
                       object dsf = wq.GetValue(w);
                   }
               }
            }
        }
        void InitDB()
        {
            db = new veritabanı();
            db.DbType = veritabanı.eDbType.SQLite;
            db.Connectionstring = "Data Source=dbwebrone.db" + ";Version=3;";
            db.Baglan();
        }
    }
}
