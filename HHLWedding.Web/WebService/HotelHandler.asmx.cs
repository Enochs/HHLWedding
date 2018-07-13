using HHLWedding.BLLAssmbly.FD;
using HHLWedding.DataAssmblly;
using HHLWedding.Web.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly.CommonModel;
using HHLWedding.EditoerLibrary;

namespace HHLWedding.Web.WebService
{
    /// <summary>
    /// HotelHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class HotelHandler : System.Web.Services.WebService
    {
        #region 服务

        /// <summary>
        /// 酒店
        /// </summary>
        HotelService _hotelService = new HotelService();

        /// <summary>
        /// 酒店标签
        /// </summary>
        HotelLabelService _labelService = new HotelLabelService();

        #endregion

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        #region HotelLabel

        #region 添加标签
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc:添加标签
        /// </summary>
        /// <param name="name">标签名称</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage InsertLabel(string name, int empId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {

                if (name != "")
                {
                    FD_HotelLabel m_label = new FD_HotelLabel();
                    m_label.LabelName = name;
                    m_label.CreateEmployee = empId.ToString().ToInt32();
                    m_label.CreateDate = DateTime.Now;
                    m_label.Status = (byte)SysStatus.Enable;
                    int result = _labelService.Insert(m_label);
                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                        ajax.Message = "添加成功";
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

        #region 修改标签信息
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc: 修改标签信息
        /// </summary>
        [WebMethod]
        public AjaxMessage UpdateLabel(int labelId, string name)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                FD_HotelLabel m_label = _labelService.GetByID(labelId);
                if (m_label != null)
                {
                    m_label.LabelName = name;
                    int result = _labelService.Update(m_label);
                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                        ajax.Message = "修改成功";
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

        #region 修改状态
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc: 修改状态
        /// </summary>
        /// <param name="labelId"></param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage SetHotelSingleStatus(int labelId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                FD_HotelLabel m_label = _labelService.GetByID(labelId);
                if (m_label != null)
                {
                    if (m_label.Status == (byte)SysStatus.Enable)
                    {
                        m_label.Status = (byte)SysStatus.Disable;
                        ajax.Message = "禁用成功";
                    }
                    else
                    {
                        m_label.Status = (byte)SysStatus.Enable;
                        ajax.Message = "启用成功";
                    }
                    int result = _labelService.Update(m_label);
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

        #region 根据LabelId获取标签信息
        /// <summary>
        /// @author:wp
        /// @datetime:2016-08-16
        /// @desc: 根据LabelId获取标签信息
        /// </summary>
        /// <param name="LabelId">标签ID</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetByLabelId(int labelId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            try
            {
                FD_HotelLabel m_label = _labelService.GetByID(labelId);
                if (m_label != null)
                {
                    ajax.Value = m_label.LabelName;
                    ajax.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                ajax.Message = e.Message;
            }

            return ajax;
        }
        #endregion

        #endregion

        #region Hotel

        #region 添加酒店
        /// <summary>
        /// 添加酒店
        /// </summary>
        /// <param name="hotel">酒店实体类</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage InsertHotel(FD_Hotel hotel)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            if (hotel != null)
            {
                bool isExists = _hotelService.IsExists(hotel.HotelName);
                if (isExists == false)          //酒店不存在  就新增
                {
                    hotel.CreateDate = DateTime.Now;
                    hotel.Letter = PinYin.GetFirstLetter(hotel.HotelName.ToString());
                    hotel.Status = (byte)SysStatus.Enable;

                    int result = _hotelService.Insert(hotel);
                    if (result > 0)
                    {
                        ajax.IsSuccess = true;
                        ajax.Message = "添加成功";
                    }
                }
                else
                {
                    ajax.Message = "酒店名称已经存在";
                }
            }

            return ajax;
        }
        #endregion

        #region 修改酒店信息
        /// <summary>
        /// 添加酒店
        /// </summary>
        /// <param name="hotel">酒店实体类</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage UpdateHotel(FD_Hotel hotel)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";

            bool isExists = _hotelService.UpdateisExists(hotel.HotelID, hotel.HotelName);
            if (isExists == false)
            {
                var m_hotel = _hotelService.GetByID(hotel.HotelID);
                m_hotel.HotelName = hotel.HotelName;
                m_hotel.Area = hotel.Area;
                m_hotel.Address = hotel.Address;
                m_hotel.HotelType = hotel.HotelType;
                m_hotel.Phone = hotel.Phone;
                m_hotel.DeskCount = hotel.DeskCount;
                m_hotel.Start = hotel.Start;
                m_hotel.End = hotel.End;
                m_hotel.Sort = hotel.Sort;
                m_hotel.Label = hotel.Label;
                m_hotel.LabelContent = hotel.LabelContent;
                m_hotel.Description = hotel.Description;

                m_hotel.Letter = PinYin.GetFirstLetter(hotel.HotelName.ToString());

                int result = _hotelService.Update(m_hotel);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "修改成功";
                }
            }
            else
            {
                ajax.Message = "酒店名称已经存在";
            }


            return ajax;
        }
        #endregion

        #region 根据酒店ID加载酒店信息
        /// <summary>
        /// 添加酒店
        /// </summary>
        /// <param name="hotelId">酒店Id</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage GetHotelById(string hotelId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";
            if (!string.IsNullOrEmpty(hotelId))
            {
                var m_hotel = _hotelService.GetByID(hotelId.ToInt32());
                if (m_hotel != null)
                {
                    ajax.Data = m_hotel;
                    ajax.IsSuccess = true;
                }
            }
            return ajax;
        }
        #endregion

        #region 修改酒店信息
        /// <summary>
        /// 添加酒店
        /// </summary>
        /// <param name="hotel">酒店实体类</param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage SetSingleStatus(string hotelId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";

            try
            {
                if (!string.IsNullOrEmpty(hotelId))
                {
                    FD_Hotel m_hotel = _hotelService.GetByID(hotelId.ToInt32());
                    if (m_hotel != null)
                    {
                        if (m_hotel.Status == (byte)SysStatus.Enable)
                        {
                            m_hotel.Status = (byte)SysStatus.Disable;
                            ajax.Message = "禁用成功";
                        }
                        else
                        {
                            m_hotel.Status = (byte)SysStatus.Enable;
                            ajax.Message = "启用成功";
                        }

                        int result = _hotelService.Update(m_hotel);
                        if (result > 0)
                        {
                            ajax.IsSuccess = true;
                        }
                        else
                        {
                            ajax.Message = "系统异常";
                        }
                    }
                    else
                    {
                        ajax.Message = "没有找到酒店";
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

        #endregion
    }
}
