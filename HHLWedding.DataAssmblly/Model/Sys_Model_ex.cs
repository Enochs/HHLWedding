using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHLWedding.DataAssmblly.CommonModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HHLWedding.DataAssmblly
{
    public partial class Sys_Channel
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

    public partial class Sys_Department
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

    public partial class Sys_EmployeeJob
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

    public partial class Sys_EmployeeType
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

    public partial class Sys_Employee
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

    public partial class Sys_EmployeePower
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

    public partial class HHL_Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string LoginName { get; set; }
        public int? TypeId { get; set; }
        public int? JobId { get; set; }
        public string TypeName { get; set; }
        public string JobName { get; set; }
    }
}
