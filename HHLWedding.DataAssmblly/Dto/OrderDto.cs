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
    public class OrderDto
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCoder { get; set; }
        /// <summary>
        /// 客户Id
        /// </summary>
        public Guid CustomerId { get; set; }
        /// <summary>
        /// 新娘姓名
        /// </summary>
        public string Bride { get; set; }

        /// <summary>
        /// 新郎姓名 
        /// </summary>
        public string Groom { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 新娘姓名/新郎姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 婚期
        /// </summary>
        public Nullable<System.DateTime> PartyDate { get; set; }
        /// <summary>
        /// 新娘电话
        /// </summary>
        public string BridePhone { get; set; }
        /// <summary>
        /// 新郎电话
        /// </summary>
        public string GroomPhone { get; set; }
        /// <summary>
        /// 经办人电话
        /// </summary>
        public string OperatorPhone { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 到店日期
        /// </summary>
        public Nullable<System.DateTime> ComeDate { get; set; }

        /// <summary>
        /// 客户状态
        /// </summary>
        public Nullable<int> OrderState { get; set; }

        /// <summary>
        /// 跟单人
        /// </summary>
        public Nullable<int> EmployeeId { get; set; }

        public string EmployeeName { get; set; }


        /// <summary>
        /// 订单的创建人
        /// </summary>
        public Nullable<int> CreateEmployee { get; set; }

        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelId { get; set; }

        public string HotelName { get; set; }

        /// <summary>
        /// 渠道类型名称
        /// </summary>
        public Nullable<int> SaleTypeId { get; set; }

        public string SaleTypeName { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        public Nullable<int> SaleSourceId { get; set; }


        public string SaleSourceName { get; set; }


        /// <summary>
        /// 跟单次数
        /// </summary>
        public Nullable<int> FollowCount { get; set; }

        /// <summary>
        /// 最后一次沟通时间
        /// </summary>
        public Nullable<System.DateTime> LastFollowDate { get; set; }

        /// <summary>
        /// 下次沟通时间
        /// </summary>
        public Nullable<System.DateTime> NextFollowDate { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateDate { get; set; }

        /// <summary>
        /// 进度状态名称
        /// </summary>
        [NotMapped]
        [Display(Name = "进度状态")]
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
            get
            {
                return StateValue.GetDisplayName();
            }
        }

    }
}
