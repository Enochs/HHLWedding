using HHLWedding.BLLInterface.IWeddingInterface;
using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace HHLWedding.BLLAssmbly.FD
{
    public class SaleSourceService : BaseService<FD_SaleSource>, ISaleSource
    {
        public readonly BaseService<FD_SaleType> _saleTypeService = new BaseService<FD_SaleType>();

        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();

        #region 获取渠道类型
        /// <summary>
        /// 获取渠道类型下拉框
        /// </summary>
        /// <returns></returns>
        public List<ListItem> GetSaleTypeDDL()
        {
            List<Expression<Func<FD_SaleType, bool>>> parmList = new List<Expression<Func<FD_SaleType, bool>>>();

            //启用状态
            parmList.Add(c => c.Status == 1);

            List<FD_SaleType> list = _saleTypeService.GetListBy(parmList);
            List<ListItem> option = new List<ListItem>();
            foreach (var item in list)
            {
                option.Add(new ListItem { Text = item.SaleTypeName, Value = item.SaleTypeID.ToString() });
            }

            //option.Insert(0, new ListItem { Text = "--请选择--", Value = "" });
            return option;
        }
        #endregion


        #region 获取渠道名称
        /// <summary>
        /// 获取渠道名称下拉框
        /// </summary>
        /// <returns></returns>
        public List<ListItem> GetSaleSouceDDL()
        {
            List<Expression<Func<FD_SaleSource, bool>>> parmList = new List<Expression<Func<FD_SaleSource, bool>>>();

            //启用状态
            parmList.Add(c => c.Status == 1);

            List<FD_SaleSource> list = this.GetListBy(parmList);
            List<ListItem> option = new List<ListItem>();
            foreach (var item in list)
            {
                option.Add(new ListItem { Text = item.SourceName, Value = item.SourceId .ToString() });
            }

            option.Insert(0, new ListItem { Text = "请选择", Value = "-1" });
            return option;
        }
        #endregion

        #region 根据渠道类型 获取渠道
        /// <summary>
        /// 根据渠道类型获取渠道
        /// </summary>
        public List<FD_SaleSource> GetSourceByTypeId(int saleTypeId)
        {
            return ObjEntity.FD_SaleSource.Where(c => c.SaleTypeId == saleTypeId).ToList();
        }
        #endregion
    }
}
