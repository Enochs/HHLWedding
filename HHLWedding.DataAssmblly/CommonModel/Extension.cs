using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace HHLWedding.DataAssmblly.CommonModel
{
    public static class Extension
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumVal)
        {
            if (enumVal == null) return "无效";
            string enumStr = enumVal.ToString();
            var type = enumVal.GetType();
            string[] enumValues = { enumStr };
            if (enumStr.Contains(","))
            {
                enumValues = enumStr.Split(',').Select(i => i.Trim()).ToArray();
            }

            string showName = "";
            foreach (var item in enumValues)
            {

                var menInfo = type.GetMember(item);
                if (menInfo == null || menInfo.Count() == 0)
                {
                    showName = enumVal.ToString();
                }
                else
                {
                    var attributes = menInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                    DisplayAttribute attribute = (attributes.Length > 0) ? (DisplayAttribute)attributes[0] : null;
                    showName = attribute == null ? enumVal.ToString() : attribute.Name;
                }
            }

            return showName.ToString();
        }



        public static string GetPlayName<TEnum>(this TEnum enumVal, string value)
        {
            if (enumVal == null)
            {
                return "无效";
            }

            string showName = "";
            var type = enumVal.GetType();
            var menInfo = type.GetMember(value);

            if (menInfo == null || menInfo.Count() == 0)
            {
                showName = enumVal.ToString();
            }
            else
            {
                var attributes = menInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                DisplayAttribute attribute = (attributes.Length > 0) ? (DisplayAttribute)attributes[0] : null;
                showName = attribute == null ? enumVal.ToString() : attribute.Name;
            }

            return showName.ToString();
        }

        #region 获取list 绑定下拉框
        /// <summary>
        /// 获取listItem
        /// </summary>
        /// <param name="TEnum">枚举类型</param>
        /// <param name="enumVal">枚举值</param>
        /// <param name="isDefault">是否包含请选择项 true. 包含   false  不包含(默认true)</param>
        /// <returns></returns>

        public static List<ListItem> GetSelectList<TEnum>(this TEnum enumVal, string text = "请选择", bool isDefault = true)
        {

            List<ListItem> list = new List<ListItem>();

            //var query = from TEnum status in Enum.GetValues(typeof(TEnum))
            //            select new ListItem()
            //            {
            //                Text = (status as Enum).GetDisplayName(),
            //                Value = (Enum.GetValues(typeof(TEnum))).ToString(),       //获取的这是英文名称
            //                Selected = enumVal == null ? false : enumVal.ToString().Contains(status.ToString())
            //            };

            //list.AddRange(query);

            Type type = typeof(TEnum);
            foreach (int myCode in Enum.GetValues(type))
            {
                string value = Enum.GetName(type, myCode);      //获取英文名称

                string strVaule = myCode.ToString();            //获取值
                string strName = enumVal.GetPlayName(value);       //获取Display名称

                ListItem item = new ListItem { Text = strName, Value = strVaule };
                list.Add(item);
            }

             if (isDefault)
            {
                list.Insert(0, new ListItem { Text = text, Value = "-1" });
            }

            return list;
        }
        #endregion



        #region 类型转换

        private static int ToInt32(this string Value)
        {
            int IntValue = 0;
            int.TryParse(Value, out IntValue);
            return IntValue;

        }

        #endregion
    }
}
