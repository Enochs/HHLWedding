using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using HHLWedding.ToolsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using System.Linq.Expressions;

namespace HHLWedding.BLLAssmbly
{
    public class EmployeePower : ICRUDInterface<Sys_EmployeePower>
    {

        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();


        #region 根据Id查找员工权限
        /// <summary>
        /// @datetime:2016-08-03
        /// @author:wp
        /// @desc:根据ID查找权限
        /// </summary>
        /// <param name="KeyID">powerID</param>
        /// <returns></returns>

        public Sys_EmployeePower GetByID(int? KeyID)
        {
            return ObjEntity.Sys_EmployeePower.FirstOrDefault(c => c.PowerID == KeyID);
        }
        #endregion


        #region 获取所有员工的权限
        /// <summary>
        /// @datetime:2016-08-03
        /// @author:wp
        /// @desc: 获取所有权限
        /// </summary>
        /// <returns></returns>

        public List<Sys_EmployeePower> GetByAll()
        {
            return ObjEntity.Sys_EmployeePower.ToList();
        }
        #endregion

        public List<Sys_EmployeePower> GetByIndex(int PageSize, int PageIndex, out int SourceCount)
        {
            throw new NotImplementedException();
        }

        #region 添加员工权限
        /// <summary>
        /// @datetime:2016-08-03
        /// @author:wp
        /// @desc: 添加员工权限
        /// </summary>
        /// <param name="ObjectT">实体类</param>
        /// <returns></returns>
        public int Insert(Sys_EmployeePower ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.Sys_EmployeePower.Add(ObjectT);
                return ObjEntity.SaveChanges();
            }
            return 0;
        }
        #endregion


        #region 修改员工权限
        /// <summary>
        /// @datetime:2016-08-03
        /// @author:wp
        /// @desc: 添加员工权限
        /// </summary>
        /// <param name="ObjectT">实体类</param>
        /// <returns></returns>
        public int Update(Sys_EmployeePower ObjectT)
        {
            if (ObjectT != null)
            {
                return ObjEntity.SaveChanges();
            }
            return 0;
        }
        #endregion

        #region 删除功能
        /// <summary>
        /// @datetime:2016-08-03
        /// @author:wp
        /// @desc: 根据传入的实体删除
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>

        public int Delete(Sys_EmployeePower ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.Sys_EmployeePower.Remove(ObjectT);
                return ObjEntity.SaveChanges();
            }
            return 0;
        }
        #endregion

        #region 验证用户该权限是否存在
        /// <summary>
        /// @datetime:2016-08-03
        /// @author:wp
        /// @desc: 是否存在该权限
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="ChannelId"></param>
        /// <returns></returns>

        public bool CheckPower(int EmployeeId, int ChannelId)
        {
            Sys_EmployeePower employeePower = ObjEntity.Sys_EmployeePower.FirstOrDefault(c => c.EmployeeID == EmployeeId && c.ChannelID == ChannelId);
            if (employeePower == null)          //不存在  就可以添加
            {
                return false;
            }
            else                               //若存在  就修改
            {
                return true;
            }
        }
        #endregion


        #region 获取拥有该权限的实体类
        /// <summary>
        /// 获取某个员工的权限
        /// </summary>
        /// <param name="EmployeeId">员工Id</param>
        /// <param name="ChannelId">权限ID</param>
        /// <returns></returns>
        public Sys_EmployeePower GetEmpPower(int EmployeeId, int ChannelId)
        {
            return ObjEntity.Sys_EmployeePower.FirstOrDefault(c => c.EmployeeID == EmployeeId && c.ChannelID == ChannelId);
        }
        #endregion


        #region 查询该员工所有权限
        /// <summary>
        /// 查询该员工所有权限
        /// </summary>
        /// <param name="EmloyeeId"></param>
        /// <returns></returns>

        public List<Sys_EmployeePower> GetAllByEmployeeId(int EmloyeeId)
        {
            return ObjEntity.Sys_EmployeePower.Where(c => c.EmployeeID == EmloyeeId).ToList();
        }
        #endregion

        #region 查询某个状态的员工的权限
        /// <summary>
        /// 根据状态查询员工权限
        /// </summary>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public List<Sys_EmployeePower> GetAllByStatus(int EmloyeeId, int status)
        {
            return ObjEntity.Sys_EmployeePower.Where(c => c.EmployeeID == EmloyeeId && c.Status == status).ToList();
        }
        #endregion

        /// <summary>
        /// 获取员工启用的父级权限
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public List<Sys_EmployeePower> GetEmpParentPower(int EmloyeeId)
        {
            return ObjEntity.Sys_EmployeePower.Where(c => c.EmployeeID == EmloyeeId && c.Status == 1 && c.ItemLevel == 1).OrderBy(c => c.Sort).ToList();
        }

        /// <summary>
        /// 获取员工某个权限下的子级权限
        /// </summary>
        /// <returns></returns>
        public List<Sys_EmployeePower> GetEmpChildPower(int EmloyeeId, int ChannelId)
        {
            return ObjEntity.Sys_EmployeePower.Where(c => c.EmployeeID == EmloyeeId && c.Parent == ChannelId && c.Status == 1 && c.ItemLevel == 2).OrderBy(c => c.Sort).ToList();
        }


        public List<Sys_EmployeePower> GetAllByPager(List<PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 修改UrlAddress
        /// </summary>
        /// <param name="ChannedlId"></param>
        /// <returns></returns>
        public void UpdateChannel(int ChannedlId, Sys_Channel Channel)
        {
            ObjEntity.Sys_EmployeePower.Where(c => c.ChannelID == ChannedlId).Update(c => new Sys_EmployeePower
            {
                UrlAddress = Channel.ChannelAddress,
                Powername = Channel.ChannelName,
                Sort = Channel.SortInt
            });
        }

        public List<Sys_EmployeePower> GetAllByPage(int pageIndex, int pageSize, List<Expression<Func<Sys_EmployeePower, bool>>> parmList, string sort, out int count)
        {
            throw new NotImplementedException();
        }
    }
}
