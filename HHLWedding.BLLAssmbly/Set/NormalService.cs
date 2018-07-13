using HHLWedding.DataAssmblly.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace HHLWedding.BLLAssmbly.Set
{
    public class NormalService
    {

        //基础一般服务

        /// <summary>
        /// 获取所有沟通状态
        /// </summary>
        /// <returns></returns>
        public List<ListItem> GetAllState()
        {
            CustomerState cusState = CustomerState.NoInvite;
            var stateList = cusState.GetSelectList("所有状态");
            return stateList;
        }
    }
}
