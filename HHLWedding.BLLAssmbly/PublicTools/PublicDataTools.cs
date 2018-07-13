using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.DataAssmblly;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.BLLAssmbly
{
    public static class PublicDataTools<T> where T : class
    {

        #region 根据条件 分页查询
        //  GetDataByPower(T ObjEntity, int? EmployeeID, int? ChannelID, ObjectParameter[] ObjParameterList)
        /// <summary>
        /// 根据复杂条件获取数据
        /// </summary>
        /// <param name="ObjEntity">需要的实体</param>
        /// <param name="ObjParameterList">参数集合</param>
        ///OrderByCloumname排序字段
        /// <returns></returns>
        public static List<T> GetDataByWhereParameter(List<PMSParameters> ObjParameterList, string OrderByCloumname, int PageSize, int PageIndex, out int SourceCount, int Sort = 1)
        {
            string WhereStr = GetWhere(ObjParameterList);

            string CountSql = " Select count(*) from " + typeof(T).Name + " as c where 1=1 " + WhereStr.Replace("{", "(").Replace("}", ")").Replace("System.Boolean", "bit").Replace("False", "'0'").Replace("True", "'1'");

            //CountSql = CountSql.Replace("System.DateTime", "date");
            CountSql = CountSql.Replace("cast(", "").Replace("as System.DateTime)", "");
            HHL_WeddingEntities ObjContex = new HHL_WeddingEntities();
            var SourctcountResualt = ObjContex.Database.SqlQuery<int>(CountSql, new List<ObjectParameter>().ToArray()).First();
            SourceCount = SourctcountResualt;

            using (
                EntityConnection ObjEntityconn = new EntityConnection("name=HHL_WeddingEntities"))
            {
                List<ObjectParameter> ObjListparmeter = new List<ObjectParameter>();
                ObjectContext ObjDataContext = new ObjectContext(ObjEntityconn);
                PageIndex = PageIndex - 1;
                ObjListparmeter.Add(new ObjectParameter("skip", PageIndex * PageSize));
                ObjListparmeter.Add(new ObjectParameter("limit", PageSize));
                List<T> ObjList = new List<T>();
                OrderType GroupBy = OrderType.Desc;
                if (Sort == 2)
                {
                    GroupBy = OrderType.Asc;
                }
                string RunSql = "Select VALUE c from HHL_WeddingEntities." + typeof(T).Name + " as c where 1=1 " + WhereStr + " order by c." + OrderByCloumname + " " + GroupBy.ToString() + " SKIP @skip LIMIT @limit ";


                var ObjReturnList = ObjDataContext.CreateQuery<T>(RunSql, ObjListparmeter.ToArray());
                ObjList = ObjReturnList.ToList();
                ObjEntityconn.Close();

                return ObjList;
            }
        }
        #endregion

        #region 构造where语句
        /// <summary>
        /// 构造where语句
        /// </summary>
        /// <param name="ObjParameter"></param>
        /// Needmanager是否需要查询下级
        /// <returns></returns>
        public static string GetWhere(List<PMSParameters> ObjParameterList)
        {
            StringBuilder SqlWhere = new StringBuilder();
            foreach (var ObjParameter in ObjParameterList)
            {
                ////如果需要查询我所管理的下级则查询
                //if (ObjParameter.IsContainsManagedEmployee)
                //{
                //    SqlWhere.Append(" and c." + ObjParameter.Name + " " + NSqlTypes.IN.ToString() + Employee.GetMyManagerEmpLoyee(ObjParameter.Value.ToString().ToInt32(), string.Empty));
                //    continue;
                //}

                switch (ObjParameter.Type)
                {
                    //模糊查询
                    case NSqlTypes.LIKE:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " Like '%" + ObjParameter.Value + "%'");
                        break;
                    case NSqlTypes.DateBetween://时间段之间
                        SqlWhere.Append(" and c." + ObjParameter.Name + " >= cast('" + ObjParameter.Value.ToString().Split(',')[0].ToDateTime().ToString("yyyy-MM-dd 00:00:00") + "' as System.DateTime) and c." + ObjParameter.Name + "<=cast('" + ObjParameter.Value.ToString().Split(',')[1].ToDateTime().ToString("yyyy-MM-dd 23:59:59") + "' as System.DateTime)");
                        break;
                    case NSqlTypes.OrDateBetween://时间段之间
                        SqlWhere.Append(" or c." + ObjParameter.Name + " >= cast('" + ObjParameter.Value.ToString().Split(',')[0].ToDateTime().ToString("yyyy-MM-dd 00:00:00") + "' as System.DateTime) and c." + ObjParameter.Name + "<=cast('" + ObjParameter.Value.ToString().Split(',')[1].ToDateTime().ToString("yyyy-MM-dd 23:59:59") + "' as System.DateTime)");
                        break;
                    case NSqlTypes.DateEquals:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " >= cast('" + ObjParameter.Value.ToString().ToDateTime().ToString("yyyy-MM-dd 00:00:00") + "' as System.DateTime)");
                        break;
                    //大于某个值
                    case NSqlTypes.Greaterthan:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " >= " + ObjParameter.Value);
                        break;
                    case NSqlTypes.NumLessThan:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " <= " + ObjParameter.Value);
                        break;
                    case NSqlTypes.LessThan:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " <= cast('" + ObjParameter.Value.ToString().ToDateTime().ToString("yyyy-MM-dd 00:00:00") + "' as System.DateTime)");
                        break;
                    case NSqlTypes.NumIn:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " in { " + ObjParameter.Value + " }");
                        break;
                    case NSqlTypes.OR:
                        SqlWhere.Append(" or c." + ObjParameter.Name + " = '" + ObjParameter.Value + "'");
                        break;
                    case NSqlTypes.AndLike:
                        SqlWhere.Append(" and (c." + ObjParameter.Name + " like '%" + ObjParameter.Value + "%'");
                        break;
                    case NSqlTypes.OrInt:
                        //多值
                        //if (ObjParameter.Value.ToString().Contains(","))
                        //{
                        //    SqlWhere.Append(" and (c." + ObjParameter.Name + " = " + ObjParameter.Value.ToString().Split(',')[0]);
                        //    SqlWhere.Append(" or c." + ObjParameter.Name + " = " + ObjParameter.Value.ToString().Split(',')[1] + ")");
                        //}
                        //多字段
                        if (ObjParameter.Name.ToString().Contains(","))
                        {
                            SqlWhere.Append(" and (c." + ObjParameter.Name.ToString().Split(',')[0] + " = " + ObjParameter.Value);
                            SqlWhere.Append(" or c." + ObjParameter.Name.ToString().Split(',')[1] + " = " + ObjParameter.Value + ")");
                        }
                        break;
                    case NSqlTypes.ORLike:
                        SqlWhere.Append(" or c." + ObjParameter.Name + " like '%" + ObjParameter.Value + "%'");
                        break;
                    case NSqlTypes.IN:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " in { " + ObjParameter.Value + " }");
                        break;
                    case NSqlTypes.OrIn:
                        SqlWhere.Append(" or c." + ObjParameter.Name + " in { " + ObjParameter.Value + " }");
                        break;
                    case NSqlTypes.Equal:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " = " + ObjParameter.Value + "");
                        break;
                    case NSqlTypes.Bit:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " =cast (" + ObjParameter.Value + " as System.Boolean)");
                        break;
                    case NSqlTypes.NotIN:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " not in { " + ObjParameter.Value + " }");
                        break;
                    case NSqlTypes.StringNotIN:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " not in { '" + ObjParameter.Value + " '}");
                        break;
                    case NSqlTypes.StringEquals:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " = '" + ObjParameter.Value + "'");
                        break;
                    case NSqlTypes.IsNull:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " Is Null");
                        break;
                    case NSqlTypes.IsNotNull:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " Is Not Null");
                        break;
                    case NSqlTypes.ColumnOr:
                        SqlWhere.Append(" and (c." + ObjParameter.Name.Split(',')[0] + "=" + ObjParameter.Value + " or c." + ObjParameter.Name.Split(',')[1] + "=" + ObjParameter.Value + ")");
                        break;
                    case NSqlTypes.NumBetween:
                        SqlWhere.Append(" and (c." + ObjParameter.Name + " between " + ObjParameter.Value.ToString().Split(',')[0] + " and " + ObjParameter.Value.ToString().Split(',')[1] + ")");
                        break;
                    case NSqlTypes.NotEquals:
                        SqlWhere.Append(" and c." + ObjParameter.Name + " != " + ObjParameter.Value + " ");
                        break;
                }
            }
            return SqlWhere.ToString();
        }
        #endregion

        #region 过时方法
        /// <summary>
        /// 返回参数
        /// </summary>
        /// <param name="ObjParameter"></param>
        /// <returns></returns>
        [Obsolete("此方法已过时，不再使用")]
        public static string ReturnWhere(ObjectParameter ObjParameter, out string EmpLoyeeID, out string KeyPar)
        {
            if (ObjParameter.Name.Contains("EmpLoyeeID"))
            {
                if (ObjParameter.Value.ToString().Contains(","))
                {
                    EmpLoyeeID = ObjParameter.Value.ToString().Split(',')[0];
                }
                else
                {
                    EmpLoyeeID = ObjParameter.Value.ToString();
                }
            }
            else
            {
                EmpLoyeeID = string.Empty;
            }

            if (ObjParameter.Name.Contains("SerchKeypar"))
            {
                KeyPar = ObjParameter.Value.ToString();
                return string.Empty;
            }
            else
            {
                KeyPar = string.Empty;
            }
            //LIKE查询
            if (ObjParameter.Name.Contains("LIKE"))
            {

                return " c." + ObjParameter.Name.Split('_')[0] + " Like '%" + ObjParameter.Value + "%'";
            }

            //字符串OR 
            if (ObjParameter.Name.Contains("OR"))
            {
                if (ObjParameter.Value.ToString().Split(',')[1] != string.Empty)
                {
                    return " c." + ObjParameter.Name.Split('_')[0] + " ='" + ObjParameter.Value.ToString().Split(',')[0] + "'"
                    + " or c." + ObjParameter.Name.Split('_')[0] + " ='" + ObjParameter.Value.ToString().Split(',')[1] + "'";
                }
                else
                {
                    return " c." + ObjParameter.Name.Split('_')[0] + " ='" + ObjParameter.Value.ToString().Split(',')[0] + "'"
                    + " or c." + ObjParameter.Name.Split('_')[2] + " ='" + ObjParameter.Value.ToString().Split(',')[0] + "'";
                }
            }

            //字段OR 仅两字段
            if (ObjParameter.Name.Contains("PVP"))
            {

                return " (c." + ObjParameter.Name.Split('_')[0] + " =" + ObjParameter.Value.ToString().Split(',')[0]
                + " or c." + ObjParameter.Name.Split('_')[1] + " =" + ObjParameter.Value.ToString().Split(',')[1] + ")";
            }

            //大于某个数字
            if (ObjParameter.Name.Contains("NumGreaterthan"))
            {
                return " c." + ObjParameter.Name.Split('_')[0] + " > " + ObjParameter.Value.ToString();
            }

            //大于小于之间
            if (ObjParameter.Name.Contains("Greaterthan"))
            {
                return " c." + ObjParameter.Name.Split('_')[0] + " >= " + ObjParameter.Value.ToString().Split(',')[0] +
                       " and c." + ObjParameter.Name.Split('_')[0] + "<=" + ObjParameter.Value.ToString().Split(',')[1];
            }
            //数字OR
            if (ObjParameter.Name.Contains("NumOr"))
            {
                var ObjValueList = ObjParameter.Value.ToString().Split(',');
                var ItemName = ObjParameter.Name.Split('_')[0];
                string EsqlWhere = string.Empty;
                foreach (var Objitem in ObjValueList)
                {
                    EsqlWhere += " c." + ObjParameter.Name.Split('_')[0] + " =" + Objitem + " or ";
                }
                EsqlWhere = EsqlWhere.Remove(EsqlWhere.Length - 3);
                EsqlWhere = "( " + EsqlWhere + " ) ";
                return EsqlWhere;
            }

            if (ObjParameter.Name.Contains("between"))
            {
                //判断是Detatime类型 范围
                if (ObjParameter.Value.ToString().Contains("/") || ObjParameter.Value.ToString().Contains("-"))
                {   //cast('1977-11-11' as System.DateTime)")
                    return " c." + ObjParameter.Name.Split('_')[0] + " >= cast('" + ObjParameter.Value.ToString().Split(',')[0].ToDateTime().ToString("yyyy-MM-dd 00:00:00") +
                        "' as System.DateTime) and c." + ObjParameter.Name.Split('_')[0] + "<=cast('" + ObjParameter.Value.ToString().Split(',')[1].ToDateTime().ToString("yyyy-MM-dd 23:59:59") + "' as System.DateTime)";
                }
                else
                {    //数字 范围


                    return " c." + ObjParameter.Name.Split('_')[0] + " BETWEEN " + ObjParameter.Value.ToString().Split(',')[0] + " and " + ObjParameter.Value.ToString().Split(',')[1];
                }
            }



            if (ObjParameter.Name.Contains("OTTimerSpan"))
            {
                //判断是Detatime类型 范围
                string Month = ObjParameter.Value.ToString().Split(',')[0];
                string Day = (int.Parse(ObjParameter.Value.ToString().Split(',')[1]) + 7).ToString();
                string DayStar = (int.Parse(ObjParameter.Value.ToString().Split(',')[1])).ToString();
                //cast('1977-11-11' as System.DateTime)")
                // return " c." + ObjParameter.Name.Split('_')[0] + " >= cast('" + ObjParameter.Value.ToString().Split(',')[0].ToDateTime().ToShortDateString() +
                //     "' as System.DateTime) and c." + ObjParameter.Name.Split('_')[0] + "<=cast('" + ObjParameter.Value.ToString().Split(',')[1].ToDateTime().ToShortDateString() + "' as System.DateTime) or" +
                //" c." + ObjParameter.Name.Split('_')[1] + " >= cast('" + ObjParameter.Value.ToString().Split(',')[2].ToDateTime().ToShortDateString() +
                //     "' as System.DateTime) or c." + ObjParameter.Name.Split('_')[1] + "<=cast('" + ObjParameter.Value.ToString().Split(',')[3].ToDateTime().ToShortDateString() + "' as System.DateTime)";
                return "Month(c." + ObjParameter.Name.Split('_')[0] + ")=" + Month + "" + " or Month(c." + ObjParameter.Name.Split('_')[1] + ")=" + Month + " and (" + "Day(c." + ObjParameter.Name.Split('_')[0] + ")>=" + DayStar + "or Day(c." + ObjParameter.Name.Split('_')[0] + ")<=" + Day + ")" + " and (" + "Day(c." + ObjParameter.Name.Split('_')[1] + ")>=" + DayStar + "or Day(c." + ObjParameter.Name.Split('_')[1] + ")<=" + Day + ")";

            }

            return " c." + ObjParameter.Name + "=@" + ObjParameter.Name;
        }
        #endregion

        #region 不分页查询数据
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="GetWhere">条件</param>
        /// <param name="OrderByColumn">排序字段</param>
        /// <param name="GroupBy">排序方式(倒序 Or 正序)</param>
        /// <returns></returns>
        public static List<T> GetByParameters(string GetWhere, string OrderByColumn, OrderType GroupBy = OrderType.Asc)
        {
            using (EntityConnection ObjEntityconn = new EntityConnection("name=HHL_WeddingEntities"))
            {
                List<ObjectParameter> ObjListparmeter = new List<ObjectParameter>();
                ObjectContext ObjDataContext = new ObjectContext(ObjEntityconn);

                string sql = "Select VALUE c from HHL_WeddingEntities." + typeof(T).Name + " as c where 1=1  " + GetWhere + " order by c." + OrderByColumn + " " + GroupBy.ToString();
                List<T> ObjList = new List<T>();

                var ObjReturnList = ObjDataContext.CreateQuery<T>(sql);
                ObjList = ObjReturnList.ToList();
                ObjEntityconn.Close();
                return ObjList;
            }
        }
        #endregion

        #region 判断该数据是否存在
        /// <summary>
        /// 判断该条数据是否存在
        /// </summary>
        /// <returns></returns>
        public static bool IsExists(List<PMSParameters> ObjParameterList)
        {
            string WhereStr = GetWhere(ObjParameterList);


            string sql = "Select count(*) from " + typeof(T).Name + " as c where 1=1 " + WhereStr;
            HHL_WeddingEntities ObjContex = new HHL_WeddingEntities();
            var SourctcountResualt = ObjContex.Database.SqlQuery<int>(sql, new List<ObjectParameter>().ToArray()).First();
            int Count = SourctcountResualt;
            if (Count > 0)
            {
                return true;            //该条数据已经存在
            }
            else
            {
                return false;           //不存在 就可以新增
            }
        }
        #endregion

    }
}
