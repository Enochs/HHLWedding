using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.BLLAssmbly;
using System.Linq.Expressions;
using HHLWedding.DataAssmblly.CommonModel;
using Wuqi.Webdiyer;

namespace HHLWedding.BLLManager
{


    public class MessageManager
    {
        BaseService<sm_Message> _msgService = new BaseService<sm_Message>();
        Employee _empService = new Employee();

        /// <summary>
        /// 获取邮件  倒序排序
        /// </summary>
        /// <param name="type"> 1. 收件箱   2.发件箱</param>
        /// <param name="isType"></param>
        /// <returns></returns>
        public List<sm_Message> GetAllByPager(int pageIndex, int pageSize, string search, string EmployeeName, ref int sourceCount, AspNetPager CtrPager, int type = 1, int isType = 0)
        {
            List<Expression<Func<sm_Message, bool>>> pars = new List<Expression<Func<sm_Message, bool>>>();

            int EmployeeId = LoginInfo.UserInfo.EmployeeId;

            if (!string.IsNullOrEmpty(search))
            {
                pars.Add(c => c.MessageTitle.Contains(search) || c.MessageContent.Contains(search));
            }

            if (!string.IsNullOrEmpty(EmployeeName))
            {
                var employee = _empService.GetByLikeName(EmployeeName);
                if (type == 1)
                {
                    pars.Add(c => c.FromEmployee == employee.EmployeeID);
                }
                else
                {
                    pars.Add(c => c.ToEmployee.Contains(employee.EmployeeID.ToString()));
                }
            }

            if (type == 1)
            {
                pars.Add(c => EmployeeId.ToString().Contains(c.ToEmployee) && c.SendType == type);
                //重要邮件
                if (isType == 1)
                {
                    pars.Add(c => c.IsGarbage == isType);
                }
            }
            else
            {
                pars.Add(c => c.FromEmployee == EmployeeId && c.SendType == type);

                //草稿
                if (isType == 1)
                {
                    pars.Add(c => c.IsDraft == isType);
                }
            }

            //倒序排序
            return _msgService.GetPagedList<DateTime?>(pageIndex, pageSize, ref sourceCount, pars, c => c.SendDateTime, false, CtrPager);
        }
    }
}
