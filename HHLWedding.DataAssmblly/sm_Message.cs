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
    
    public partial class sm_Message
    {
        public int MessageId { get; set; }
        public string MessageTitle { get; set; }
        public string MessageContent { get; set; }
        public Nullable<int> FromEmployee { get; set; }
        public string ToEmployee { get; set; }
        public Nullable<System.DateTime> SendDateTime { get; set; }
        public Nullable<int> SendType { get; set; }
        public Nullable<int> PreMsgId { get; set; }
        public Nullable<int> NextmsgId { get; set; }
        public Nullable<int> IsRead { get; set; }
        public Nullable<int> IsDraft { get; set; }
        public Nullable<int> IsGarbage { get; set; }
    }
}