using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.ToolsLibrary;
using System.Web.UI.WebControls;

namespace HHLWedding.EditoerLibrary
{
    public static class DisplayNameExtension
    {
        /// <summary>
        /// 获取枚举的备注信息
        /// </summary>
        /// <param name="val">枚举值</param>
        /// <returns></returns>
        //public static string GetDisplayName(this Enum enumValue)
        //{
        //    string str = enumValue.ToString();
        //    System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
        //    object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
        //    if (objs == null || objs.Length == 0) return str;
        //    System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
        //    return da.Description;
        //}

        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumVal)
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

        /// <summary>
        /// 获取displayName
        /// </summary>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public static string GetDisplayNames(this Enum enumVal, string value)
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
        /// <param name="enumObj"></param>
        /// <returns></returns>

        public static List<ListItem> GetSelectList(this Enum enumVal, Type type, bool isDefault = true)
        {

            List<ListItem> list = new List<ListItem>();

            foreach (int myCode in Enum.GetValues(type))
            {
                string value = Enum.GetName(type, myCode);      //获取英文名称

                string strVaule = myCode.ToString();            //获取值
                string strName = GetDisplayNames(enumVal, value);       //获取Display名称

                ListItem item = new ListItem { Text = strName, Value = strVaule };
                list.Add(item);
            }
            if (isDefault)
            {
                list.Insert(0, new ListItem { Text = "请选择", Value = "-1" });
            }
            return list;
        }
        #endregion

    }


}
