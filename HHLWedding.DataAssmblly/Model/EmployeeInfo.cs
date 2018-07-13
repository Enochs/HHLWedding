using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHLWedding.DataAssmblly.Model
{
    public class EmployeeInfo
    {
        public int EmployeeID { get; set; }
        public string LoginName { get; set; }
        public string EmployeeName { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string JobName { get; set; }
        public string TypeName { get; set; }
        public string Phone { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime BornDate { get; set; }
    }
}
