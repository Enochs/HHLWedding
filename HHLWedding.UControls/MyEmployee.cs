using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using System.Reflection;
using System.Collections;

namespace HHLWedding.Control
{
    public class MyEmployee : DropDownList
    {
        //BaseService<Sys_Employee> _empService = new BaseService<Sys_Employee>();

        Employee _empService = new Employee();

        public MyEmployee()
        {
            this.DataSource = _empService.GetMyManagerEmpLoyee(LoginInfo.UserInfo.EmployeeId);
            this.DataTextField = "EmployeeName";
            this.DataValueField = "EmployeeID";
            this.DataBind();
            this.CssClass = "chosen-select form-control";
        }

        public void DataBinder(string text = "请选择")
        {
            this.Items.Insert(0, new ListItem { Value = "-1", Text = text });

        }

        #region 获取我的所有下级客户

        /// <summary>
        /// 获取我的所有下级客户Id   拼接客户Id
        /// </summary>
        /// <returns></returns>
        public string GetMyEmpKey(bool isLookLow = false)
        {
            List<int?> ObjKeyList = new List<int?>();
            string employee = "";

            if (this.SelectedValue.ToInt32() > 0 && isLookLow == false)
            {
                employee = this.SelectedValue.ToString() + ",";
            }
            else
            {

                _empService.GetMyManagerEmpLoyees(LoginInfo.UserInfo.EmployeeId.ToString().ToInt32(), ref employee);
            }

            return employee.Substring(0, employee.Length - 1);
        }

        /// <summary>
        /// 获取我的所有下级客户Id 返回List
        /// </summary>
        /// <returns></returns>
        public List<string> GetMyEmpKeyList()
        {
            List<string> ObjKeyList = new List<string>();

            if (this.SelectedValue.ToInt32() > 0)
            {
                ObjKeyList = _empService.GetMyEmployeeKey(this.SelectedValue.ToInt32());
            }
            else
            {
                ObjKeyList.Add(LoginInfo.UserInfo.EmployeeId.ToString());
            }
            return ObjKeyList;
        }

        #endregion

        #region 获取我管理的人员
        /// <summary>
        /// 获取我管理的人员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName">列名</param>
        public void GetMyEmployee<T>(string columnName, List<Expression<Func<T, bool>>> parmList, bool isLookLow = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(columnName))
                {
                    string myEmployee = GetMyEmpKey(isLookLow);

                    ParameterExpression param = Expression.Parameter(typeof(T), "c");
                    MethodInfo method = typeof(string).GetMethod("Contains");
                    MethodInfo strings = typeof(object).GetMethod("ToString", new Type[] { });
                    MemberExpression right1 = Expression.Property(param, typeof(T).GetProperty(columnName));
                    Expression right = Expression.Call(right1, strings);
                    ConstantExpression left = Expression.Constant(myEmployee);

                    MethodCallExpression filter = Expression.Call(left, method, right);

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

        #region 传入list 获取我管理的人员
        /// <summary>
        /// 传入list 获取我管理的人员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columnName"></param>
        /// <param name="parmList"></param>
        public void GetMyEmployeeList<T>(string columnName, List<Expression<Func<T, bool>>> parmList)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "c");
            PropertyInfo property = typeof(T).GetProperty(columnName);

            //左表达式
            MemberExpression lefts = Expression.Property(param, property);

            //转string类型
            MethodInfo strings = typeof(object).GetMethod("ToString", new Type[] { });
            Expression left =Expression.Call(lefts, strings);
            //构造右表达式登录员工ID
            ConstantExpression right2 = Expression.Constant(LoginInfo.UserInfo.EmployeeId.ToString());
            ////进行合并：employeeid==登录员工ID
            Expression where2 = Expression.Equal(left, right2);

            //等式右边的值
            List<string> MyKeyList = GetMyEmpKeyList();

            Expression filterTmp = where2;

            int index = 0;
            foreach (var itemValue in MyKeyList as IEnumerable ?? Enumerable.Empty<object>())
            {
                index++;
                right2 = Expression.Constant(itemValue);

                if (MyKeyList.Count >= 2)
                {

                    if (typeof(string) == property.PropertyType)
                    {
                        if (index == 1)
                        {
                            where2 = Expression.Equal(left, right2);
                        }
                        else
                        {
                            filterTmp = Expression.Equal(left, right2);
                        }
                    }
                    else
                    {
                        filterTmp = Expression.Equal(left, right2);
                    }

                    if (index >= 2)
                    {
                        where2 = Expression.Or(filterTmp, where2);
                    }
                }
                else        //只有一个人(没有下级或者下级为空)
                {
                    where2 = Expression.Equal(left, right2);
                }
            }

            Expression<Func<T, bool>> pras = Expression.Lambda<Func<T, bool>>(where2, param);
            parmList.Add(pras);

        }
        #endregion
    }
}
