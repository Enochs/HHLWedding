using HHLWedding.DataAssmblly.CommonModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHLWedding.DataAssmblly.Dto
{
    public class CustomerDto
    {
        public Guid CustomerId { get; set; }
        public string Bride { get; set; }
        public string Groom { get; set; }
        public string Operator { get; set; }
        public string BridePhone { get; set; }
        public string GroomPhone { get; set; }
        public string OperatorPhone { get; set; }
        public string ContactMan { get; set; }
        public string ContactPhone { get; set; }
        public Nullable<System.DateTime> PartyDate { get; set; }
        public string Hotel { get; set; }
        public Nullable<int> SaleType { get; set; }
        public Nullable<int> Channel { get; set; }
        public Nullable<int> State { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public Nullable<int> CreateEmployee { get; set; }
        /// <summary>
        /// 邀约人
        /// </summary>
        public Nullable<int> InviteEmployee { get; set; }
        /// <summary>
        /// 统筹师
        /// </summary>
        public Nullable<int> OrderEmployee { get; set; }
        /// <summary>
        /// 策划师
        /// </summary>
        public Nullable<int> QuotedEmployee { get; set; }


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

    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Bride { get; set; }
        public string Groom { get; set; }
        public string Operator { get; set; }
        public string BridePhone { get; set; }
        public string GroomPhone { get; set; }
        public string OperatorPhone { get; set; }
        public string ContactMan { get; set; }
        public string ContactPhone { get; set; }
        public Nullable<System.DateTime> PartyDate { get; set; }
        public string Hotel { get; set; }
        public string SaleTypeName { get; set; }
        public string SaleSourceName { get; set; }
        public string ReCommandName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }

        public Nullable<byte> Status { get; set; }

        public Nullable<int> IsVip { get; set; }
        public int? State { get; set; }
        public int isFinish { get; set; }
        public int EvalState { get; set; }


        [NotMapped]
        [Display(Name = "进度状态")]
        public CustomerState StateValue
        {
            get
            {
                return (CustomerState)(this.State == null ? 0 : this.State);
            }
            set
            {
                this.State = Convert.ToByte(value);
            }
        }

        public string StateName
        {
            get
            {
                return StateValue.GetDisplayName();
            }
        }
    }

}
