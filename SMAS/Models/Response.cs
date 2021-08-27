using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMAS.Models
{
    public class Response<T> where T : class
    {
        public string code { get; set; }
        public string message { get; set; }
        public T Result { get; set; }
    }

    public class LoginResult
    {
        public string PO_PARENT_GROUP { get; set; }
       
        public string PO_DEALER_MAP_CD { get; set; }
       
        public string PO_LOC_CD { get; set; }
       
        public string PO_COMP_FA { get; set; }
       
        public string PO_DEALER_NAME { get; set; }
       
        public string PO_DEALER_CODE { get; set; }
       
        public string PO_REGION_CD { get; set; }
       
        public string PO_REGION { get; set; }
       
        public string PO_ZONE_CD { get; set; }
       
        public string PO_ZONE { get; set; }
       
        public string PO_LOC_DESC { get; set; }
       
        public string PO_EMP_CD { get; set; }
       
        public string PO_DESG_CD { get; set; }
        
    }
}