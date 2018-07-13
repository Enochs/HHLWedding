
/**
 Version :HaoAi 1.0
 File Name :Customers
 Author:黄晓可
 Date:2013.3.13
 Description:用户权限管理 实现ICRUDInterface<T> 接口中的方法
 **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
namespace HHLWedding.BLLAssmbly
{
    public class UserJurisdiction : ICRUDInterface<Sys_UserJurisdiction>
    {        /// <summary>
        /// EF操作实例化
        /// </summary>
        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();

        /// <summary>
        /// 删除用户频道权限
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Delete(Sys_UserJurisdiction ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.Sys_UserJurisdiction.Remove(GetByID(ObjectT.JurisdictionID));
                return ObjEntity.SaveChanges();

            }
            return 0;
        }



        /// <summary>
        /// 根据频道ID删除权限
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public int DeleteByChannelID(int ChannelID)
        {
            var ObjChannelList = ObjEntity.Sys_UserJurisdiction.Where(C => C.ChannelID == ChannelID).ToList();
            foreach (var Objitem in ObjChannelList)
            {
                ObjEntity.Sys_UserJurisdiction.Remove(Objitem);
                ObjEntity.SaveChanges();
            }
            return ChannelID;

        }


        /// <summary>
        /// 获取此模块下的人
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public List<int> GetEmpLoyeeByChannel(int? ChannelID)
        {
            var ObjChannelList = ObjEntity.Sys_UserJurisdiction.Where(C => C.ChannelID == ChannelID && C.IsClose == false);
            List<int> ObjEmpLoyeeKeyList = new List<int>();
            foreach (var Objitem in ObjChannelList)
            {
                ObjEmpLoyeeKeyList.Add(Objitem.EmployeeID.Value);
            }

            return ObjEmpLoyeeKeyList.Distinct().ToList();
        }

        public bool CheckByClassType(string Type, int? EmpLoyeeID)
        {
            ChannelService ObjChannelBLL = new ChannelService();
            var objChannel = ObjChannelBLL.GetbyClassType(Type);
            if (objChannel != null)
            {
                var ObjModel = ObjEntity.Sys_UserJurisdiction.FirstOrDefault(C => C.ChannelID == objChannel.ChannelID && C.EmployeeID == EmpLoyeeID && C.IsClose == false);
                if (ObjModel != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据用户和频道删除
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public int DeleteByEmpLoyeeID(int EmployeeID, int ChannelID)
        {
            if (EmployeeID != 0)
            {
                var ObjDeleteModel = ObjEntity.Sys_UserJurisdiction.FirstOrDefault
                    (C => C.EmployeeID == EmployeeID && C.ChannelID == ChannelID);
                if (ObjDeleteModel != null)
                {
                    ObjEntity.Sys_UserJurisdiction.Remove(ObjDeleteModel);
                    return ObjEntity.SaveChanges();
                }
                else
                {
                    return 0;
                }

            }
            return 0;
        }

        /// <summary>
        /// 获取用户是否有此频道的阅读权限
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public bool GetUserJurisdictionByChannel(int EmployeeID, int ChannelID)
        {

            return ObjEntity.Sys_UserJurisdiction.Count(C => C.EmployeeID == EmployeeID && C.ChannelID == ChannelID) > 0;
        }



        /// <summary>
        /// 获取频道下的用户上级审核人
        /// </summary>
        /// <param name="ChannelType"></param>
        /// <param name="EmpLoyeeID"></param>
        /// <returns></returns>
        public Sys_UserJurisdiction GetByChannelType(string ChannelType, int EmpLoyeeID)
        {
            ChannelService ObjChannelBLL = new ChannelService();
            var ObjChannemModel = ObjChannelBLL.GetbyClassType(ChannelType);
            if (ObjChannemModel != null)
            {
                var ObjUserJurisdiction = ObjEntity.Sys_UserJurisdiction.FirstOrDefault(C => C.EmployeeID == EmpLoyeeID && C.ChannelID == ObjChannemModel.ChannelID);
                if (ObjUserJurisdiction != null)
                {
                    return ObjUserJurisdiction;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 根据功能Type和用户获取单独的权限
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public Sys_UserJurisdiction GetUserJurisdictionByChanneltype(int? EmployeeID, string Type)
        {
            ChannelService ObjChannelBLL = new ChannelService();
            var ChannelID = ObjChannelBLL.GetbyClassType(Type).ChannelID;
            return ObjEntity.Sys_UserJurisdiction.FirstOrDefault(C => C.EmployeeID == EmployeeID && C.ChannelID == ChannelID);
        }



        /// <summary>
        /// 获取用户的频道
        /// </summary>
        /// <param name="EmpLoyeeID"></param>
        /// <returns></returns>
        //public List<View_EmpLoyeeChannel> GetEmPloyeeChannel(int? EmpLoyeeID, int? ParentID)
        //{
        //    var ReturnList = ObjEntity.View_EmpLoyeeChannel.Where(C => C.EmployeeID == EmpLoyeeID && C.Parent == ParentID).ToList();
        //    if (ReturnList.Count > 0)
        //    {
        //        return ReturnList;
        //    }
        //    else
        //    {
        //        return ObjEntity.View_EmpLoyeeChannel.Where(C => C.EmployeeID == EmpLoyeeID && C.ChannelID == ParentID).ToList();
        //    }
        //}




        /// <summary>
        /// 根据频道和用户获取单条用户权限
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public Sys_UserJurisdiction GetUserJurisdictionByChannelandEmpLoyee(int? EmployeeID, int? ChannelID)
        {

            var ObExistModel = ObjEntity.Sys_UserJurisdiction.FirstOrDefault(C => C.EmployeeID == EmployeeID && C.ChannelID == ChannelID);
            if (ObExistModel == null)
            {

                Sys_UserJurisdiction ObjctT = new Sys_UserJurisdiction();
                Employee ObjEmployeeBLL = new Employee();
                ObjctT.EmployeeID = EmployeeID;
                ObjctT.ChannelID = ChannelID;
                ObjctT.DepartmentID = ObjEmployeeBLL.GetByID(EmployeeID).DepartmentID;
                ObjctT.IsClose = true;
                ObjctT.DataPower = 1;
                ObjctT.DataPowerMd5Key = string.Empty;
                this.Insert(ObjctT);
                return ObjctT;

            }
            else
            {
                return ObExistModel;
            }
        }


        /// <summary>
        /// 添加录入客户信息
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int StarInsert(Sys_UserJurisdiction ObjectT)
        {
            if (ObjectT != null)
            {
                var UserJurisdictionModel = ObjEntity.Sys_UserJurisdiction.FirstOrDefault(C => C.ChannelID == ObjectT.ChannelID && C.EmployeeID == ObjectT.EmployeeID);
                if (UserJurisdictionModel == null)
                {
                    ObjEntity.Sys_UserJurisdiction.Add(ObjectT);

                    if (ObjEntity.SaveChanges() > 0)
                    {
                        return ObjectT.JurisdictionID;
                    }
                }
                //else
                //{
                //    //UserJurisdictionModel.IsClose = ObjectT.IsClose;

                //    UserJurisdictionModel.ChecksEmployee = ObjectT.ChecksEmployee;
                //    UserJurisdictionModel.DataPower = ObjectT.DataPower;
                //    UserJurisdictionModel.Dispatching = ObjectT.Dispatching;
                //    this.Update(UserJurisdictionModel);
                //}

            }
            return 0;
        }



        /// <summary>
        /// 获取用户的派工人
        /// </summary>
        /// <param name="ChannelGetType"></param>
        /// <returns></returns>
        public int? GetDispatchingByChannelType(string ChannelGetType, int EmpLoyeeID)
        {
            ChannelService ObjChannelBLL = new ChannelService();
            var ChannelID = ObjChannelBLL.GetbyClassType(ChannelGetType).ChannelID;
            var ObjJurisdModel = ObjEntity.Sys_UserJurisdiction.FirstOrDefault(C => C.EmployeeID == EmpLoyeeID && C.ChannelID == ChannelID);
            if (ObjJurisdModel == null)
            {
                return EmpLoyeeID;
            }
            else
            {
                return ObjJurisdModel.Dispatching;

            }

        }


        /// <summary>
        /// 获取所有用户权限
        /// </summary>
        /// <returns></returns>
        public List<Sys_UserJurisdiction> GetByAll()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 根据用户获取权限
        /// </summary>
        /// <param name="EmployeeID">员工ID</param>
        /// <returns>返回元用下的频道</returns>
        public List<Sys_UserJurisdiction> GetByEmpLoyee(int? EmployeeID)
        {
            return ObjEntity.Sys_UserJurisdiction.Where(C => C.EmployeeID == EmployeeID).ToList();
        }

        /// <summary>
        /// 根据ID获取用户频道权限
        /// </summary>
        /// <param name="KeyID"></param>
        /// <returns></returns>
        public Sys_UserJurisdiction GetByID(int? KeyID)
        {
            return ObjEntity.Sys_UserJurisdiction.FirstOrDefault(C => C.JurisdictionID == KeyID);
        }

        public List<Sys_UserJurisdiction> GetByIndex(int PageSize, int PageIndex, out int SourceCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加录入客户信息
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Insert(Sys_UserJurisdiction ObjectT)
        {
            if (ObjectT != null)
            {
                var UserJurisdictionModel = ObjEntity.Sys_UserJurisdiction.FirstOrDefault(C => C.ChannelID == ObjectT.ChannelID && C.EmployeeID == ObjectT.EmployeeID);
                if (UserJurisdictionModel == null)
                {
                    ObjEntity.Sys_UserJurisdiction.Add(ObjectT);
                    if (ObjEntity.SaveChanges() > 0)
                    {
                        return ObjectT.JurisdictionID;
                    }
                }
                else
                {
                    UserJurisdictionModel.IsClose = ObjectT.IsClose;
                    UserJurisdictionModel.ChecksEmployee = ObjectT.ChecksEmployee;
                    UserJurisdictionModel.DataPower = ObjectT.DataPower;
                    UserJurisdictionModel.Dispatching = ObjectT.Dispatching;
                    this.Update(UserJurisdictionModel);
                }

            }
            return 0;
        }


        /// <summary>
        /// 修改客户频道权限信息
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Update(Sys_UserJurisdiction ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.SaveChanges();
                return ObjectT.JurisdictionID;
            }
            return 0;
        }


    }
}
