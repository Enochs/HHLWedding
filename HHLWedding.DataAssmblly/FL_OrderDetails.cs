//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HHLWedding.DataAssmblly
{
    using System;
    using System.Collections.Generic;
    
    public partial class FL_OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public Nullable<System.Guid> OrderId { get; set; }
        public Nullable<System.Guid> CustomerId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<int> OrderState { get; set; }
        public string OrderContent { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> NextFollowDate { get; set; }
    
        public virtual FL_Order FL_Order { get; set; }
    }
}
