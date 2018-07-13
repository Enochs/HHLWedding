using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.BLLAssmbly.PublicTools
{

    public static class CommonService
    {



        private static HHL_WeddingEntities dbContext = new HHL_WeddingEntities();
        /// <summary>
        /// author:wp
        /// datetime:2016-07-02
        /// des: 获取code规则
        /// </summary>
        /// <param name="codeLenth"></param>
        /// <returns></returns>
        public static string getCode(string parentCode, int codeLenth, int itemLevel)
        {

            string str_fix = "";
            int count = 0;
            if (itemLevel == 0)     //生成父级
            {
                count = dbContext.Sys_Channel.Where(c => c.Parent == 0).Count() + 1;
                int length = codeLenth - count.ToString().Length;
                for (int i = 0; i < length; i++)
                {
                    str_fix += "0";
                }
            }
            else
            {
                count = dbContext.Sys_Channel.Where(c => c.IndexCode.StartsWith(parentCode) && c.Parent != 0).Count() + 1;

                //补齐位数
                int length = codeLenth - count.ToString().Length;
                for (int i = 0; i < length; i++)
                {
                    str_fix += "0";
                }
            }

            return parentCode + str_fix + count.ToString();

        }
    }
}
