using HHLWedding.DataAssmblly.CommonModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHLWedding.DataAssmblly
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public partial class FL_Customer
    {
        [NotMapped]
        [Display(Name = "状态")]
        public SysStatus StatusValue
        {
            get
            {
                return (SysStatus)(this.Status == null ? 0 : this.Status);
            }
            set
            {
                this.Status = Convert.ToByte(value);
            }
        }
    }

    public partial class FL_InviteDetails
    {
        [NotMapped]
        [Display(Name = "状态")]
        public CustomerState StateValue
        {
            get
            {
                return (CustomerState)(this.InviteState == null ? 0 : this.InviteState);
            }
            set
            {
                this.InviteState = Convert.ToByte(value);
            }
        }
    }

    public partial class FL_OrderDetails
    {
        [NotMapped]
        [Display(Name = "状态")]
        public CustomerState StateValue
        {
            get
            {
                return (CustomerState)(this.OrderState == null ? 0 : this.OrderState);
            }
            set
            {
                this.OrderState = Convert.ToByte(value);
            }
        }

        public string StateName
        {
            get { return StateValue.GetDisplayName(); }
        }
    }



}
