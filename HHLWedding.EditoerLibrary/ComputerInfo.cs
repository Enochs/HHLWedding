using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net;
using System.IO;
using Microsoft.Win32;

namespace HHLWedding.EditoerLibrary
{
    /// @author      ：qjhang
    /// @datetime    ：2018/7/24 17:29:47
    /// @desc        ：ComputerInfo  
    /// @lastAuthor  ：qjhang
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
            Address=GetIP()[1];
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

        static List<string> GetIP()     //0.IP 1.地址
        {
            List<string> tempList = new List<string>();
            string tempip = "";
            string tempAddress = "";
            try
            {
                //获取本机外网ip的url
                string getIpUrl = "http://www.ipip.net/ip.html";//网上获取ip地址的网站
                WebRequest wr = WebRequest.Create(getIpUrl);
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.UTF8);
                string all = sr.ReadToEnd(); //读取网站的数据
                                             //解析出需要的数据
                int start = all.IndexOf("<td>当前IP</td>");
                int end = all.IndexOf("</span></td>");
                tempip = all.Substring(start, end - start).Replace("<td>当前IP</td>\r\n            <td colspan=\"6\"><span style=\"margin-left: -200px;\">", "");
                tempList.Add(tempip);

                int addresstart = all.LastIndexOf("<td>地理位置</td>");
                int addersSEnd = addresstart + 121; //all.IndexOf("</span>");
                tempAddress = all.Substring(addresstart, addersSEnd - addresstart).Replace("<td>地理位置</td>\r\n                <td colspan=\"6\" style=\"position: relative;\"><span style=\"margin-left: -200px;\">", "");
                tempList.Add(tempAddress);
                sr.Close();
                s.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return tempList;
        }

    }
}
