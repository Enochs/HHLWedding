using HHLWedding.BLLAssmbly.Sys;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.Web.AdminWorkArea.CommonForm
{
    /// <summary>
    /// fileupload 的摘要说明
    /// </summary>
    public class fileupload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            if (context.Request["REQUEST_METHOD"] == "OPTIONS")
            {
                context.Response.End();
            }
            SaveFile(context);
        }

        /// <summary>
        /// 文件保存操作
        /// </summary>
        /// <param name="basePath"></param>
        private void SaveFile(HttpContext context, string basePath = "/Files/")
        {
            HttpServerUtility server = context.Server;
            HttpRequest request = context.Request;
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

            string path = basePath;
            string type = string.Empty;
            if (!string.IsNullOrEmpty(request["isType"]))       //跟路径
            {
                type = request["isType"].ToString();
                basePath = ConfigurationManager.AppSettings[type].ToString();
                path = server.MapPath(basePath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            if (!string.IsNullOrEmpty(request["isName"]))           //子路径
            {
                string fname = request["isName"].ToString();
                path = path + fname;
                basePath = basePath + fname;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            //后缀名
            var suffix = files[0].ContentType.Split('/')[1];
            //原始图片名称
            //var _temp = System.Web.HttpContext.Current.Request["name"];
            //图片名称
            Random rand = new Random(24 * (int)DateTime.Now.Ticks);
            string name = rand.Next() + "." + suffix;

            //保存全路径
            string normalPath = basePath + "/" + name;
            string fullPath = path + "/" + name;

            //添加到数据库
            CommonImageService _imgService = new CommonImageService();
            if (type == "hotelUrl")
            {
                CommonImages commonImg = new CommonImages();
                commonImg.ImgName = name;
                commonImg.ImgUrl = normalPath;
                commonImg.TypeId = 1;       //1.代表酒店
                commonImg.CommonId = request["isId"].ToInt32();     //酒店Id
                commonImg.CreateDate = DateTime.Now;
                commonImg.State = 0;

                _imgService.Insert(commonImg);
            }

            files[0].SaveAs(fullPath);

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}