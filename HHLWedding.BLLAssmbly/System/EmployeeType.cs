using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.DataAssmblly;
using HHLWedding.BLLInterface;
using HHLWedding.ToolsLibrary;
namespace HHLWedding.BLLAssmbly
{
    public class EmployeeType:ICRUDInterface<Sys_EmployeeType>
    {
        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();
        /// <summary>
        /// 人员类型EmployeeTypeID来进行删除操作
        /// </summary>
        /// <param name="ObjectT">人员类型实体类</param>
        /// <returns></returns>
        public int Delete(Sys_EmployeeType ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.Sys_EmployeeType.FirstOrDefault(
                C => C.EmployeeTypeID == ObjectT.EmployeeTypeID).Status = 1;
                return ObjEntity.SaveChanges();
            }
            return 0;
        }
        /// <summary>
        /// 返回人员类型表中所有的信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_EmployeeType> GetByAll()
        {
            return ObjEntity.Sys_EmployeeType.Where(C => C.Status == 0).ToList();
        }


        /// <summary>
        /// 对于人员类型表的分页操作
        /// </summary>
        /// <param name="PageSize">一页多少条</param>
        /// <param name="PageIndex">当前第几页</param>
        /// <param name="SourceCount">out 输出一共有多少条记录</param>
        /// <returns>返回人员类型表的集合</returns>
        public List<Sys_EmployeeType> GetByIndex(int PageSize, int PageIndex, out int SourceCount)
        {
            SourceCount = ObjEntity.Sys_EmployeeType.Count();

            List<Sys_EmployeeType> resultList = ObjEntity.Sys_EmployeeType.Where(C => C.Status == 0)
                //进行排序功能操作，不然系统会抛出异常
                   .OrderByDescending(C => C.EmployeeTypeID)
                   .Skip(PageSize * (PageIndex-1)).Take(PageSize).ToList();

            if (resultList.Count == 0)
            {
                resultList = new List<Sys_EmployeeType>();
            }
            return resultList;
        }


        /// <summary>
        /// 添加用户类型
        /// </summary>
        /// <param name="ObjectT">用户类型实体</param>
        /// <returns>返回新增加的自增序列</returns>
        public int Insert(Sys_EmployeeType ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.Sys_EmployeeType.Add(ObjectT);
                if (ObjEntity.SaveChanges() > 0)
                {

                    return ObjectT.EmployeeTypeID;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 修改用户类型
        /// </summary>
        /// <param name="ObjectT">用户类型实体</param>
        /// <returns>返回修改的自增序列</returns>
        public int Update(Sys_EmployeeType ObjectT)
        {
            if (ObjectT != null)
            {
                if (ObjEntity.SaveChanges() > 0)
                {

                    return ObjectT.EmployeeTypeID;
                }
            }

            return 0;
        }

        /// <summary>
        /// 返回单个员工类型信息
        /// </summary>
        /// <param name="KeyID"></param>
        /// <returns></returns>
        public Sys_EmployeeType GetByID(int? KeyID)
        {
          return  ObjEntity.Sys_EmployeeType.FirstOrDefault(C=>C.EmployeeTypeID==KeyID);
        }

        #region 分页获取员工类型
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
        public List<Sys_EmployeeType> GetAllByPager(List<PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount)
        {
            return PublicDataTools<Sys_EmployeeType>.GetDataByWhereParameter(pars, orderName, pageSize, page, out sourceCount);
        }
        #endregion
       
    }
}
