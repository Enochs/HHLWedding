using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.BLLInterface;
using HHLWedding.DataAssmblly;
using EntityFramework.Extensions;
using HHLWedding.ToolsLibrary;
using HHLWedding.DataAssmblly.CommonModel;
using System.Linq.Expressions;
using HHLWedding.ToolsLibrary;

namespace HHLWedding.BLLAssmbly
{
    public class ChannelService : ICRUDInterface<Sys_Channel>
    {

        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();

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
        /// 模板页需要(导航)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Sys_Channel GetChannelByUrl(string url)
        {
            Sys_Channel channel = ObjEntity.Sys_Channel.FirstOrDefault(c => c.ChannelAddress == url);
            return channel;
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

        /// <summary>
        /// 根据状态获取channel菜单
        /// </summary>
        /// <param name="Status">状态</param>
        /// <param name="parent">父级</param>
        /// <returns></returns>
        public List<Sys_Channel> GetAllByStatus(int Status, int Parent = 0)
        {
            return ObjEntity.Sys_Channel.Where(c => c.Status == Status && c.Parent == Parent).ToList();
        }


        public int Delete(Sys_Channel ObjectT)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 批量修改状态
        /// </summary>
        /// <param name="type">1.禁用→启用  2.启用→禁用</param>
        /// <returns></returns>
        public int UpdatePartStatus(string channelId, int type)
        {
            int result = 0;
            string[] ids = channelId.Split(',');

            var query = (from c in ObjEntity.Sys_Channel
                         where ids.Contains(c.ChannelID.ToString())
                         select c);

            if (type == 1)              //修改为启用
            {
                result = query.Update(c => new Sys_Channel { Status = (byte)SysStatus.Enable });
            }
            else                        //修改为禁用
            {
                result = query.Update(c => new Sys_Channel { Status = (byte)SysStatus.Disable });
            }
            return result;
        }


        /// <summary>
        /// 获取最大排序值
        /// </summary>
        /// <returns></returns>
        public int GetMaxSort(int parent)
        {
            string sql = "SELECT Max(SortInt) FROM Sys_Channel where Parent=" + parent;
            DBHelper db = new DBHelper();
            int Maxsort = db.ExcuteScalar(sql).ToString().ToInt32();

            return Maxsort + 1;
        }


        /// <summary>
        /// @author:wp
        /// @datetime:2017-03-07
        /// @desc:获取模块类名
        /// </summary>
        /// <param name="parent">渠道ID</param>
        /// <returns></returns>
        public string GetTheme(int parent)
        {
            string theme = ObjEntity.Sys_Channel.FirstOrDefault(c => c.ChannelID == parent && c.Parent == 0).ChannelAddress;
            return theme;
        }



        public List<Sys_Channel> GetAllByPage(int pageIndex, int pageSize, List<Expression<Func<Sys_Channel, bool>>> parmList, string sort, out int count)
        {
            throw new NotImplementedException();
        }
    }
}
