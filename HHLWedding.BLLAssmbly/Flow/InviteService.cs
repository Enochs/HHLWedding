using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.ToolsLibrary;
using System.Linq.Expressions;
using HHLWedding.DataAssmblly.Dto;
using EntityFramework.Extensions;

namespace HHLWedding.BLLAssmbly.Flow
{


    public class InviteService : BaseService<FL_Invite>
    {
        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();

        /// <summary>
        /// 分页获取客户(邀约状态: 1.未邀约 2.邀约中 3.邀约成功)
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<InviteDto> GetAllByPage(int pageIndex, int pageSize, List<Expression<Func<InviteDto, bool>>> parmList, string sort, out int count)
        {
            var query = (from c in ObjEntity.FL_Invite
                         join d in ObjEntity.FL_Customer on c.CustomerID equals d.CustomerID
                         join st in ObjEntity.FD_SaleType on d.SaleType equals st.SaleTypeID
                         join ss in ObjEntity.FD_SaleSource on d.Channel equals ss.SourceId
                         join h in ObjEntity.FD_Hotel on d.Hotel equals h.HotelID.ToString()
                         join e in ObjEntity.Sys_Employee on c.EmployeeId equals e.EmployeeID
                         select new InviteDto
                         {
                             InviteId = c.InviteId,
                             CustomerId = d.CustomerID,
                             Bride = d.Bride,
                             Groom = d.Groom,
                             Operator = d.Operator,
                             CustomerName = (!string.IsNullOrEmpty(d.Bride) ? d.Bride + (d.Groom != "" ? "/" + d.Groom : "")    //新娘/新郎(可能为空)
                             : (!string.IsNullOrEmpty(d.Groom)
                             ? d.Groom              //只有新郎姓名
                             : (!string.IsNullOrEmpty(d.Operator) ? "经办人：" + d.Operator : ""         //新娘、新郎姓名都为空  只显示经办人
                             ))),
                             ContactPhone = d.ContactPhone,
                             PartyDate = d.PartyDate,
                             InviteEmployee = c.EmployeeId,
                             InviteEmpName = e.EmployeeName,
                             FollowCount = string.IsNullOrEmpty(c.FollowCount.ToString()) ? 0 : c.FollowCount,
                             Hotel = h.HotelName,
                             SaleType = d.SaleType,
                             SaleSource = d.Channel,
                             SaleTypeName = st.SaleTypeName,
                             SaleSourceName = ss.SourceName,
                             IsVip = d.IsVip,
                             State = d.State,
                             Status = d.Status,
                             CreateDate = c.CreateDate,
                             NextFollowDate = c.NextFollowDate,
                             LastFollowDate = c.LastFollowDate
                         });


            if (parmList != null)
            {
                foreach (var parm in parmList)
                {
                    query = query.Where(parm);
                }
            }

            //返回总条数
            count = query.FutureCount();

            query = DBHelper.SortingAndPaging<InviteDto>(query, sort, pageIndex, pageSize);
            return query.ToList();
        }


        /// <summary>
        /// @author：wp
        /// @datetime:2017-01-13
        /// @desc:根据邀约Id获取沟通记录
        /// </summary>
        /// <param name="InviteId">邀约ID</param>
        /// <returns></returns>
        public List<FL_InviteDetails> GetDataListById(int InviteId)
        {
            return ObjEntity.FL_InviteDetails.Where(c => c.InviteId == InviteId).ToList();
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2017-02-28
        /// @desc:根据客户Id获取沟通记录
        /// </summary>
        /// <param name="InviteId">邀约ID</param>
        /// <returns></returns>
        public List<FL_InviteDetails> GetDataListByCustomerId(Guid CustomerId)
        {
            return ObjEntity.FL_InviteDetails.Where(c => c.CustomerId == CustomerId).ToList();
        }

        #region 根据客户Id获取邀约信息
        /// <summary>
        /// @author：wp
        /// @datetime:2017-01-23
        /// @desc: 根据客户Id获取邀约信息
        /// </summary>
        /// <param name="customerId">客户Id</param>
        /// <returns></returns>
        public FL_Invite GetByCustomerId(Guid customerId)
        {
            return ObjEntity.FL_Invite.FirstOrDefault(c => c.CustomerID == customerId);
        }
        #endregion
    }
}
