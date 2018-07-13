using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace HHLWedding.BLLAssmbly.Sys
{
    public class CommonImageService : ICRUDInterface<CommonImages>
    {
        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();

        /// <summary>
        /// 根据Id查找图片
        /// </summary>
        /// <param name="KeyID"></param>
        /// <returns></returns>
        public CommonImages GetByID(int? KeyID)
        {
            return ObjEntity.CommonImages.FirstOrDefault(c => c.ImgId == KeyID);
        }

        public List<CommonImages> GetByAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="ObjectT">添加</param>
        /// <returns></returns>
        public int Insert(CommonImages ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.CommonImages.Add(ObjectT);
                return ObjEntity.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// 修改图片
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Update(CommonImages ObjectT)
        {
            if (ObjectT != null)
            {
                return ObjEntity.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Delete(CommonImages ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.CommonImages.Remove(GetByID(ObjectT.ImgId));
                return ObjEntity.SaveChanges();
            }
            return 0;
        }

        /// <summary>
        /// 分页获取 显示图片
        /// </summary>
        public List<CommonImages> GetAllByPager(List<ToolsLibrary.PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount)
        {
            return PublicDataTools<CommonImages>.GetDataByWhereParameter(pars, orderName, pageSize, page, out sourceCount);
        }


        /// <summary>
        /// 获取酒店的封面图片
        /// </summary>
        /// <param name="CommonId"></param>
        /// <returns></returns>
        public CommonImages GetTitleImg(int CommonId)
        {
            return ObjEntity.CommonImages.FirstOrDefault(c => c.CommonId == CommonId && c.State == 1);
        }

        public List<CommonImages> GetAllByPage(int pageIndex, int pageSize, List<Expression<Func<CommonImages, bool>>> parmList, string sort, out int count)
        {
            throw new NotImplementedException();
        }
    }
}
