using HHLWedding.DataAssmblly;
using HHLWedding.EditoerLibrary;
using HHLWedding.EditoerLibrary.Extension;
using HHLWedding.Web.Handler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly.CommonModel;
using System.Web.SessionState;
using System.Transactions;
using HHLWedding.BLLAssmbly.Flow;
using HHLWedding.BLLAssmbly;

namespace HHLWedding.Web.AdminWorkArea.Handler
{
    /// <summary>
    /// ImportCustomer 的摘要说明
    /// </summary>
    public class ImportCustomer : IHttpHandler
    {

        #region 服务

        CustomerService _cusService = new CustomerService();
        BaseService<FL_Customer> _customerService = new BaseService<FL_Customer>();
        BaseService<FD_SaleSource> _saleSourceService = new BaseService<FD_SaleSource>();
        BaseService<SS_Report> _reportService = new BaseService<SS_Report>();
        BaseService<FL_Invite> _inviteService = new BaseService<FL_Invite>();
        BaseService<FL_InviteDetails> _inviteDetailsService = new BaseService<FL_InviteDetails>();
        BaseService<FL_Order> _orderService = new BaseService<FL_Order>();

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/octet-stream";
            //HttpPostedFile imgFile = context.Request.Files["upFile"];           //upFile   就是input的name
            ImportFile(context);

        }


        /// <summary>
        /// 导入客户文件
        /// </summary>

        public void ImportFile(HttpContext context)
        {
            //获取上传的excel文件
            HttpFileCollection file = HttpContext.Current.Request.Files;
            int employeeId = context.Request.Form["hideEmployee"].ToInt32();        //获取当前登录员工的id

            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;

            string pathAddress = "";        //完整的excel保存路径
            string type = "success";          //成功状态
            string suffix = "";             //后缀名
            try
            {



                if (file.Count == 0)
                {
                    //直接点击导入 没有选择Excel文件  
                    ajax.Message = "文件不能为空,请选择上传文件";
                    type = "error";
                }
                else
                {
                    HttpPostedFile upFile = file[0];

                    #region 判断文件类型 大小 保存文件

                    if (type == "success")
                    {
                        int filesize = upFile.ContentLength;                                            //excel文件大小
                        int Maxsize = 4000 * 1024;                                                      //最大空间大小为4M
                        string filename = DateTime.Now.ToString("HHmmssfff") + upFile.FileName;         //文件名
                        string path = HttpContext.Current.Server.MapPath("/Template/");                 //文件夹路径
                        pathAddress = path + filename;                                                  //完整的excel保存路径
                        suffix = Path.GetExtension(pathAddress).ToString();                             //获取后缀名


                        if (suffix != ".xls" && suffix != ".xlsx")
                        {
                            ajax.Message = "上传文件必须是xlsx或xls文件";
                            type = "error";
                        }
                        else
                        {
                            if (filesize > Maxsize)
                            {
                                ajax.Message = "文件大小不能超过4M";
                                type = "error";
                            }
                            else
                            {
                                //判断文件夹是否存在  若不存在 就新建
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                //保存Excel
                                upFile.SaveAs(pathAddress);

                                type = "success";
                            }
                        }
                    }
                    #endregion

                    #region 验证excel模板(根据标题验证)
                    //获取excel的数据  用DataTable保存
                    DataTable dt = null;
                    DataRow rowTitle = null;
                    if (type == "success")
                    {
                        dt = ExcelUtil.ExcelToDataTable(pathAddress, "getTitle");
                        rowTitle = dt.Rows[0];
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (rowTitle[0].ToString() != "新娘姓名" || rowTitle[1].ToString() != "新郎姓名" || rowTitle[2].ToString() != "经办人" || rowTitle[3].ToString() != "新娘联系电话" || rowTitle[4].ToString() != "新郎联系电话" || rowTitle[5].ToString() != "经办人联系电话" || rowTitle[6].ToString() != "婚期" || rowTitle[7].ToString() != "酒店" || rowTitle[8].ToString() != "渠道类型" || rowTitle[9].ToString() != "渠道名称" || rowTitle[10].ToString() != "推荐人" || rowTitle[11].ToString() != "沟通进度")
                                {
                                    ajax.Message = "当前文件模板错误,请参考当前页面的模板进行使用";
                                    type = "errorr";
                                }
                                else if (dt.Rows.Count == 1)
                                {
                                    ajax.Message = "当前上传文件中没有客户信息,请填写客户信息";
                                    type = "errorr";
                                }
                            }
                        }
                        else
                        {
                            ajax.Message = "请不要上传空白的Excel";
                            type = "errorr";
                        }
                    }
                    #endregion

                    #region 循环验证excel里的内容
                    if (type == "success")
                    {
                        if (dt != null)
                        {
                            #region 验证内容是否完整

                            //记录列名
                            string columnName = "";

                            //循环遍历datatable里的数据 进行验证
                            for (int i = 1; i < dt.Rows.Count; i++)
                            {

                                //table中的每一行
                                DataRow row = dt.Rows[i];
                                //验证新人姓名是否为空
                                if (row[0].ToString() == "" && row[1].ToString() == "" && row[2].ToString() == "")
                                {
                                    ajax.Message += "新人姓名 至少一列不能为空,请补全信息 \n\r";
                                    type = "error";
                                }

                                if (row[3].ToString() == "" && row[4].ToString() == "" && row[5].ToString() == "")
                                {
                                    ajax.Message += "新人联系电话 至少一列不能为空,请补全信息";
                                    type = "error";
                                }

                                if (type == "success")
                                {
                                    for (int j = 8; j <= 10; j++)
                                    {

                                        if (row[j].ToString() == "")
                                        {
                                            columnName += rowTitle[j].ToString() + ",";
                                        }

                                        if (j == dt.Columns.Count - 1)
                                        {
                                            columnName = columnName.Substring(0, columnName.Length - 1).ToString();
                                            ajax.Message += "第" + (i + 1) + "行," + columnName + "列不能为空,";
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Excel内容完整 判断Excel信息 (进行验证)

                            //Excel内容完整
                            if (type == "success")
                            {
                                for (int i = 1; i < dt.Rows.Count; i++)
                                {
                                    //table中的每一行
                                    DataRow row = dt.Rows[i];
                                    for (int j = 3; j <= 5; j++)
                                    {
                                        //新人电话验证
                                        if (row[j].ToString() != string.Empty)
                                        {
                                            if (row[j].ToString().Length != 11 || RegexExtension.IsValidPhone(row[j].ToString()) == false)
                                            {
                                                ajax.Message = "Excel文件中 " + rowTitle[j] + "格式不正确";
                                                type = "error";
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 添加客户至数据库
                    if (type == "success")
                    {
                        FL_Customer m_customer = new FL_Customer();
                        List<FL_Customer> cus_list = new List<FL_Customer>();

                        int nums = 0;
                        if (dt.Rows.Count >= 2)
                        {
                            List<FL_Customer> customer_list = new List<FL_Customer>();
                            List<FL_Invite> invite_list = new List<FL_Invite>();
                            List<SS_Report> report_list = new List<SS_Report>();

                            for (int i = 1; i < dt.Rows.Count; i++)
                            {
                                DataRow row = dt.Rows[i];
                                m_customer = new FL_Customer();
                                m_customer.CustomerID = Guid.NewGuid();
                                m_customer.Bride = row[0].ToString();
                                m_customer.Groom = row[1].ToString();
                                m_customer.Operator = row[2].ToString();
                                m_customer.BridePhone = row[3].ToString();
                                m_customer.GroomPhone = row[4].ToString();
                                m_customer.OperatorPhone = row[5].ToString();

                                //主要联系人
                                if (row[0].ToString() != string.Empty)
                                {
                                    m_customer.ContactMan = row[0].ToString();
                                }
                                else if (row[1].ToString() != string.Empty)
                                {
                                    m_customer.ContactMan = row[1].ToString();
                                }
                                else if (row[2].ToString() != string.Empty)
                                {
                                    m_customer.ContactMan = row[2].ToString();
                                }

                                //主要联系电话
                                if (row[3].ToString() != string.Empty)
                                {
                                    m_customer.ContactPhone = row[3].ToString();
                                }
                                else if (row[4].ToString() != string.Empty)
                                {
                                    m_customer.ContactPhone = row[4].ToString();
                                }
                                else if (row[5].ToString() != string.Empty)
                                {
                                    m_customer.ContactPhone = row[5].ToString();
                                }

                                m_customer.PartyDate = DateTime.FromOADate(double.Parse(row[6].ToString())).ToString("yyyy-MM-dd").ToDateTime();
                                m_customer.Hotel = row[7].ToString().Substring(0, row[7].ToString().IndexOf('.'));
                                int length = row[8].ToString().Substring(row[8].ToString().LastIndexOf("M") + 1).Length;
                                m_customer.SaleType = row[8].ToString().Substring(row[8].ToString().LastIndexOf('M') + 1, length).ToString().ToInt32();
                                m_customer.Channel = row[9].ToString().Substring(0, row[9].ToString().IndexOf('.')).ToString().ToInt32();
                                m_customer.ReCommand = row[10].ToString();
                                m_customer.State = row[11].ToString().Substring(0, row[11].ToString().IndexOf('.')).ToInt32();      //沟通状态
                                m_customer.DeskCount = 0;
                                m_customer.Budget = 0;
                                m_customer.Type = 1;                  //1.婚宴  2.生日宴   3.宝宝宴
                                m_customer.BanqueType = "1";          //1.午宴   2.晚宴
                                m_customer.IsVip = 1;                 //0.普通客户  1.会员客户
                                m_customer.CreateEmployee = employeeId;
                                m_customer.CreateDate = DateTime.Now;
                                m_customer.IsFinish = 0;            //是否完成
                                m_customer.EvalState = 0;           //是否评价
                                m_customer.Status = (byte)SysStatus.Enable;
                                m_customer.Description = string.Empty;


                                //Report 信息
                                SS_Report report = new SS_Report();

                                report.CustomerId = m_customer.CustomerID;
                                report.CreateEmployee = employeeId;
                                report.InviteEmployee = employeeId;
                                report.CreateDate = DateTime.Now;
                                report.CustomerState = m_customer.State;


                                //邀约信息
                                FL_Invite m_invite = new FL_Invite();
                                m_invite.CustomerID = m_customer.CustomerID;
                                m_invite.EmployeeId = employeeId;
                                m_invite.CreateEmployee = employeeId;
                                m_invite.CreateDate = DateTime.Now;
                                m_invite.IsLose = false;

                                FL_InviteDetails details = new FL_InviteDetails();
                                FL_Order order = new FL_Order();
                                if (m_customer.State == 5)        //确认到店
                                {
                                    //邀约内容
                                    m_invite.OrderEmployee = employeeId.ToString().ToInt32();

                                    details.CustomerId = m_customer.CustomerID;
                                    details.EmployeeId = employeeId;
                                    details.InviteState = (int)CustomerState.ComeOrder;
                                    details.StateName = details.StateValue.GetDisplayName();
                                    details.InviteContent = "Excel导入客户  直接确认到店";
                                    details.CreateDate = DateTime.Now;

                                    //订单 统筹信息

                                    order.OrderID = Guid.NewGuid();
                                    order.OrderCoder = Guid.NewGuid().ToString().Replace("-", "");
                                    order.CustomerId = m_customer.CustomerID;
                                    order.ComeDate = DateTime.Now;
                                    order.EmployeeId = employeeId;
                                    order.OrderState = (int)CustomerState.ComeOrder;
                                    order.FollowCount = 0;
                                    order.CreateEmployee = employeeId;
                                    order.CreateDate = DateTime.Now;

                                    m_invite.FollowCount = 1;
                                    m_invite.OrderEmployee = employeeId.ToString().ToInt32();
                                    m_invite.LastFollowDate = DateTime.Now;

                                    report.OrderId = order.OrderID;
                                    report.OrderEmployee = employeeId.ToString().ToInt32();
                                }

                                using (TransactionScope scope = new TransactionScope())
                                {
                                    if (m_customer.State == 5)        //确认到店
                                    {
                                        //添加客户
                                        _customerService.Add(m_customer);

                                        //添加邀约
                                        FL_Invite r_invite = _inviteService.Add(m_invite);
                                        details.InviteId = r_invite.InviteId;
                                        //添加邀约内容
                                        _inviteDetailsService.Add(details);


                                        //添加到店  订单
                                        FL_Order r_order = _orderService.Add(order);

                                        //添加统计
                                        _reportService.Add(report);
                                    }
                                    else
                                    {

                                        ////添加客户
                                        //_customerService.Add(m_customer);

                                        ////添加邀约
                                        //_inviteService.Add(m_invite);
                                        ////添加统计
                                        //_reportService.Add(report);

                                        customer_list.Add(m_customer);
                                        invite_list.Add(m_invite);
                                        report_list.Add(report);

                                    }


                                    scope.Complete();
                                }

                                nums++;
                            }
                            //批量添加
                            if (customer_list.Count > 0)
                            {
                                _customerService.InsertForList(customer_list);
                            }
                            if (invite_list.Count > 0)
                            {
                                _inviteService.InsertForList(invite_list);
                            }
                            if (report_list.Count > 0)
                            {
                                _reportService.InsertForList(report_list);
                            }

                        }


                        if (type == "success")
                        {
                            ajax.IsSuccess = true;
                            ajax.Message = "导入成功,共导入" + nums.ToString() + "条数据";
                        }
                    }
                    #endregion
                }
            }
            catch (SqlException ex)
            {
                ajax.Message = ex.Message + "   请重新导入！";
                type = "error";
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
                type = "error";
            }
            finally
            {
                //删除Excel文件
                if (File.Exists(pathAddress))
                {
                    File.Delete(pathAddress);
                }
            }

            context.Response.Write(JsonConvert.SerializeObject(ajax));
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