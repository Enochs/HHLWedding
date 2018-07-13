using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace HHLWedding.DataAssmblly.CommonModel
{
    public class LoginInfo : Page
    {
        /// <summary>
        /// 登录客户信息
        /// </summary>
        public static UserEmployee UserInfo
        {
            get
            {

                Page pages = new Page();
                if (pages.Session != null && pages.Session["UserInfo"] != null)
                {
                    return pages.Session["UserInfo"] as UserEmployee;
                }
                else
                {
                    UserEmployee user = new UserEmployee();
                    user.EmployeeId = 1;
                    return user;
                }
                //return null;
            }
        }

    }
}
