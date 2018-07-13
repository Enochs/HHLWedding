using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HHLWedding.ToolsLibrary;
using EntityFramework.Extensions;
using HHLWedding.DataAssmblly.CommonModel;

namespace HHLWedding.BLLAssmbly.Flow
{
    public class OrderService : BaseService<FL_Order>
    {
        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();


        /// <summary>
        /// @author：wp
        /// @date：2017-01-13
        /// @desc:根据客户获取订单
        /// </summary>
        /// <returns></returns>
        public FL_Order GetByCustomerId(Guid CustomerId)
        {
            var m_order = ObjEntity.FL_Order.FirstOrDefault(c => c.CustomerId == CustomerId);
            return m_order;
        }


        /// <summary>
        /// @author：wp
        /// @date：2017-02-27
        /// @desc: 分页获取订单信息
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        public List<OrderDto> GetAllByPage(int page, int pageSize, ref int souceCount, string name, string phone, string orderEmployee)
        {
            IQueryable<OrderDto> query = (from d in ObjEntity.FL_Customer
                                          join c in ObjEntity.FL_Order on d.CustomerID equals c.CustomerId
                                          select new OrderDto
                                          {
                                              OrderId = c.OrderID,
                                              OrderCoder = c.OrderCoder,
                                              CustomerId = d.CustomerID,
                                              Bride = d.Bride,
                                              Groom = d.Groom,
                                              Operator = d.Operator,
                                              CustomerName = (!string.IsNullOrEmpty(d.Bride) ? d.Bride + (d.Groom != "" ? "/" + d.Groom : "")    //新娘/新郎(可能为空)
                                              : (!string.IsNullOrEmpty(d.Groom)
                                              ? d.Groom              //只有新郎姓名
                                              : (!string.IsNullOrEmpty(d.Operator) ? "经办人：" + d.Operator : ""         //新娘、新郎姓名都为空  只显示经办人
                                              ))),
                                              BridePhone = d.BridePhone,
                                              GroomPhone = d.GroomPhone,
                                              OperatorPhone = d.OperatorPhone,
                                              ContactPhone = d.ContactPhone,
                                              PartyDate = d.PartyDate,
                                              ComeDate = c.ComeDate,
                                              EmployeeId = c.EmployeeId,
                                              CreateEmployee = c.CreateEmployee,
                                              OrderState = d.State,
                                              HotelId = d.Hotel,
                                              SaleTypeId = d.SaleType,
                                              SaleSourceId = d.Channel,
                                              FollowCount = c.FollowCount,
                                              LastFollowDate = c.LastFollowDate,
                                              NextFollowDate = c.NextFollowDate,
                                              CreateDate = c.CreateDate
                                          });

            List<Expression<Func<OrderDto, bool>>> parmList = new List<Expression<Func<OrderDto, bool>>>();

            //新人姓名
            if (!string.IsNullOrEmpty(name))
            {
                parmList.Add(c => c.CustomerName.Contains(name));
            }

            //联系电话
            if (!string.IsNullOrEmpty(phone))
            {
                parmList.Add(c => c.BridePhone == phone || c.GroomPhone == phone || c.OperatorPhone == phone);
            }

            //跟单人
            if (!string.IsNullOrEmpty(orderEmployee) && orderEmployee.ToInt32() > 0)
            {
                parmList.Add(c => c.EmployeeId.ToString() == orderEmployee);
            }

            //状态
            parmList.Add(c => ("5,6,7,8").Contains(c.OrderState.ToString()));

            var DataList = new BaseService<OrderDto>().GetListByPageer<DateTime?>(query, page, pageSize, ref souceCount, parmList, c => c.CreateDate, false);

            return DataList;
        }



        /// <summary>
        /// @author：wp
        /// @date：2017-02-28
        /// @desc:根据客户获取跟单的沟通记录
        /// </summary>
        /// <returns></returns>
        public List<FL_OrderDetails> GetDataListByCustomerId(Guid CustomerId)
        {
            List<FL_OrderDetails> m_order = ObjEntity.FL_OrderDetails.Where(c => c.CustomerId == CustomerId).ToList();
            return m_order;
        }

        /// <summary>
        /// 分页获取客户(邀约状态: 1.未邀约 2.邀约中 3.邀约成功)
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<OrderDto> GetAllByPages(int pageIndex, int pageSize, List<Expression<Func<OrderDto, bool>>> parmList, string sort, out int count)
        {
            var query = (from c in ObjEntity.FL_Order
                         join d in ObjEntity.FL_Customer on c.CustomerId equals d.CustomerID
                         join st in ObjEntity.FD_SaleType on d.SaleType equals st.SaleTypeID
                         join ss in ObjEntity.FD_SaleSource on d.Channel equals ss.SourceId
                         join h in ObjEntity.FD_Hotel on d.Hotel equals h.HotelID.ToString()
                         join e in ObjEntity.Sys_Employee on c.EmployeeId equals e.EmployeeID
                         select new OrderDto
                         {
                             OrderId = c.OrderID,
                             OrderCoder = c.OrderCoder,
                             CustomerId = d.CustomerID,
                             Bride = d.Bride,
                             Groom = d.Groom,
                             Operator = d.Operator,
                             CustomerName = (!string.IsNullOrEmpty(d.Bride) ? d.Bride + (d.Groom != "" ? "/" + d.Groom : "")    //新娘/新郎(可能为空)
                             : (!string.IsNullOrEmpty(d.Groom)
                             ? d.Groom              //只有新郎姓名
                             : (!string.IsNullOrEmpty(d.Operator) ? "经办人：" + d.Operator : ""         //新娘、新郎姓名都为空  只显示经办人
                             ))),
                             BridePhone = d.BridePhone,
                             GroomPhone = d.GroomPhone,
                             OperatorPhone = d.OperatorPhone,
                             ContactPhone = d.ContactPhone,
                             PartyDate = d.PartyDate,
                             EmployeeId = c.EmployeeId,
                             EmployeeName = e.EmployeeName,
                             CreateEmployee = c.CreateEmployee,
                             OrderState = c.OrderState,
                             FollowCount = string.IsNullOrEmpty(c.FollowCount.ToString()) ? 0 : c.FollowCount,
                             HotelId = d.Hotel,
                             HotelName = h.HotelName,
                             SaleTypeId = st.SaleTypeID,
                             SaleSourceId = ss.SourceId,
                             SaleTypeName = st.SaleTypeName,
                             SaleSourceName = ss.SourceName,
                             ComeDate = c.ComeDate,
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

            query = DBHelper.SortingAndPaging<OrderDto>(query, sort, pageIndex, pageSize);
            return query.ToList();
        }


        /// <summary>
        /// 根据客户Id获取流失原因
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public FL_OrderDetails GetLoseOrder(Guid CustomerId)
        {
            return ObjEntity.FL_OrderDetails.Where(c => c.CustomerId == CustomerId && c.OrderState == (byte)CustomerState.LoseOrder).OrderByDescending(c => c.CreateDate).FirstOrDefault();
        }
    }
}
