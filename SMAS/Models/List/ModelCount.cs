using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SMAS.Models.List
{
    public class ModelMainResponce
    {

        public string code { get; set; }
        public string message { get; set; }
        public IEnumerable<Modaldetails> result { get; set; }
    }

    public class Modaldetails
    {
        //internal static List<Modaldetails> result;

        public int PDF { get; set; }
        public int Video { get; set; }
        public string SMAS_MODEL_CD { get; set; }
        public string SMAS_MODEL_DESC { get; set; }
        public string SERVICE_CODE { get; set; }
        public string LID { get; set; }
    }
    public class SubscribeModel
    {
        public int ID { get; set; }
        public int SID { get; set; }
        public string TITLE { get; set; }
        public Boolean ISSUBSCRIBED { get; set; }
        public string ROSTATUS { get; set; }
        public string FILEPATH { get; set; }


    }

    public class ModelCount
    {
        public int MID { get; set; }
        public string MODEL { get; set; }
        public int PDF { get; set; }
        public int Video { get; set; }
        public string MSTATUS { get; set; }
        public string LID { get; set; }

    }

    public class DMSLanguages
    {
        public string LANGUAGE_CODE { get; set; }
        public string LANGUAGE_DESC { get; set; }

    }


    public class DMSRegion
    {
        public string REGION_CD { get; set; }
        public string REGION_DESC { get; set; }

    }


    public class DMSdealerdeatails
    {

        public string CITY_DESC { get; set; }
        public string FOR_CD { get; set; }
        public string LOC_CD { get; set; }
        public string LOC_DESC { get; set; }
        public string MUL_SRV_DEALER_CD { get; set; }
        public string OUTLET_CD { get; set; }
        public string REGION_CD { get; set; }
        public string REGION_DESC { get; set; }
        public string WORKSHOP_CD { get; set; }
        public string WORKSHOP_NAME { get; set; }
        public string ZONE_CD { get; set; }
        public string ZONE_DESC { get; set; }


    }


    public class DLRdeatils
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<DLRdeatilslist> result { get; set; }

    }

    public class DLRdeatilslist
    {
        public string CITY_DESC { get; set; }
        public string DEALER_CODE { get; set; }

        public string DEALER_NAME { get; set; }
        public string FOR_CD { get; set; }
        public string LOC_CD { get; set; }
        public string LOC_DESC { get; set; }
        public string OUTLET_CD { get; set; }
        public string REGION_CD { get; set; }

    }





    public class consolidateresponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public List<ConsolidateReportList> result { get; set; }

    }

    public class ConsolidateReportList
    {

        public string Region { get; set; }
        public string DealerName { get; set; }

        public string DealerCode { get; set; }
        public string ForCode { get; set; }
        public string Outletcode { get; set; }
        public string Location { get; set; }
        public string UserID { get; set; }
        public string Language { get; set; }
        public string Model { get; set; }
        public string Manuals { get; set; }

        public int FID { get; set; }
        public decimal UsageTime { get; set; }

    }





    public class UsageReportResponce
    {
        public string code { get; set; }
        public string message { get; set; }
        public UsageReport result { get; set; }
    }
    public class UsageReportList
    {
        public string FID { get; set; }
        public string Models { get; set; }
        public string Language { get; set; }
        public int NO_OF_TIMES_USED { get; set; }
        public decimal  TOTAL_TIME_USED { get; set; }
        public string USEDBY { get; set; }
        public string ROSTATUS { get; set; }
        public DateTime? DATE { get; set; }
        public string MANUALS { get; set; }
    }
    public class UsageReport
    {
        public string WORKSHOP_NAME { get; set; }//
        public string WORKSHOP_CD { get; set; }//

        public int TOTAL_NUMBER_SUBSCRIBES { get; set; }//

        public DateTime REPORTGENERATEDDATE { get; set; }//
        public int TOTAL_NUMBER_USERS { get; set; }//
        public List<UsageReportList> UsageReportData { get; set; }
        //added dms web service key 

    }


    public class Reportrequest
    {
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public Nullable<DateTime> Fromdate { get; set; }
        public Nullable<DateTime> Todate { get; set; }
    }

    public class ReportrequestConsolidate
    {
        public Nullable<DateTime> Fromdate { get; set; }
        public Nullable<DateTime> Todate { get; set; }
    }


    public class SubscribeResponce
    {
        public string code { get; set; }
        public string message { get; set; }
        public IEnumerable<Subscribe> result { get; set; }
        //public IEnumerable<Subscribe1> result { get; set; }
    }

    public class SubscribeResponce_Final
    {
        public string code { get; set; }
        public string message { get; set; }
        public IEnumerable<SUBSUCRIBE_FINAL> result { get; set; }
        //public IEnumerable<Subscribe1> result { get; set; }
    }

    //[Table("tblSUBSUCRIBE_FINAL")]
    //public class SUBSUCRIBE_FINAL
    //{
    //    [Key]
    //    public int ID { get; set; }
    //    public int FID { get; set; }
    //    public int SID { get; set; }
    //    public string DEALERCODE { get; set; }
    //    public string USEDBY { get; set; }
    //    public Nullable<DateTime> STARTTIME { get; set; }
    //    public Nullable<DateTime> ENDTIME { get; set; }
    //    //public string CREATEDDATE { get; set; }
    //    public string MODIFEDEFDATE { get; set; }
    //    public string TotalTime { get; set; }
    //}

    //[Table("tblSubsribe")]
    //public class Subscribe
    //{
    //    [Key]
    //    public int ID { get; set; }
    //    public int FID { get; set; }
    //    public int? SID { get; set; }

    //    public string DEALERCODE { get; set; }
    //    public string USEDBY { get; set; }

    //}


}