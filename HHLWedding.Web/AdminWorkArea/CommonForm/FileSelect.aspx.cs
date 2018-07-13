using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HHLWedding.Web.AdminWorkArea.CommonForm
{
    public partial class FileSelect : System.Web.UI.Page
    {

        #region 页面加载
        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BinderData();
            }
        }
        #endregion


        #region 文件加载
        /// <summary>
        /// 数据文件加载
        /// </summary>
        public void BinderData()
        {
            string path = Server.MapPath("/images/"); //文件夹路径
            string[] paths = Directory.GetFiles(path); //获取文件夹下全部文件路径
            List<FileInfo> files = new List<FileInfo>();
            foreach (string filepath in paths)
            {
                FileInfo file = new FileInfo(filepath); //获取单个文件
                files.Add(file);
            }
            //files[0]./*DirectoryName*/
            //files[0].Directory.Parent + "/" + files[0].Directory.Name + "/" + files[0].Name
            repFile.DataSource = files;
            repFile.DataBind();
        }
        #endregion
    }
}