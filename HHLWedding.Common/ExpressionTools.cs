using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.ToolsLibrary;
using System.Collections;
using HHLWedding.EditoerLibrary;

namespace HHLWedding.Common
{
    //中间层 引用数据层
    public class ExpressionTools
    {
        protected static Employee _empService = new Employee();

        #region 查询条件 lamada表达式(根据method而定)
        /// <summary>
        /// 查询条件 lamada表达式(method默认 Contains)
        /// </summary>
        /// <param name="IsLowLevel">是否包含显示下级</param>
        /// <param name="columnName">列名</param>
        public static void GetParsByCondition<T>(string columnName, List<Expression<Func<T, bool>>> parmList, string value = "", string methodInfo = "Contains", bool IsLowLevel = false, bool IsLookLowLevel = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(columnName))
                {

                    ParameterExpression param = Expression.Parameter(typeof(T), "c");
                    MethodInfo method = typeof(string).GetMethod(methodInfo);
                    MethodInfo strings = typeof(object).GetMethod("ToString", new Type[] { });
                    MemberExpression left = Expression.Property(param, typeof(T).GetProperty(columnName));

                    //右边表达式
                    ConstantExpression right = null;
                    if (IsLowLevel)         //涉及到Employee才会有下级
                    {
                        var myKey = value;
                        if (IsLookLowLevel)
                        {
                            _empService.GetMyManagerEmpLoyees(value.ToInt32(), ref myKey);
                            myKey = myKey.Substring(0, myKey.Length - 1);
                        }

                        right = Expression.Constant(myKey);
                    }
                    else
                    {
                        right = Expression.Constant(value);
                    }

                    MethodCallExpression filter = null;
                    if (methodInfo == "Contains")
                    {
                        //int类型转为string的表达式
                        Expression left1 = Expression.Call(left, strings);
                        filter = Expression.Call(right, method, left1);
                    }
                    else
                    {
                        filter = Expression.Call(left, method, right);
                    }

                    Expression<Func<T, bool>> pras = Expression.Lambda<Func<T, bool>>(filter, param);
                    parmList.Add(pras);
                }
            }
            catch (Exception e)
            {
                new Exception(e.Message);
            }
        }
        #endregion

        #region 查询条件 lamada表达式( ==   or)
        /// <summary>
        /// 查询条件 lamada表达式( ==   or)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName"></param>
        /// <param name="parmList"></param>
        public static void GetEqualPars<T>(string columnName, List<Expression<Func<T, bool>>> parmList, string value = "", string methodInfo = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {

                    ParameterExpression param = Expression.Parameter(typeof(T), "c");
                    MethodInfo method = null;
                    if (!string.IsNullOrEmpty(methodInfo))
                    {
                        method = typeof(string).GetMethod(methodInfo);
                    }
                    //转string类型
                    MethodInfo strings = typeof(object).GetMethod("ToString", new Type[] { });

                    string[] attrColumn = null;
                    PropertyInfo property = null;
                    MemberExpression left = null;
                    ////构造右表达式
                    ConstantExpression right2 = Expression.Constant(value);
                    Expression where2 = null;
                    Expression filterTmp = where2;

                    if (columnName.Contains("/"))
                    {
                        attrColumn = columnName.Split('/');
                        //获取字段(多字段)
                        for (int i = 0; i < attrColumn.Length; i++)
                        {
                            property = typeof(T).GetProperty(attrColumn[i]);

                            left = Expression.Property(param, property);
                            Expression left1 = Expression.Call(left, strings);

                            if (methodInfo == "Contains")       //模糊查询
                            {
                                if (i == 0)
                                {
                                    ////进行合并：例如:employeeid==登录员工ID
                                    where2 = Expression.Call(left1, method, right2);
                                    filterTmp = where2;
                                }
                                else
                                {
                                    ////进行合并：例如:employeeid==登录员工ID
                                    filterTmp = Expression.Call(left1, method, right2);
                                    where2 = Expression.Or(filterTmp, where2);
                                }
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    ////进行合并：例如:employeeid==登录员工ID
                                    where2 = Expression.Equal(left1, right2);
                                    filterTmp = where2;
                                }
                                else
                                {
                                    ////进行合并：例如:employeeid==登录员工ID
                                    filterTmp = Expression.Equal(left1, right2);
                                    where2 = Expression.Or(filterTmp, where2);
                                }
                            }
                        }
                    }
                    else
                    {

                        property = typeof(T).GetProperty(columnName);
                        left = Expression.Property(param, columnName);
                        ////构造右表达式登录员工ID
                        right2 = Expression.Constant(value);

                        if (methodInfo == "Contains")       //模糊查询
                        {
                            where2 = Expression.Call(left, method, right2);
                        }
                        else
                        {
                            ////进行合并：例如 employeeid==登录员工ID
                            Expression left1 = Expression.Call(left, strings);
                            where2 = Expression.Equal(left1, right2);

                        }
                    }

                    Expression<Func<T, bool>> pras = Expression.Lambda<Func<T, bool>>(where2, param);
                    parmList.Add(pras);
                }
            }
            catch (Exception e)
            {
                new Exception(e.Message);
            }

        }
        #endregion

    }

    public partial class PropModel
    {
        /// <summary>
        /// 字段名称属性
        /// </summary>
        public string property { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// lamada符号
        /// </summary>
        public string method { get; set; }
    }
}
