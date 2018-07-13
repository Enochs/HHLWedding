using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using EntityFramework.Extensions;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.BLLAssmbly
{
    public class EmployeeJob : ICRUDInterface<Sys_EmployeeJob>
    {
        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();
        /// <summary>
        /// 员工工作JobID来进行删除操作
        /// </summary>
        /// <param name="ObjectT">员工工作实体类</param>
        /// <returns></returns>
        public int Delete(Sys_EmployeeJob ObjectT)
        {
            if (ObjectT != null)
            {

                ObjEntity.Sys_EmployeeJob.FirstOrDefault(
                   C => C.JobID == ObjectT.JobID).Status = 1;
                return ObjEntity.SaveChanges();


            }
            return 0;

        }
        /// <summary>
        /// 返回员工工作表中所有的信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_EmployeeJob> GetByAll()
        {
            return ObjEntity.Sys_EmployeeJob.Where(C => C.Status == 1).ToList();

        }
        /// <summary>
        /// 根据主键返回单个员工工作信息
        /// </summary>
        /// <param name="KeyID">主键值</param>
        /// <returns>根据查询之后的结果，如果为空则返回默认实例</returns>
        public Sys_EmployeeJob GetByID(int? KeyID)
        {
            if (KeyID.HasValue)
            {
                Sys_EmployeeJob empJob = ObjEntity.Sys_EmployeeJob.FirstOrDefault(
                    C => C.JobID == KeyID);
                if (empJob != null)
                {
                    return empJob;
                }

            }

            return new Sys_EmployeeJob();


        }
        /// <summary>
        /// 对于员工工作表的分页操作
        /// </summary>
        /// <param name="PageSize">一页多少条</param>
        /// <param name="PageIndex">当前第几页</param>
        /// <param name="SourceCount">out 输出一共有多少条记录</param>
        /// <returns>返回员工工作表的集合</returns>
        public List<Sys_EmployeeJob> GetByIndex(int PageSize, int PageIndex, out int SourceCount)
        {
            SourceCount = ObjEntity.Sys_EmployeeJob.Where(C => C.Status == 0).Count();

            List<Sys_EmployeeJob> resultList = ObjEntity.Sys_EmployeeJob.Where(C => C.Status == 0)
                //进行排序功能操作，不然系统会抛出异常
                   .OrderByDescending(C => C.JobID)
                   .Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();

            if (resultList.Count == 0)
            {
                resultList = new List<Sys_EmployeeJob>();
            }
            return resultList;

        }
        /// <summary>
        /// 添加员工工作信息
        /// </summary>
        /// <param name="ObjectT">员工工作实体类</param>
        /// <returns>返回新添加员工工作信息的编号</returns>
        public int Insert(Sys_EmployeeJob ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.Sys_EmployeeJob.Add(ObjectT);
                if (ObjEntity.SaveChanges() > 0)
                {
                    return ObjectT.JobID;
                }

            }
            return 0;
        }

        /// <summary>
        /// 根据员工工作JobID，修改某个员工工作的信息
        /// </summary>
        /// <param name="ObjectT">员工工作类实体</param>
        /// <returns>返回被修改的某个员工工作的JobID</returns>
        public int Update(Sys_EmployeeJob ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.SaveChanges();
                return ObjectT.JobID;
            }
            return 0;
        }

        #region 分页获取员工职务
        /// <summary>
        /// @author:wupeng
        /// @datetime:2016-07-22
        /// @desc:分页获取员工职务
        /// </summary>
        /// <param name="pars">参数</param>
        /// <param name="orderName">排序字段</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="sourceCount">总条数</param>
        /// <returns></returns>
        public List<Sys_EmployeeJob> GetAllByPager(List<PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount)
        {
            return PublicDataTools<Sys_EmployeeJob>.GetDataByWhereParameter(pars, orderName, pageSize, page, out sourceCount);
        }
        #endregion
    }
}
