using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMAS.Models
{
    public class tblFeedback_Form
    {
        [Key]
        public int Id { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string Category { get; set; }
        public string Languages { get; set; }
        public string Models { get; set; }
        public string FileTypes { get; set; }
        public string Subjects { get; set; }
        public string Feedback { get; set; }
        public bool? Status { get; set; }
        public string Createdby { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Updatedby { get; set; }
        public DateTime? UpdatedOn { get; set; }
      
    }


    public class tblFeedback_Form_Reuturn 
    {
        public string code { get; set; }
        public string message { get; set; }

        public string result { get; set; }

    }

    public class GetDetailsModel
    {
        public string Dealercode { get; set; }
    }
}