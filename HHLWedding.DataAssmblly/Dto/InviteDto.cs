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
    public class InviteDto
    {
        public Guid CustomerId { get; set; }
        public int InviteId { get; set; }
        public string Bride { get; set; }
        public string Groom { get; set; }
        public string Operator { get; set; }
        public string CustomerName { get; set; }
        public Nullable<System.DateTime> PartyDate { get; set; }
        public string ContactPhone { get; set; }
        public string Hotel { get; set; }
        public Nullable<int> SaleType { get; set; }
        public Nullable<int> SaleSource { get; set; }
        public string SaleTypeName { get; set; }
        public string SaleSourceName { get; set; }
        public string ReCommandName { get; set; }
        /// <summary>
        /// 客户状态进程(CustomerState)
        /// </summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public Nullable<byte> Status { get; set; }
        /// <summary>
        /// 是否是会员
        /// </summary>
        public Nullable<int> IsVip { get; set; }

        /// <summary>
        /// 邀约人
        /// </summary>
        public Nullable<int> InviteEmployee { get; set; }

        public string InviteEmpName { get; set; }
        /// <summary>
        /// 邀约沟通次数
        /// </summary>
        public Nullable<int> FollowCount { get; set; }
        /// <summary>
        /// 创建时间(邀约)
        /// </summary>
        public Nullable<System.DateTime> CreateDate { get; set; }
        /// <summary>
        /// 下次沟通时间
        /// </summary>
        public Nullable<System.DateTime> NextFollowDate { get; set; }
        /// <summary>
        /// 最后一次沟通时间
        /// </summary>
        public Nullable<System.DateTime> LastFollowDate { get; set; }

        public List<FL_InviteDetails> DetaistList { get; set; }


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
