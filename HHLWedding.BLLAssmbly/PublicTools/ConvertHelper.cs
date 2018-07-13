using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace HHLWedding.BLLAssmbly
{
    public class ConvertHelper
    {

        #region ToList 把DataTable转换成List
        /// <summary>
        /// DataTable To List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dt) where T : new()
        {
            // 定义集合
            List<T> ts = new List<T>();

            // 获得此模型的类型
            Type type = typeof(T);

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            try
                            {
                                //if ((value.GetType() == typeof(Int64) && pi.PropertyType == typeof(string))|| (value.GetType() == typeof(Int32) && pi.PropertyType == typeof(string)))
                                //    pi.SetValue(t, value.ToString(), null);
                                //else 
                                //if (value.GetType() == typeof(decimal))
                                //{
                                //    pi.SetValue(t, value, null);
                                //}
                                //else 
                                if (pi.PropertyType == typeof(System.Decimal) || pi.PropertyType == typeof(decimal))
                                {
                                    pi.SetValue(t, decimal.Parse(value.ToString()), null);
                                }
                                else if (pi.PropertyType == typeof(float))
                                {
                                    pi.SetValue(t, float.Parse(value.ToString()), null);
                                }
                                else if (pi.PropertyType == typeof(System.Double))
                                {
                                    pi.SetValue(t, double.Parse(value.ToString()), null);
                                }
                                else if (pi.PropertyType == typeof(System.DateTime))
                                {
                                    pi.SetValue(t, DateTime.Parse(value.ToString()), null);
                                }
                                else if ((pi.PropertyType == typeof(int)) || (pi.PropertyType == typeof(System.Int32)))
                                {
                                    pi.SetValue(t, int.Parse(value.ToString()), null);
                                }
                                else if (pi.PropertyType == typeof(System.Int16))
                                {
                                    pi.SetValue(t, Int16.Parse(value.ToString()), null);
                                }
                                else if (pi.PropertyType == typeof(System.Int64))
                                {
                                    pi.SetValue(t, Int64.Parse(value.ToString()), null);
                                }
                                else if (pi.PropertyType == typeof(System.String))
                                {
                                    pi.SetValue(t, value.ToString(), null);
                                }
                                else
                                {
                                    pi.SetValue(t, value, null);
                                }

                            }
                            catch
                            { }

                        }
                    }
                }

                ts.Add(t);
            }

            return ts;

        }
        #endregion

        #region ToDataTable 将泛型集合类转换成DataTable
        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(List<T> list) where T : new()
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (!result.Columns.Contains(pi.Name))
                        result.Columns.Add(pi.Name, pi.PropertyType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (result.Columns.Contains(pi.Name))
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        #endregion

        #region ToListDictionary 把DataTable转换成 Dictionary
        /// <summary>
        /// dataTable转化为list<dictionary<string,string>>以供后面建立动态实体
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> ToListDictionary(DataTable dt)
        {
            List<Dictionary<string, string>> lst = new List<Dictionary<string, string>>();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dic[dt.Columns[i].ColumnName] = row[i].ToString();
                }
                lst.Add(dic);
            }
            return lst;
        }
        #endregion


        public static List<Dictionary<string, object>> ToListDictionary<T>(List<T> list)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (T t in list)
            {
                PropertyInfo[] propertys = t.GetType().GetProperties();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (PropertyInfo p in propertys)
                {
                    dic[p.Name] = p.GetValue(t, null);
                }
                if (dic.Keys.Count > 0)
                    result.Add(dic);
            }
            return result;
        }
    }
}


