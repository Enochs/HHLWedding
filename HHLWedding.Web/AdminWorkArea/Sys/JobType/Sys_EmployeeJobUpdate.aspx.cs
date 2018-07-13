using HHLWedding.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HHLWedding.ToolsLibrary;
using HHLWedding.BLLAssmbly;

namespace HHLWedding.Web.AdminWorkArea.Sys.JobType
{
    public partial class Sys_EmployeeJobUpdate : SystemPage
    {
        EmployeeJob _jobService = new EmployeeJob();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int jobId = Request["jobId"].ToString().ToInt32();
                var empJob = _jobService.GetByID(jobId);
                
            }
        }
    }
}