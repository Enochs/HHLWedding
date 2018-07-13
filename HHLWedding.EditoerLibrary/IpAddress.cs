using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace HHLWedding.EditoerLibrary
{
    public static class IpAddress
    {
        /// <summary>
        /// 根据IP获取省市
        /// </summary>
        /// <param name="Type">类型 Type 1.返回省市   2.返回省市+区县</param>
        /// <returns></returns>
        public static string GetAddressByIp(string ip, int Type = 1)
        {
            string PostUrl = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?ip=" + ip;
            string res = GetDataByPost(PostUrl);//该条请求返回的数据为：res=1\t115.193.210.0\t115.194.201.255\t中国\t浙江\t杭州\t电信

            string[] arr = getAreaInfoList(res);

            string province = Province.GetProvince(arr[0]);
            string city = Province.GetCity(arr[1]);
            if (string.IsNullOrEmpty(city))
            {
                city = Province.GetArea(arr[1]);
            }


            if (Type == 1)
            {
                return province;
            }
            else
            {
                return province + " " + (string.IsNullOrEmpty(city) ? arr[1] : city);
            }
        }

        /// <summary>
        /// Post请求数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetDataByPost(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //string s = "anything";
            //byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(s);
            //req.Method = "POST";
            //req.ContentType = "application/x-www-form-urlencoded";
            //req.ContentLength = requestBytes.Length;
            //Stream requestStream = req.GetRequestStream();
            //requestStream.Write(requestBytes, 0, requestBytes.Length);
            //requestStream.Close();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
            string backstr = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return backstr;
        }

        /// <summary>
        /// 处理所要的数据
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string[] getAreaInfoList(string ipData)
        {
            //1\t115.193.210.0\t115.194.201.255\t中国\t浙江\t杭州\t电信
            string[] areaArr = new string[10];
            string[] newAreaArr = new string[2];
            try
            {
                //取所要的数据，这里只取省市
                areaArr = ipData.Split('\t');
                newAreaArr[0] = areaArr[4];//省
                newAreaArr[1] = areaArr[5];//市
            }
            catch (Exception e)
            {
                throw e;
                // TODO: handle exception
            }
            return newAreaArr;
        }

        /// <summary>
        /// 获取外网Ip
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string tempip = "";
            try
            {
                WebRequest wr = WebRequest.Create("http://www.ip138.com/ip2city.asp");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据

                int start = all.IndexOf("[") + 1;
                int end = all.IndexOf("]", start);
                tempip = all.Substring(start, end - start);
                sr.Close();
                s.Close();
            }
            catch
            {
            }
            return tempip;
        }


        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }


    }
}
