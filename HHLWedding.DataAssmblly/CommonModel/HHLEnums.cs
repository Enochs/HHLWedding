using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHLWedding.DataAssmblly.CommonModel
{

    /// <summary>
    /// 用户状态
    /// </summary>
    public enum SysStatus
    {
        /// <summary>
        /// 禁用
        /// </summary>
        [Display(Name = "禁用")]
        Disable = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Display(Name = "启用")]
        Enable = 1,
    }

    /// <summary>
    /// 客户状态
    /// </summary>
    public enum CustomerState
    {
        [Display(Name = "未邀约")]
        NoInvite = 1,
        [Display(Name = "邀约中")]
        DoVinte = 2,
        [Display(Name = "邀约成功")]
        SuccessInvite = 3,
        [Display(Name = "流失(邀约)")]
        LoseInvite = 4,
        [Display(Name = "确认到店")]
        ComeOrder = 5,
        [Display(Name = "二选一")]
        TwoToOne = 6,
        [Display(Name = "多选一")]
        MoreToOne = 7,
        [Display(Name = "成功预定")]
        SuccessOrder = 8,
        [Display(Name = "流失(跟单)")]
        LoseOrder = 9,
        [Display(Name = "策划方案")]
        QuoteodPrice = 10,
        [Display(Name = "订单派工")]
        Dispatching = 11,
        [Display(Name = "执行中")]
        Implement = 12,
    }

    /// <summary>
    /// 酒店类型名称
    /// </summary>
    public enum HotelTypeName
    {
        [Display(Name = "特色酒店")]
        HType1 = 1,
        [Display(Name = "自助餐厅")]
        HType2 = 2,
        [Display(Name = "三星级酒店")]
        HType3 = 3,
        [Display(Name = "四星级酒店")]
        HType4 = 4,
        [Display(Name = "五星级酒店")]
        HType5 = 5,
    }


    /// <summary>
    /// 消息类型
    /// </summary>
    public enum SendTypeName
    {
        /// <summary>
        /// 禁用
        /// </summary>
        [Display(Name = "收件")]
        GetMsg = 1,

        /// <summary>
        /// 启用
        /// </summary>
        [Display(Name = "发件")]
        SendMsg = 2,
    }

}
