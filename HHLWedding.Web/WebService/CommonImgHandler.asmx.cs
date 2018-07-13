using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using HHLWedding.ToolsLibrary;
using HHLWedding.BLLAssmbly;
using HHLWedding.DataAssmblly;
using HHLWedding.BLLAssmbly.Sys;
using HHLWedding.Web.Handler;
using HHLWedding.BLLAssmbly.FD;

namespace HHLWedding.Web.WebService
{
    /// <summary>
    /// CommonImgHandler 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class CommonImgHandler : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        CommonImageService _imgService = new CommonImageService();
        HotelService _hotelService = new HotelService();

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage DeleteImgById(int imgId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";

            CommonImages image = _imgService.GetByID(imgId);
            if (image != null)
            {
                int result = _imgService.Delete(image);
                if (result > 0)
                {
                    ajax.IsSuccess = true;
                    ajax.Message = "删除成功";
                }
            }

            return ajax;
        }


        /// <summary>
        /// 修改封面图片
        /// </summary>
        /// <param name="imgId"></param>
        /// <returns></returns>
        [WebMethod]
        public AjaxMessage SetThemeTitle(int imgId, int hotelId)
        {
            AjaxMessage ajax = new AjaxMessage();
            ajax.IsSuccess = false;
            ajax.Message = "";

            //前封面图片
            CommonImages image = _imgService.GetTitleImg(hotelId);
            if (image != null)
            {
                image.State = 0;
                _imgService.Update(image);
            }

            //封面图片
            CommonImages uimage = _imgService.GetByID(imgId);
            uimage.State = 1;
            int result = _imgService.Update(uimage);
            if (result > 0)
            {
                //修改酒店封面图片
                FD_Hotel hotel = _hotelService.GetByID(hotelId);
                hotel.HotelThemeImage = uimage.ImgUrl;
                _hotelService.Update(hotel);
                ajax.IsSuccess = true;
                ajax.Message = "设置成功";
            }

            return ajax;
        }
    }
}
