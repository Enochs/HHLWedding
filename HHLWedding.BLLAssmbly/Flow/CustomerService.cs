using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.DataAssmblly.Dto;
using HHLWedding.EditoerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wuqi.Webdiyer;
using EntityFramework.Extensions;

namespace HHLWedding.BLLAssmbly.Flow
{
    public class CustomerService : BaseService<FL_Customer>
    {

        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();
        /// <summary>
        /// 分页获取客户
        /// </summary>
        /// <returns></returns>
        public List<CustomerDto> GetCustomerListByPager(int pageIndex, int pageSize, ref int rowCount, AspNetPager CtrPagerIndex, string name, string phone, int state, int hotelId)
        {
            IQueryable<CustomerDto> query = (from c in ObjEntity.FL_Customer
                                             join d in ObjEntity.FL_Invite on c.CustomerID equals d.CustomerID into ignore
                                             from ig in ignore.DefaultIfEmpty()
                                             select new CustomerDto
                                             {
                                                 CustomerId = c.CustomerID,
                                                 Bride = c.Bride,
                                                 Groom = c.Groom,
                                                 Operator = c.Operator,
                                                 BridePhone = c.BridePhone,
                                                 GroomPhone = c.GroomPhone,
                                                 OperatorPhone = c.OperatorPhone,
                                                 ContactMan = c.ContactMan,
                                                 ContactPhone = c.ContactPhone,
                                                 PartyDate = c.PartyDate,
                                                 Hotel = c.Hotel,
                                                 SaleType = c.SaleType,
                                                 Channel = c.Channel,
                                                 CreateDate = c.CreateDate,
                                                 State = c.State,
                                                 Status = c.Status,
                                                 CreateEmployee = c.CreateEmployee,
                                                 InviteEmployee = ig.EmployeeId,
                                                 OrderEmployee = null,
                                                 QuotedEmployee = null
                                             });
            List<Expression<Func<CustomerDto, bool>>> parmList = new List<Expression<Func<CustomerDto, bool>>>();

            if (!string.IsNullOrEmpty(name))
            {
                parmList.Add(c => c.Bride.Contains(name) || c.Groom.Contains(name) || c.Operator.Contains(name));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                parmList.Add(c => c.BridePhone == phone || c.GroomPhone == phone || c.OperatorPhone == phone);
            }

            if (state > 0)
            {
                parmList.Add(c => c.State == state);
            }

            if (hotelId > 0)
            {
                parmList.Add(c => c.Hotel == hotelId.ToString());
            }
            Employee _empService = new Employee();
            List<string> employeeKey = _empService.GetMyEmployeeKey(LoginInfo.UserInfo.EmployeeId);

            if (employeeKey.Count > 0)
            {
                parmList.Add(c => employeeKey.Contains(c.CreateEmployee.ToString()));
            }




            List<CustomerDto> DataList = new BaseService<CustomerDto>().GetListByPageer<DateTime?>(query, pageIndex, pageSize, ref rowCount, parmList, c => c.CreateDate, false, CtrPagerIndex);
            return DataList;
        }



        public List<Customer> GetAllPageCustomer(int pageIndex, int pageSize, string name, string phone, string sort, out int count)
        {
            IQueryable<Customer> query = (from c in ObjEntity.FL_Customer
                                          join st in ObjEntity.FD_SaleType on c.SaleType equals st.SaleTypeID
                                          join ss in ObjEntity.FD_SaleSource on c.Channel equals ss.SourceId
                                          join h in ObjEntity.FD_Hotel on c.Hotel equals h.HotelID.ToString()
                                          select new Customer
                                          {
                                              CustomerId = c.CustomerID,
                                              CustomerName = c.Bride + "/" + c.Groom,
                                              Bride = c.Bride,
                                              Groom = c.Groom,
                                              Operator = c.Operator,
                                              BridePhone = c.BridePhone,
                                              GroomPhone = c.GroomPhone,
                                              OperatorPhone = c.OperatorPhone,
                                              ContactMan = c.ContactMan,
                                              ContactPhone = c.ContactPhone,
                                              PartyDate = c.PartyDate,
                                              Hotel = h.HotelName,
                                              SaleTypeName = st.SaleTypeName,
                                              SaleSourceName = ss.SourceName,
                                              CreateDate = c.CreateDate,
                                              State = c.State,
                                              IsVip = c.IsVip,
                                              Status = c.Status
                                          });

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Bride.Contains(name) || c.Groom.Contains(name) || c.Operator.Contains(name));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(c => c.BridePhone == phone || c.GroomPhone == phone || c.OperatorPhone == phone);
            }

            //返回总条数
            count = query.FutureCount();

            query = DBHelper.SortingAndPaging<Customer>(query, sort, pageIndex, pageSize);


            return query.ToList();
        }
    }
}
