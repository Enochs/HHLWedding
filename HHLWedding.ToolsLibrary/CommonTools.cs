using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHLWedding.ToolsLibrary
{
    public static class CommonTools
    {
        #region 获取新人姓名
        /// <summary>
        /// 获取新人姓名(新娘/新郎)
        /// </summary>
        /// <param name="Bride">新娘姓名</param>
        /// <param name="Groom">新郎姓名</param>
        /// <returns></returns>
        public static string GetCustomerName(string Bride, string Groom)
        {
            string customerName = string.Empty;
            if (!string.IsNullOrEmpty(Bride))
            {
                customerName = Bride + "/";
            }

            if (!string.IsNullOrEmpty(Bride))
            {
                customerName += Groom;
            }
            return customerName;
        }
        #endregion

        #region 根据key获取form表单的值
        /// <summary>
        /// 获取form表单中的控件值
        /// </summary>
        /// <param name="form">表单名称</param>
        /// <param name="keyWords">控件name名称</param>
        /// <returns></returns>
        public static string GetKeyValue(System.Collections.Specialized.NameValueCollection form, string keyWords)
        {
            string resultValue = "";

            try
            {
                foreach (var item in form.AllKeys)
                {
                    if (item.EndsWith(keyWords))
                    {
                        resultValue = form[item];
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultValue;
        }
        #endregion
    }
}
