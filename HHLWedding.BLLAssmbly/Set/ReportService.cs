using HHLWedding.DataAssmblly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHLWedding.BLLAssmbly.Set
{
    public class ReportService : BaseService<SS_Report>
    {
        HHL_WeddingEntities ObjEntity = new HHL_WeddingEntities();

        #region 根据客户Id查找Report
        /// <summary>
        /// @author:wp
        /// @datetime:2016-12-09
        /// @desc:根据客户Id查找Report
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public SS_Report GetByCustomerId(Guid CustomerId)
        {
            if (CustomerId != null || CustomerId != Guid.Empty)
            {
                SS_Report report = this.ObjEntity.SS_Report.FirstOrDefault(c => c.CustomerId == CustomerId);
                return report;
            }
            return null;
        }
        #endregion
    }
}
