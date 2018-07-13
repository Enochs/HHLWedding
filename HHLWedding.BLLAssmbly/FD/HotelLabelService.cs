using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using EntityFramework.Extensions;
using System.Linq.Expressions;

namespace HHLWedding.BLLAssmbly.FD
{
    public class HotelLabelService : ICRUDInterface<FD_HotelLabel>
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

        public FD_HotelLabel GetByID(int? KeyID)
        {
            return ObjEntity.FD_HotelLabel.FirstOrDefault(c => c.LabelID == KeyID);
        }
        #endregion

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc: 获取所有酒店标签
        /// </summary>
        /// <returns></returns>
        public List<FD_HotelLabel> GetByAll()
        {
            return ObjEntity.FD_HotelLabel.ToList();
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc:添加酒店标签
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Insert(FD_HotelLabel ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.FD_HotelLabel.Add(ObjectT);
                if (ObjEntity.SaveChanges() > 0)
                {
                    return ObjectT.LabelID;
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
        public int Update(FD_HotelLabel ObjectT)
        {
            if (ObjectT != null)
            {
                return ObjEntity.SaveChanges();
            }
            return 0;
        }


        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc:删除酒店标签
        /// </summary>
        /// <returns></returns>
        public int Delete(FD_HotelLabel ObjectT)
        {
            if (ObjectT != null)
            {
                return ObjEntity.FD_HotelLabel.Where(c => c.LabelID == ObjectT.LabelID).Delete();
                //FD_HotelLabel m_label = GetByID(ObjectT.LabelID);
                //ObjEntity.FD_HotelLabel.Remove(m_label);
                //return ObjEntity.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc:分页获取
        /// </summary>
        /// <returns></returns>
        public List<FD_HotelLabel> GetAllByPager(List<ToolsLibrary.PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount)
        {
            return PublicDataTools<FD_HotelLabel>.GetDataByWhereParameter(pars, orderName, pageSize, page, out sourceCount);
        }

        /// <summary>
        /// @author：wp
        /// @datetime:2016-08-15
        /// @desc:获取所有启用/禁用的Label
        /// </summary>
        /// <returns></returns>
        public List<FD_HotelLabel> GetAllByEnable(int status)
        {
            return ObjEntity.FD_HotelLabel.Future().Where(C => C.Status == status).ToList();
        }


        public List<FD_HotelLabel> GetAllByPage(int pageIndex, int pageSize, List<Expression<Func<FD_HotelLabel, bool>>> parmList, string sort, out int count)
        {
            throw new NotImplementedException();
        }
    }
}
