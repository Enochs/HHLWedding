using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.DataAssmblly;
using System.Web.UI.WebControls;

namespace HHLWedding.BLLInterface.IWeddingInterface
{
    public interface ISaleSource:IBase<FD_SaleSource>
    {
        List<ListItem> GetSaleTypeDDL();
    }
}
