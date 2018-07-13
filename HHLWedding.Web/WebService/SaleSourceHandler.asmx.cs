using HHLWedding.BLLAssmbly;
using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using HHLWedding.ToolsLibrary;
using System.Linq.Expressions;
using HHLWedding.EditoerLibrary;
using HHLWedding.BLLAssmbly.FD;

namespace HHLWedding.Web.WebService
{
    /// <summary>
    /// SaleSourceHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class SaleSourceHandler : System.Web.Services.WebService
    {

        #region 数据服务

        BaseService<FD_SaleType> _SaleTypeService = new BaseService<FD_SaleType>();
        SaleSourceService _SaleSourceSerrvice = new SaleSourceService();

        #endregion


        #region SaleType

        #region 添加渠道类型
        /// <summary>
        /// 添加渠道类型
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage CreateSaleType(string typeName)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(typeName))
                {
                    int EmployeeId = LoginInfo.UserInfo.EmployeeId;
                    FD_SaleType saleType = new FD_SaleType()
                    {
                        SaleTypeName = typeName,
                        CreateEmployee = LoginInfo.UserInfo.EmployeeId,
                        CreateDate = DateTime.Now,
                        Status = 1

                    };

                    var result = _SaleTypeService.Add(saleType);
                    if (result != null)
                    {
                        ajax.Message = "添加成功";
                        ajax.IsSuccess = true;
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion

        #region 修改渠道类型
        /// <summary>
        /// 修改渠道类型
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage UpdateSaleType(string SaleTypeId, string SaleTypeName)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(SaleTypeId))
                {
                    int TypeId = SaleTypeId.ToInt32();
                    //根据Id查询
                    Expression<Func<FD_SaleType, bool>> whereLambda = c => c.SaleTypeID == TypeId;
                    FD_SaleType m_saleType = _SaleTypeService.GetModel(whereLambda);
                    m_saleType.SaleTypeName = SaleTypeName;

                    //执行修改
                    int result = _SaleTypeService.Update(m_saleType);
                    if (result > 0)
                    {
                        ajax.Message = "修改成功";
                        ajax.IsSuccess = true;
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion

        #region 删除渠道类型
        /// <summary>
        /// 删除渠道类型
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage DeleteSaleType(string SaleTypeId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(SaleTypeId))
                {
                    int TypeId = SaleTypeId.ToInt32();
                    //根据Id查询
                    object[] key = new object[1] { SaleTypeId.ToInt32() };
                    FD_SaleType m_saleType = _SaleTypeService.Find(key);
                    //执行功能 禁用状态→启用
                    if (m_saleType.Status == 0)
                    {
                        m_saleType.Status = (byte)SysStatus.Enable;
                        ajax.Message = "启用成功";
                    }
                    else
                    {
                        m_saleType.Status = (byte)SysStatus.Disable;
                        ajax.Message = "禁用成功";
                    }

                    int result = _SaleTypeService.Update(m_saleType);

                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion

        #region 获取渠道类型 下拉框
        /// <summary>
        /// 获取渠道类型 下拉框
        /// </summary>
        [WebMethod]
        public AjaxMessage GetSaleTypeDDL()
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            try
            {
                var ItemList = _SaleSourceSerrvice.GetSaleTypeDDL();
                if (ItemList.Count() > 0)
                {
                    ajax.data = ItemList;
                    ajax.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message.ToString();
            }
            return ajax;
        }
        #endregion

        #endregion


        #region SaleSource

        #region 添加渠道信息
        /// <summary>
        /// 添加渠道信息
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage CreateSaleSource(FD_SaleSource saleSource)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (saleSource != null)
                {
                    saleSource.CreateDate = DateTime.Now;
                    saleSource.CreateEmployee = LoginInfo.UserInfo.EmployeeId;
                    saleSource.Status = (byte)SysStatus.Enable;
                    saleSource.letter = PinYin.GetFirstLetter(saleSource.SourceName);

                    var result = _SaleSourceSerrvice.Add(saleSource);
                    if (result != null)
                    {
                        ajax.Message = "添加成功";
                        ajax.IsSuccess = true;
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion

        #region 修改渠道信息
        /// <summary>
        /// 修改渠道信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage UpdateSaleSource(FD_SaleSource saleSource, string SourceId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(SourceId))
                {
                    int Id = SourceId.ToString().ToInt32();
                    FD_SaleSource m_saleSource = _SaleSourceSerrvice.GetById(Id);

                    if (m_saleSource != null)
                    {
                        m_saleSource.SourceName = saleSource.SourceName;
                        m_saleSource.SaleTypeId = saleSource.SaleTypeId;
                        m_saleSource.IsRebate = saleSource.IsRebate;
                        m_saleSource.SourceAddress = saleSource.SourceAddress;
                        m_saleSource.CommoandName = saleSource.CommoandName;
                        m_saleSource.CommondPhone = saleSource.CommondPhone;
                        m_saleSource.CommondBankName = saleSource.CommondBankName;
                        m_saleSource.CommondBankCard = saleSource.CommondBankCard;
                        m_saleSource.Description = saleSource.Description;

                        //执行修改
                        int result = _SaleSourceSerrvice.Update(m_saleSource);
                        if (result > 0)
                        {
                            ajax.Message = "修改成功";
                            ajax.IsSuccess = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion

        #region 状态启用/禁用
        /// <summary>
        /// 状态启用/禁用
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public AjaxMessage SetSingleStatus(string SaleSourceId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(SaleSourceId))
                {
                    int SourceId = SaleSourceId.ToString().ToInt32();
                    Expression<Func<FD_SaleSource, bool>> pars = c => c.SourceId == SourceId;
                    FD_SaleSource m_saleSource = _SaleSourceSerrvice.GetModel(pars);
                    if (m_saleSource.Status == (byte)SysStatus.Enable)
                    {
                        m_saleSource.Status = (byte)SysStatus.Disable;
                        ajax.Message = "禁用成功";
                    }
                    else
                    {
                        m_saleSource.Status = (byte)SysStatus.Enable;
                        ajax.Message = "启用成功";
                    }


                    int result = _SaleSourceSerrvice.Update(m_saleSource);
                    if (result > 0)
                    {

                        ajax.IsSuccess = true;
                    }

                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }
            return ajax;
        }
        #endregion

        #region 加载 获取渠道信息
        /// <summary>
        /// 获取渠道信息
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetSaleSource(string SaleSourceId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(SaleSourceId))
                {
                    FD_SaleSource m_saleSource = _SaleSourceSerrvice.GetById(SaleSourceId.ToString().ToInt32());

                    if (m_saleSource != null)
                    {
                        ajax.IsSuccess = true;
                        ajax.Data = m_saleSource;
                    }
                    //else
                    //{
                    //    ajax.Data = null;
                    //}
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
                ajax.Data = null;
            }
            return ajax;
        }
        #endregion

        #region 根据渠道类型Id获取该类型的所有渠道
        /// <summary>
        /// 根据渠道类型Id获取该类型的所有渠道
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetSaleSourceByTypeID(string saleTypeId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                if (!string.IsNullOrEmpty(saleTypeId))
                {
                    List<FD_SaleSource> dataList = _SaleSourceSerrvice.GetSourceByTypeId(saleTypeId.ToString().ToInt32());

                    if (dataList != null && dataList.Count > 0)
                    {
                        ajax.IsSuccess = true;
                        ajax.Data = dataList;
                    }
                }
                else
                {
                    ajax.IsSuccess = true;
                    ajax.Data = new List<FD_SaleSource>();
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
                ajax.Data = new List<FD_SaleSource>();
            }
            return ajax;
        }
        #endregion

        #endregion
    }
}
