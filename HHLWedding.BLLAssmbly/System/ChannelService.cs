using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using EntityFramework.Extensions;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.BLLAssmbly
{
    public class ChannelService : ICRUDInterface<Sys_Channel>
    {

        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();

        /// <summary>
        /// 删除频道
        /// </summary>
        /// <param name="ObjectT">频道实体</param>
        /// <returns>返回删除的的实体KEY</returns>
        public int Delete(Sys_Channel ObjectT)
        {
            UserJurisdiction ObjUserJurisdictionBLL = new UserJurisdiction();

            ObjUserJurisdictionBLL.DeleteByChannelID(ObjectT.ChannelID);
            ObjEntity.Sys_Channel.Remove(GetByID(ObjectT.ChannelID));
            ObjEntity.SaveChanges();
            return ObjectT.ChannelID;
        }

        /// <summary>
        /// 获取部门所需
        /// </summary>
        /// <returns></returns>
        public List<Sys_Channel> GetforDepartment()
        {
            return ObjEntity.Sys_Channel.Where(C => C.Parent == 0 && C.ChannelGetType != null && C.ChannelGetType != string.Empty).OrderBy(C => C.CreateDate).ToList();
        }

        #region 根据父级获取菜单
        /// <summary>
        /// 根据父级获取
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<Sys_Channel> GetAllByParent(int parent)
        {
            return ObjEntity.Sys_Channel.Where(c => c.Parent == parent).Future().ToList();
        }
        #endregion

        /// <summary>
        /// author：wp
        /// datetime:2016-07-02
        /// desc：获取所有父级菜单
        /// </summary>
        /// <returns></returns>
        public List<Sys_Channel> GetAllParent()
        {
            return ObjEntity.Sys_Channel.Future().Where(c => c.Parent == 0).ToList();
        }

        /// <summary>
        /// 根据ID获取频道
        /// </summary>
        /// <param name="KeyID"></param>
        /// <returns></returns>
        public Sys_Channel GetByID(int? KeyID)
        {
            return ObjEntity.Sys_Channel.FirstOrDefault(C => C.ChannelID == KeyID);
        }


        public Sys_Channel GetByName(string name)
        {
            return ObjEntity.Sys_Channel.FirstOrDefault(c => c.ChannelName == name);
        }

        public bool GetByUrl(string url)
        {
            Sys_Channel channel = ObjEntity.Sys_Channel.FirstOrDefault(c => c.ChannelAddress == url);
            if (channel != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 暂时不需要分页
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="SourceCount"></param>
        /// <returns></returns>
        public List<Sys_Channel> GetByIndex(int PageSize, int PageIndex, out int SourceCount)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 根据频道类型获取
        /// </summary>
        /// <param name="ClassType"></param>
        /// <returns></returns>
        public Sys_Channel GetbyClassType(string ClassType)
        {
            var ObjModel = ObjEntity.Sys_Channel.FirstOrDefault(C => C.ChannelGetType == ClassType);
            return ObjModel;
        }


        /// <summary>
        /// 添加频道
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Insert(Sys_Channel ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.Sys_Channel.Add(ObjectT);
                if (ObjEntity.SaveChanges() > 0)
                {
                    return ObjectT.ChannelID;
                }

            }
            return 0;
        }

        /// <summary>
        /// 修改频道
        /// </summary>
        /// <param name="ObjectT"></param>
        /// <returns></returns>
        public int Update(Sys_Channel ObjectT)
        {
            if (ObjectT != null)
            {
                ObjEntity.SaveChanges();
                return ObjectT.ChannelID;
            }
            return 0;
        }

        /// <summary>
        /// 获取所有频道
        /// </summary>
        /// <returns></returns>
        public List<Sys_Channel> GetByAll()
        {
            return ObjEntity.Sys_Channel.Future().ToList();
        }


        /// <summary>
        /// 分页获取频道
        /// </summary>
        /// <param name="pars"></param>
        /// <param name="orderName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sourceCount"></param>
        /// <returns></returns>
        public List<Sys_Channel> GetAllByPager(List<PMSParameters> pars, string orderName, int page, int pageSize, out int sourceCount)
        {
            
            return PublicDataTools<Sys_Channel>.GetDataByWhereParameter(pars, orderName, pageSize, page, out sourceCount);
        }
    }
}
