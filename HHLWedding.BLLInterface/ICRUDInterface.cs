using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.ToolsLibrary;
using System.Linq.Expressions;

namespace HHLWedding.BLLInterface
{
    /// <summary>
    /// 增删改查接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICRUDInterface<T> where T : class
    {

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        T GetByID(int? KeyID);


        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        List<T> GetByAll();


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns>返回Key</returns>
        int Insert(T ObjectT);


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        int Update(T ObjectT);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        int Delete(T ObjectT);

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pars">条件</param>
        /// <param name="orderName">排序</param>
        /// <param name="page">页面</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="sourceCount">总跳输</param>
        /// <returns></returns>
        List<T> GetAllByPager(List<PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount);

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="parmList">条件</param>
        /// <param name="sort">排序字段</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        List<T> GetAllByPage(int pageIndex, int pageSize, List<Expression<Func<T, bool>>> parmList, string sort, out int count);
    }
}

