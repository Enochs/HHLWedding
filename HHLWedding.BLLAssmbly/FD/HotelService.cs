using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using System.Linq.Expressions;

namespace HHLWedding.BLLAssmbly.FD
{
    public class HotelService : ICRUDInterface<FD_Hotel>
    {
        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();

        #region 根据ID获取标签
        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc: 根据ID获取标签
        /// </summary>
        /// <param name="KeyID"></param>
        /// <returns></returns>

        public FD_Hotel GetByID(int? KeyID)
        {
            return ObjEntity.FD_Hotel.FirstOrDefault(c => c.HotelID == KeyID);
        }
        #endregion

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc: 获取所有酒店标签
        /// </summary>
        /// <returns></returns>
        public List<FD_Hotel> GetByAll()
        {
            return ObjEntity.FD_Hotel.ToList();
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc:添加酒店标签
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Insert(FD_Hotel ObjectT)
        {
            if (ObjectT != null)
            {
                var model = ObjEntity.FD_Hotel.Add(ObjectT);
                if (model != null)
                {
                    return ObjEntity.SaveChanges();
                }
            }
            return 0;
        }


        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc:修改酒店标签
        /// </summary>
        /// <returns></returns>
        public int Update(FD_Hotel ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.SaveChanges();
                return ObjectT.HotelID;
            }
            return 0;
        }


        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc:删除酒店标签
        /// </summary>
        /// <returns></returns>
        public int Delete(FD_Hotel ObjectT)
        {
            if (ObjectT != null)
            {
                return ObjEntity.FD_Hotel.Where(c => c.HotelID == ObjectT.HotelID).Delete();
            }
            return 0;
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc:分页获取
        /// </summary>
        /// <returns></returns>
        public List<FD_Hotel> GetAllByPager(List<ToolsLibrary.PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount)
        {
            return PublicDataTools<FD_Hotel>.GetDataByWhereParameter(pars, orderName, pageSize, page, out sourceCount);
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc:获取所有启用/禁用的Hotel
        /// </summary>
        /// <returns></returns>
        public List<FD_Hotel> GetAllEnable(int status)
        {
            return ObjEntity.FD_Hotel.Future().Where(C => C.Status == status).ToList();
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-31
        /// @desc: 判断酒店是否存在
        /// </summary>
        /// <param name="hotelName">酒店名称</param>
        /// <returns></returns>
        public bool IsExists(string hotelName)
        {
            FD_Hotel hotel = ObjEntity.FD_Hotel.FirstOrDefault(c => c.HotelName == hotelName);
            if (hotel != null)
            {      //酒店已经存在
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-31
        /// @desc: 修改时 判断酒店是否存在
        /// </summary> 
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public bool UpdateisExists(int hotelId, string hotelName)
        {
            List<FD_Hotel> hotelList = ObjEntity.FD_Hotel.Where(c => c.HotelID != hotelId && c.HotelName == hotelName).ToList();
            if (hotelList != null && hotelList.Count > 0)
            {      //酒店已经存在
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<FD_Hotel> GetAllByPage(int pageIndex, int pageSize, List<Expression<Func<FD_Hotel, bool>>> parmList, string sort, out int count)
        {
            throw new NotImplementedException();
        }
    }
}
