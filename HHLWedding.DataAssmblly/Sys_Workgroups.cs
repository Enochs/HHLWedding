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
    
    public partial class Sys_Workgroups
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sys_Workgroups()
        {
            this.Sys_Employee = new HashSet<Sys_Employee>();
        }
    
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sys_Employee> Sys_Employee { get; set; }
    }
}
