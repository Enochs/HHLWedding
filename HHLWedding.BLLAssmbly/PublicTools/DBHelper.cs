using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Text;
using HHLWedding.ToolsLibrary;
using System.Linq.Expressions;

namespace HHLWedding.BLLAssmbly
{
    /// <summary>
    /// 作者：吴鹏
    /// 类型: 访问数据库类
    /// 时间：2013.06.21
    /// </summary>
    public class DBHelper
    {

        private string strSQL;
        private SqlConnection Conn;            //数据库Connection对象
        private SqlCommand SqlCmd;                //数据库OdbcCommand对象
        private DataSet ds = new DataSet();       //返回数据集
        private SqlDataAdapter SqlDataAdapter;    //数据库DataAdapter对象
        private int outTime = 10 * 60;//秒

        //部署


        //数据库连接字符串(web.config来配置)，多数据库可使用DbHelperSQLP来实现.
        //string conn_str = ConfigurationManager.AppSettings["HHL_WeddingEntities"];

        //string conn_str = "server=.;uid=sa;pwd=sasa;DataBase=HHL_Wedding";

        private string conn_str = ConfigurationManager.ConnectionStrings["wedding_Conn"].ToString();


        public DBHelper getInstance
        {
            get
            {
                DBHelper db = new DBHelper();
                return db;
            }
        }

        #region 打开数据库连接
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        private void OpenConn()
        {
            try
            {
                this.Conn = new SqlConnection(conn_str);
                if ((this.Conn != null) && (this.Conn.State != ConnectionState.Open))
                {
                    this.Conn.Open();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 关闭数据库连接
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private void CloseConn()
        {
            try
            {
                if ((this.Conn != null) && (this.Conn.State == ConnectionState.Open))
                {
                    Conn.Dispose();
                    this.Conn.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region  返回DataTable Pars
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="tempStrSQL">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExcuteForDataTable(string tempStrSQL, IDataParameter[] pars)
        {
            this.OpenConn();
            DataTable dt = new DataTable();
            this.strSQL = tempStrSQL;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = tempStrSQL;
                cmd.Connection = Conn;
                cmd.CommandType = CommandType.StoredProcedure;
                if (pars != null)
                {
                    cmd.Parameters.AddRange(pars);
                }
                this.SqlDataAdapter = new SqlDataAdapter(cmd);
                this.SqlDataAdapter.SelectCommand.CommandTimeout = outTime;
                this.SqlDataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
            return dt;
        }
        #endregion

        #region  返回DataTable
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="tempStrSQL">查询语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExcuteForDataTable(string tempStrSQL)
        {
            this.OpenConn();
            DataTable dt = new DataTable();
            this.strSQL = tempStrSQL;
            try
            {
                this.SqlDataAdapter = new SqlDataAdapter(this.strSQL, this.Conn);
                this.SqlDataAdapter.SelectCommand.CommandTimeout = outTime;
                this.SqlDataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
            return dt;
        }
        #endregion

        #region 返回List<T,Pars>

        /// <summary>
        /// 返回List<T>
        /// </summary>
        /// <param name="tempStrSQL">查询语句</param>
        /// <returns>List</returns>
        public List<T> ExcuteForList<T>(string tempStrSQL, SqlParameter[] pars) where T : new()
        {
            this.OpenConn();
            DataTable dt = new DataTable();
            this.strSQL = tempStrSQL;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Conn;
                cmd.CommandText = strSQL;
                cmd.CommandType = CommandType.StoredProcedure;

                if (pars != null)
                {
                    cmd.Parameters.AddRange(pars);
                }
                this.SqlDataAdapter = new SqlDataAdapter(cmd);
                this.SqlDataAdapter.SelectCommand.CommandTimeout = outTime;
                this.SqlDataAdapter.Fill(dt);
                return ConvertHelper.ToList<T>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 返回List<T>

        /// <summary>
        /// 返回List<T>
        /// </summary>
        /// <param name="tempStrSQL">查询语句</param>
        /// <returns>List</returns>
        public List<T> ExcuteForList<T>(string tempStrSQL) where T : new()
        {
            this.OpenConn();
            DataTable dt = new DataTable();
            this.strSQL = tempStrSQL;
            try
            {
                this.SqlDataAdapter = new SqlDataAdapter(this.strSQL, this.Conn);
                this.SqlDataAdapter.SelectCommand.CommandTimeout = outTime;
                this.SqlDataAdapter.Fill(dt);
                return ConvertHelper.ToList<T>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 返回 List<Dictionary<string, string>>
        /// <summary>
        /// 返回 List<Dictionary<string, string>>
        /// </summary>
        /// <param name="tempStrSQL">查询语句</param>
        /// <returns>List</returns>
        public List<Dictionary<string, string>> ExcuteForListDic(string tempStrSQL)
        {
            this.OpenConn();
            DataTable dt = new DataTable();
            this.strSQL = tempStrSQL;
            try
            {
                this.SqlDataAdapter = new SqlDataAdapter(this.strSQL, this.Conn);
                this.SqlDataAdapter.SelectCommand.CommandTimeout = outTime;
                this.SqlDataAdapter.Fill(dt);
                return ConvertHelper.ToListDictionary(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 返回首行首列[object]
        /// <summary>
        /// 返回首行首列[object]
        /// </summary>
        /// <param name="tempStrSQL">SQL语句</param>
        /// <returns>返回首行首列</returns>
        public object ExcuteScalar(string tempStrSQL)
        {
            this.OpenConn();
            this.strSQL = tempStrSQL;
            try
            {
                SqlCmd = new SqlCommand(tempStrSQL, this.Conn);
                SqlCmd.CommandTimeout = outTime;
                return SqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 返回更新条数[int]
        /// <summary>
        /// 返回更新条数[int]
        /// </summary>
        /// <param name="tempStrSQL">SQL语句</param>
        /// <returns>更新条数</returns>
        public int ExcuteNonQuery(string tempStrSQL)
        {
            this.OpenConn();
            this.strSQL = tempStrSQL;
            int intNumber = 0;
            try
            {
                SqlCmd = new SqlCommand(tempStrSQL, this.Conn);
                SqlCmd.CommandTimeout = outTime;
                intNumber = SqlCmd.ExecuteNonQuery();
                return intNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }

        }
        #endregion

        #region 返回更新条数[int,Pars]
        /// <summary>
        /// 返回更新条数[int]
        /// </summary>
        /// <param name="tempStrSQL">SQL语句</param>
        /// <returns>更新条数</returns>
        public int ExcuteNonQuery(string tempStrSQL, SqlParameter[] pars)
        {
            this.OpenConn();
            this.strSQL = tempStrSQL;
            int intNumber = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Conn;
                cmd.CommandText = strSQL;
                cmd.CommandType = CommandType.StoredProcedure;

                if (pars != null)
                {
                    cmd.Parameters.AddRange(pars);
                }
                cmd.CommandTimeout = outTime;
                intNumber = cmd.ExecuteNonQuery();
                return intNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }

        }
        #endregion

        #region ExcuteInsertReturnAutoID
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="tempStrSQL"></param>
        /// <returns></returns>
        public int ExcuteInsertReturnAutoID(string tempStrSQL)
        {
            this.OpenConn();
            this.strSQL = tempStrSQL + ";select last_insert_rowid();";
            int intNumber = 0;
            try
            {
                SqlCmd = new SqlCommand(tempStrSQL, this.Conn);
                SqlCmd.CommandTimeout = outTime;
                intNumber = Convert.ToInt32(SqlCmd.ExecuteScalar());
                return intNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 返回是否存在记录[bool]
        /// <summary>
        /// 返回是否存在记录[bool]
        /// </summary>
        /// <param name="tempStrSQL">SQL语句</param>
        /// <returns>是否存在</returns>
        public bool ExcuteExist(string tempStrSQL)
        {
            this.OpenConn();
            this.strSQL = tempStrSQL;
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter = new SqlDataAdapter(tempStrSQL, this.Conn);
                SqlDataAdapter.SelectCommand.CommandTimeout = outTime;
                SqlDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 插入多条记录
        /// <summary>
        /// 插入多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int ExcuteInsert<T>(List<T> data, string tableName)
        {
            try
            {
                if (data.Count == 0)
                    return 0;
                this.OpenConn();
                SqlCmd = new SqlCommand();
                SqlCmd.Connection = Conn;
                SqlCmd.CommandTimeout = outTime;
                SqlCmd.CommandType = CommandType.Text;
                PropertyInfo[] propertys = data[0].GetType().GetProperties();
                int index = 0;
                if (propertys.Count() == 0)
                    return 0;
                strSQL = "insert into " + tableName + "(";
                foreach (PropertyInfo pi in propertys)
                {
                    if (index > 0)
                    {
                        strSQL += pi.Name;
                        if (propertys.LastOrDefault() != pi)
                            strSQL += ",";
                        else
                            strSQL += ")";
                    }
                    index++;      //因为主键自增长
                }
                index = 0;
                strSQL += ("values(");
                foreach (PropertyInfo pi in propertys)
                {
                    if (index > 0)
                    {
                        strSQL += ("@" + pi.Name);
                        if (propertys.LastOrDefault() != pi)
                            strSQL += ",";
                        else
                            strSQL += ")";
                    }
                    index++;
                }
                SqlCmd.CommandText = strSQL;
                foreach (T t in data)
                {
                    propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        SqlParameter para = new SqlParameter();
                        para.ParameterName = "@" + pi.Name;
                        if (pi.GetType() == typeof(DateTime))
                            para.Value = ((DateTime)pi.GetValue(t, null)).ToString("yyyy-MM-dd HH:mm:ss");
                        else
                            para.Value = pi.GetValue(t, null);
                        SqlCmd.Parameters.Add(para);
                    }
                }
                int result = SqlCmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 插入多条记录-可以设置不插入的特殊字段
        /// <summary>
        /// 插入多条记录-可以设置不插入的特殊字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <param name="exExceptFiled"></param>
        /// <returns></returns>
        public int ExcuteInsert<T>(List<T> data, string tableName, string exExceptFiled)
        {
            try
            {
                if (data.Count == 0)
                    return 0;
                string[] exExFiledArr = exExceptFiled.Split(',');
                this.OpenConn();
                SqlCmd = new SqlCommand();
                SqlCmd.Connection = Conn;
                SqlCmd.CommandTimeout = outTime;
                SqlCmd.CommandType = CommandType.Text;
                PropertyInfo[] propertys = data[0].GetType().GetProperties();
                if (propertys.Count() == 0)
                    return 0;
                strSQL = "insert into " + tableName + "(";
                foreach (PropertyInfo pi in propertys)
                {
                    if (exExFiledArr.Contains(pi.Name))
                        continue;
                    strSQL += pi.Name;
                    if (propertys.LastOrDefault() != pi)
                        strSQL += ",";
                    else
                        strSQL += ")";
                }
                if (strSQL.EndsWith(","))
                    strSQL = strSQL.Substring(0, strSQL.Length - 1) + ")";
                strSQL += ("values(");
                foreach (PropertyInfo pi in propertys)
                {
                    if (exExFiledArr.Contains(pi.Name))
                        continue;
                    strSQL += ("@" + pi.Name);
                    if (propertys.LastOrDefault() != pi)
                        strSQL += ",";
                    else
                        strSQL += ")";
                }
                if (strSQL.EndsWith(","))
                    strSQL = strSQL.Substring(0, strSQL.Length - 1) + ")";
                SqlCmd.CommandText = strSQL;
                foreach (T t in data)
                {
                    propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (exExFiledArr.Contains(pi.Name))
                            continue;
                        SqlParameter para = new SqlParameter();
                        para.ParameterName = "@" + pi.Name;
                        if (pi.PropertyType == typeof(DateTime))
                            para.Value = ((DateTime)pi.GetValue(t, null)).ToString("yyyy-MM-dd HH:mm:ss");
                        else
                            para.Value = pi.GetValue(t, null);
                        SqlCmd.Parameters.Add(para);
                    }
                }
                int result = SqlCmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 插入单条记录-可以设置不插入的特殊字段
        /// <summary>
        /// 插入单条记录-可以设置不插入的特殊字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <param name="exExceptFiled"></param>
        /// <returns></returns>
        public int ExcuteInsert<T>(T data, string tableName, string exExceptFiled)
        {
            try
            {
                return ExcuteInsert<T>(new List<T>() { data }, tableName, exExceptFiled);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 插入单条记录
        /// <summary>
        ///  插入单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int ExcuteInsert<T>(T data, string tableName)
        {
            try
            {
                return ExcuteInsert<T>(new List<T>() { data }, tableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 插入单条记录并返回自增列ID
        /// <summary>
        /// 插入单条记录并返回自增列ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <param name="exExceptFiled"></param>
        /// <returns></returns>
        public int ExcuteInsertReturnAutoID<T>(T data, string tableName, string exExceptFiled)
        {
            try
            {
                string[] exExFiledArr = exExceptFiled.Split(',');
                this.OpenConn();
                SqlCmd = new SqlCommand();
                SqlCmd.Connection = Conn;
                SqlCmd.CommandTimeout = outTime;
                SqlCmd.CommandType = CommandType.Text;
                PropertyInfo[] propertys = data.GetType().GetProperties();
                if (propertys.Count() == 0)
                    return 0;
                strSQL = "insert into " + tableName + "(";
                foreach (PropertyInfo pi in propertys)
                {
                    if (exExFiledArr.Contains(pi.Name))
                        continue;
                    strSQL += pi.Name;
                    if (propertys.LastOrDefault() != pi)
                        strSQL += ",";
                    else
                        strSQL += ")";
                }
                if (strSQL.EndsWith(","))
                    strSQL = strSQL.Substring(0, strSQL.Length - 1) + ")";
                strSQL += ("values(");
                foreach (PropertyInfo pi in propertys)
                {
                    if (exExFiledArr.Contains(pi.Name))
                        continue;
                    strSQL += ("@" + pi.Name);
                    if (propertys.LastOrDefault() != pi)
                        strSQL += ",";
                    else
                        strSQL += ")";
                }
                if (strSQL.EndsWith(","))
                    strSQL = strSQL.Substring(0, strSQL.Length - 1) + ")";
                strSQL += ";select @@Identity;";
                SqlCmd.CommandText = strSQL;

                propertys = data.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (exExFiledArr.Contains(pi.Name))
                        continue;
                    SqlParameter para = new SqlParameter();
                    para.ParameterName = "@" + pi.Name;
                    if (pi.GetType() == typeof(DateTime))
                        para.Value = ((DateTime)pi.GetValue(data, null)).ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        para.Value = pi.GetValue(data, null);
                    SqlCmd.Parameters.Add(para);
                }

                int result = Convert.ToInt32(SqlCmd.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion


        public bool ExcuteExistForParaMeters(string sql, Dictionary<string, object> paraDic)
        {
            this.OpenConn();
            try
            {
                SqlCmd = new SqlCommand(sql, Conn);
                SqlCmd.CommandType = CommandType.Text;
                foreach (string para in paraDic.Keys)
                {
                    SqlParameter spa = new SqlParameter("@" + para, paraDic[para]);
                    SqlCmd.Parameters.Add(spa);
                }
                SqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter();
                SqlDataAdapter.SelectCommand = SqlCmd;
                SqlDataAdapter.SelectCommand.CommandTimeout = outTime;
                DataTable dt = new DataTable();
                SqlDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }

        #region ExcuteTrancationForSQL 执行多条SQL语句
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public bool ExcuteTrancationForSQL(string[] sqls)
        {
            this.OpenConn();
            SqlTransaction tran = Conn.BeginTransaction();
            try
            {
                foreach (string sql in sqls)
                {
                    SqlCmd = new SqlCommand(sql, Conn);
                    SqlCmd.CommandType = CommandType.Text;
                    SqlCmd.CommandTimeout = outTime;
                    SqlCmd.Transaction = tran;
                    SqlCmd.ExecuteNonQuery();
                }
                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion


        #region 存储过程操作

        /// <summary>
        /// 执行存储过程，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(conn_str);
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;

        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(conn_str))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public List<T> ExcuteForList<T>(string storedProcName, IDataParameter[] parameters, string tableName) where T : new()
        {
            using (SqlConnection connection = new SqlConnection(conn_str))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return ConvertHelper.ToList<T>(dataSet.Tables[0]);
            }
        }



        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (SqlConnection connection = new SqlConnection(conn_str))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = Times;
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }


        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (SqlConnection connection = new SqlConnection(conn_str))
            {
                int result;
                connection.Open();
                SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion




        #region 查询
        /// <summary>
        /// 万能分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parm"></param>
        /// <param name="pagesize"></param>
        /// <param name="currentpage"></param>
        /// <param name="OrderColumnName"></param>
        /// <param name="Pkey"></param>
        /// <param name="sum"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<T> GetExcuteForList<T>(List<PMSParameters> parm, int pagesize, int currentpage, string OrderColumnName, string Pkey, ref int sum, int sort = 1) where T : new()
        {
            this.OpenConn();
            DataTable dt = new DataTable();
            string GetWheres = GetWhere(parm);
            OrderType ordertype = OrderType.Desc;
            if (sort == 2)
            {
                ordertype = OrderType.Asc;
            }
            string BySort = " order by " + OrderColumnName + " " + ordertype;
            string sql = string.Format("select top {0} * from {3} where 1=1 and {4} not in (select top {1} {4} from {3} where 1=1 {2}) {2}", pagesize, (currentpage - 1) * pagesize, GetWheres + BySort, typeof(T).Name, Pkey);
            try
            {
                this.SqlDataAdapter = new SqlDataAdapter(sql, this.Conn);
                this.SqlDataAdapter.SelectCommand.CommandTimeout = outTime;
                this.SqlDataAdapter.Fill(dt);
                var QueryList = ConvertHelper.ToList<T>(dt);

                sum = Convert.ToInt32(ExcuteScalar("select count(*) from " + typeof(T).Name + " where 1=1 " + GetWheres));

                return QueryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion


        #region 获取List集合
        /// <summary>
        /// 根据主键返回List
        /// </summary>
        /// <param name="tableName">表名</param>
        ///  /// <param name="pkey">主键名称</param>
        ///   /// <param name="value">主键值</param>
        /// <returns>List</returns>
        public List<T> GetForList<T>(string tableName, string pkey, string value) where T : new()
        {
            this.OpenConn();
            DataTable dt = new DataTable();
            this.strSQL = "select * from " + tableName + " where " + pkey + "=" + value;
            try
            {
                this.SqlDataAdapter = new SqlDataAdapter(this.strSQL, this.Conn);
                this.SqlDataAdapter.SelectCommand.CommandTimeout = outTime;
                this.SqlDataAdapter.Fill(dt);
                return ConvertHelper.ToList<T>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseConn();
            }
        }
        #endregion

        #region 条件
        public string GetWhere(List<PMSParameters> parm)
        {
            StringBuilder SqlWhere = new StringBuilder();
            foreach (var item in parm)
            {
                switch (item.Type)
                {
                    case NSqlTypes.Equal:
                        SqlWhere.Append(" and " + item.Name + " = " + item.Value);
                        break;
                    case NSqlTypes.StringEquals:
                        SqlWhere.Append(" and " + item.Name + " = '" + item.Value + "'");
                        break;
                    case NSqlTypes.LIKE:
                        SqlWhere.Append(" and " + item.Name + " like '%" + item.Value + "%'");
                        break;
                    case NSqlTypes.DateBetween: //时间段之间
                        SqlWhere.Append(" and c." + item.Name + " >= cast('" + item.Value.ToString().Split(',')[0].ToDateTime().ToString("yyyy-MM-dd 00:00:00") +
                        "' as System.DateTime) and c." + item.Name + "<=cast('" + item.Value.ToString().Split(',')[1].ToDateTime().AddDays(1).ToString("yyyy-MM-dd 23:59:59") + "' as System.DateTime)");
                        break;
                    case NSqlTypes.Greaterthan:
                        SqlWhere.Append(" and c." + item.Name + " > " + item.Value);
                        break;
                    case NSqlTypes.NumIn:
                        SqlWhere.Append(" and c." + item.Name + " in { " + item.Value + " }");
                        break;
                    case NSqlTypes.OR:

                        break;
                    case NSqlTypes.IN:
                        SqlWhere.Append(" and c." + item.Name + " in { " + item.Value + " }");
                        break;
                    case NSqlTypes.Bit:
                        SqlWhere.Append(" and c." + item.Name + " =cast (" + item.Value + " as System.Boolean)");
                        break;
                    case NSqlTypes.NotIN:
                        SqlWhere.Append(" and c." + item.Name + " not in { " + item.Value + " }");
                        break;
                    case NSqlTypes.IsNull:
                        SqlWhere.Append(" and c." + item.Name + " Is Null");
                        break;
                    case NSqlTypes.IsNotNull:
                        SqlWhere.Append(" and c." + item.Name + " Is Not Null");
                        break;
                    case NSqlTypes.ColumnOr:
                        SqlWhere.Append(" and (c." + item.Name.Split(',')[0] + "=" + item.Value + " or c." + item.Name.Split(',')[1] + "=" + item.Value + ")");
                        break;

                }
            }
            SqlWhere = SqlWhere.Replace("{", "(").Replace("}", ")").Replace("System.Boolean", "bit").Replace("False", "'0'").Replace("True", "'1'").Replace("System.DateTime", "date");
            return SqlWhere.ToString();
        }
        #endregion


        //批量插入数据
        public bool ExcuteNonQuery(DataTable dt)
        {
            this.OpenConn();
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(this.Conn);
            sqlbulkcopy.BulkCopyTimeout = 100;  //超时之前操作完成所允许的秒数
            sqlbulkcopy.BatchSize = dt.Rows.Count;  //每一批次中的行数
            sqlbulkcopy.DestinationTableName = dt.TableName;  //服务器上目标表的名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sqlbulkcopy.ColumnMappings.Add(i, i);  //映射定义数据源中的列和目标表中的列之间的关系
            }
            sqlbulkcopy.WriteToServer(dt);  // 将DataTable数据上传到数据表中
            this.CloseConn();
            return true;
        }



        #region 根据字段排序
        public static IQueryable<T> DataSorting<T>(IQueryable<T> source, string sortExpression, string sortDirection)
        {
            string sortingDir = string.Empty;
            if (sortDirection.ToUpper().Trim() == "ASC")
                sortingDir = "OrderBy";
            else if (sortDirection.ToUpper().Trim() == "DESC")
                sortingDir = "OrderByDescending";

            PropertyInfo[] properties = typeof(T).GetProperties();
            ParameterExpression param = null;
            if (string.IsNullOrEmpty(sortExpression))
            {
                param = Expression.Parameter(typeof(T), properties[0].Name);
                sortExpression = properties[0].Name;
            }
            else
            {
                param = Expression.Parameter(typeof(T), sortExpression);
            }

            PropertyInfo pi = typeof(T).GetProperty(sortExpression);
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, source.Expression, Expression.Lambda(Expression.Property(param, sortExpression), param));
            IQueryable<T> query = source.AsQueryable().Provider.CreateQuery<T>(expr);
            return query;
        }

        public static IQueryable<T> DataPaging<T>(IQueryable<T> source, int pageNumber, int pageSize)
        {
            return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<T> SortingAndPaging<T>(IQueryable<T> source, string sort, int pageNumber, int pageSize)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                string sortExpression = sort.Split('-')[0];
                string sortDirection = sort.Split('-')[1];
                IQueryable<T> query = DataSorting<T>(source, sortExpression, sortDirection);
                return DataPaging(query, pageNumber, pageSize);
            }
            else
            {
                IQueryable<T> query = DataSorting<T>(source, "", "asc");
                return DataPaging(query, pageNumber, pageSize);
            }

        }
        #endregion



        #region 条件查询


        public static IQueryable<T> GetQueryByCondtion<T>(IQueryable<T> source, string condition)
        {
            PropertyInfo[] propertys = typeof(T).GetType().GetProperties();
            return null;
        }
        #endregion
    }
}
