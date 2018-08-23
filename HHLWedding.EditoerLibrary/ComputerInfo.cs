using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net;
using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace HHLWedding.EditoerLibrary
{
    /// @author      ：wp
    /// @datetime    ：2018/7/24 17:29:47
    /// @desc        ：ComputerInfo  
    /// @lastAuthor  ：wp
    /// @lastDatetime：2018/7/24 17:29:47 
    public class ComputerInfo
    {

        public static string CpuID; //1.cpu序列号
        public static string MacAddress; //2.mac序列号
        public static string DiskID; //3.硬盘id
        public static string IpAddress; //4.ip地址
        public static string LoginUserName; //5.登录用户名
        public static string ComputerName; //6.计算机名
        public static string SystemType; //7.系统类型
        public static string TotalPhysicalMemory; //8.内存量 单位：M
        public static string WIp;   //9.外网IP
        public static string Address;   //地理位置

        static ComputerInfo()
        {
            CpuID = GetCpuID();
            MacAddress = GetMacAddress();
            DiskID = GetDiskID();
            IpAddress = GetIPAddress();
            LoginUserName = GetUserName();
            SystemType = GetSystemType();
            TotalPhysicalMemory = GetTotalPhysicalMemory();
            ComputerName = GetComputerName();
            WIp = GetIP()[0];
            Address = GetIP()[1];
        }
        //1.获取CPU序列号代码 

        static string GetCpuID()
        {
            try
            {
                string cpuInfo = "";//cpu序列号 
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //2.获取网卡硬件地址 

        static string GetMacAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //3.获取硬盘ID 
        static string GetDiskID()
        {
            try
            {
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        //4.获取IP地址 

        static string GetIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        //st=mo["IpAddress"].ToString(); 
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        /// 5.操作系统的登录用户名 
        static string GetUserName()
        {
            try
            {
                string un = "";

                un = Environment.UserName;
                return un;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }



        //6.获取计算机名
        static string GetComputerName()
        {
            try
            {
                return System.Environment.MachineName;

            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }



        ///7 PC类型 
        static string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["SystemType"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }
        }



        ///8.物理内存        
        static string GetTotalPhysicalMemory()
        {
            try
            {

                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    st = mo["TotalPhysicalMemory"].ToString();

                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        ///9.获取外网IP及地理位置     

        public static List<string> GetIP()     //0.IP 1.地址
        {
            List<string> tempList = new List<string>();
            string tempIp = "";
            string tempAddress = "";
            try
            {
                //    //获取本机外网ip的url
                //string getIpUrl = "http://www.ipip.net/ip.html";//网上获取ip地址的网站
                string getIpUrl = "http://apis.map.qq.com/ws/location/v1/ip?key=MAVBZ-RQXRF-D5YJV-J46RA-VTMFS-LFFF5";
                WebRequest wr = WebRequest.Create(getIpUrl);
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.UTF8);
                string jsonText = sr.ReadToEnd(); //读取网站的数据

                sr.Close();
                s.Close();

                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonText);
                if (ja["status"].ToString() == "0")
                {
                    tempIp = ja["result"]["ip"].ToString();
                    var nation = ja["result"]["ad_info"]["nation"].ToString();
                    var province = ja["result"]["ad_info"]["province"].ToString();
                    var city = ja["result"]["ad_info"]["city"].ToString();
                    var distinct = ja["result"]["ad_info"]["district"].ToString();

                    tempAddress = nation + " " + province + " " + city + " " + distinct;

                    tempList.Add(tempIp);
                    tempList.Add(tempAddress);

                }
                

            }
            catch (Exception e)
            {
                throw e;
            }
            return tempList;
        }

        /// <summary>
        /// 淘宝api
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static string GetIPCitys(string strIP)
        {
            try
            {
                string Url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + strIP;

                WebRequest wr = WebRequest.Create(Url);
                //WebResponse wResp = wr.GetResponse();
                //Stream respStream = wResp.GetResponseStream();
                Stream s = wr.GetResponse().GetResponseStream();
                //StreamReader sr = new StreamReader(s, Encoding.UTF8);
                StreamReader sr = new StreamReader(s, Encoding.UTF8);

                string jsonText = sr.ReadToEnd();
                sr.Close();
                s.Close();

                JObject ja = (JObject)JsonConvert.DeserializeObject(jsonText);
                if (ja["code"].ToString() == "0")
                {
                    string c = ja["data"]["city"].ToString();
                    int ci = c.IndexOf('市');
                    if (ci != -1)
                    {
                        c = c.Remove(ci, 1);
                    }
                    return c;
                }
                else
                {
                    return "未知";
                }

            }
            catch (Exception e)
            {
                return ("未知");
                throw e;

            }
        }

    }
}
