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
    /// 酒店标签
    /// </summary>
    public partial class FD_HotelLabel
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

    /// <summary>
    /// 酒店
    /// </summary>
    public partial class FD_Hotel
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


        [NotMapped]
        [Display(Name = "酒店类型")]
        public HotelTypeName TypeName
        {
            get
            {
                return (HotelTypeName)(this.HotelType == null ? 0 : this.HotelType);
            }
            set
            {
                this.HotelType = Convert.ToByte(value);
            }
        }

    }

    /// <summary>
    /// 消息
    /// </summary>
    public partial class sm_Message
    {
        [Display(Name = "消息类型")]
        public SendTypeName TypeName
        {
            get
            {
                return (SendTypeName)(this.SendType == null ? 0 : this.SendType);
            }
            set
            {
                this.SendType = Convert.ToByte(value);
            }
        }
    }


    /// <summary>
    /// 渠道类型状态
    /// </summary>
    public partial class FD_SaleType
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

    /// <summary>
    /// 渠道状态
    /// </summary>
    public partial class FD_SaleSource
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

    /// <summary>
    /// 消息类
    /// </summary>
    public class FD_Message
    {
        public int MessageId { get; set; }
        public string MessageTitle { get; set; }
        public Nullable<int> FromEmployee { get; set; }
        public string FromEmpName { get; set; }
        public string FromLoginName { get; set; }
        public string ToEmployee { get; set; }

        /// <summary>
        /// 格式 姓名+(用户名)
        /// </summary>
        public string ToFullName { get; set; }
        public string ToEmpName { get; set; }
        public string MessageContent { get; set; }
        public Nullable<System.DateTime> SendDateTime { get; set; }
        public Nullable<int> IsRead { get; set; }
    }
}
