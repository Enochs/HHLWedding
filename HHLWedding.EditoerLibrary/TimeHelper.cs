using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.EditoerLibrary
{
    public static class TimeHelper
    {
        /// <summary>
        /// 获取开始时间
        /// </summary>
        public static DateTime GetStartTime(object start)
        {
            if (start != null)
            {
                if (!string.IsNullOrEmpty(start.ToString()))
                {
                    return start.ToString().ToDateTime().ToString("yyyy-MM-dd 00:00:00").ToDateTime();
                }
                {
                    return DateTime.Now.AddYears(-50);
                }
            }
            else
            {
                return DateTime.Now.AddYears(-50);
            }
        }


        /// <summary>
        /// 获取结束时间
        /// </summary>
        public static DateTime GetEndTime(object end)
        {
            if (end != null)
            {
                if (!string.IsNullOrEmpty(end.ToString()))
                {
                    return end.ToString().ToDateTime().ToString("yyyy-MM-dd 23:59:59").ToDateTime();
                }
                else
                {
                    return DateTime.Now.AddYears(50);
                }
            }
            else
            {
                return DateTime.Now.AddYears(50);
            }
        }


        /// <summary>
        /// 获取婚期  开始时间/结束时间
        /// </summary>
        public static void GetPartyDateTime(string partyDate, out DateTime start, out DateTime end)
        {
            if (!string.IsNullOrEmpty(partyDate))
            {
                string[] time = partyDate.Split(',');
                start = GetStartTime(time[0]);
                end = GetEndTime(time[1]);
            }
            else
            {
                start = DateTime.Now.AddYears(-50);
                end = DateTime.Now.AddYears(50);
            }
        }
    }
}
