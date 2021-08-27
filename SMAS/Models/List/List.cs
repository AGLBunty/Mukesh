using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity.Core;
using System.ComponentModel.DataAnnotations;
using SMAS.Models.Common;

namespace SMAS.Models.List
{

	[Table("tblcompany")]
	public class Company
	{
		[Key]
		public int CID { get; set; }
		public string CNAME { get; set; }
		public bool CISACTIVE { get; set; }
	}
	[Table("tbldesignation")]
	public class Designation
	{
		[Key]
		public int DID { get; set; }
		public string DNAME { get; set; }
		public bool DISACTIVE { get; set; }
	}
	[Table("tblvertical")]
	public class Vertical
	{
		[Key]
		public int VID { get; set; }
		public string VNAME { get; set; }
		public bool VISACTIVE { get; set; }
	}

	[Table("Assigns")]
	public class Assign
	{
		[Key]
		public int AID { get; set; }
		public string ANAME { get; set; }
		public bool ACITIVE { get; set; }
	}
	[Table("tblLanguages")]
	public class Languages
	{
		[Key]
		public int LID { get; set; }
		public string LANGUAGE { get; set; }
		public string LCODE { get; set; }
		public bool LSTATUS { get; set; }
		public DateTime LDATE { get; set; }
	}
	[Table("tblModel")]
	public class Model
	{
		[Key]
		public int MID { get; set; }
		public string MODEL { get; set; }
		public string MSTATUS { get; set; }
		public string MCODE { get; set; }
		public int? LID { get; set; }
		public bool MPUBLICSTATUS { get; set; }
		public string PAYMENTSSTATUS { get; set; }
		public string IMAGEPATH { get; set; }
		public string TITLE { get; set; }
		public bool MACTIVE { get; set; }
		public DateTime? MDATE { get; set; }
		public DateTime? MODEFIEDDATE { get; set; }
	}
	[Table("tblFileVersions")]
	public class FileVersions
	{
		[Key]
		public int FID { get; set; }
		public string FVERSION { get; set; }
		public int? SID { get; set; }
		public bool FSTATUS { get; set; }
		public DateTime? FDATE { get; set; }
	}
	[Table("tblServiceManuals")]
	public class ServiceManuals
	{
		[Key]
		public int SID { get; set; }
		public string STYPE { get; set; }
		public bool SSTATUS { get; set; }
		public DateTime? SDATE { get; set; }
	}
	[Table("tblFileUpload")]
	public class FileUpload
	{
        [Key]
        public int ID { get; set; }
		public string ROSTATUS { get; set; }
		public string  MID { get; set; }
		public int SID { get; set; }
		public string FID { get; set; }
		public string LID { get; set; }
		public string TITLE { get; set; }
		public string PSTATUS { get; set; }
		public string FILEPATH { get; set; }
		public bool ISACTIVE { get; set; }
		public string ISPUBLISH { get; set; }
		public DateTime? CREATEDATE { get; set; }
		public DateTime? MODEFIEDDATE { get; set; }
	}
	[Table("tblGetDataModel")]
	public class Request
	{
		public int SID { get; set; }
		public string MID { get; set; }
		public string LID { get; set; }
		public  string DEALERCODE { get; set; }
		public string ROSTATUS { get; set; }
	}
    [Table("tblSubsribe")]
    public class Subscribe
    {
        [Key]
        public int ID { get; set; }
        public int FID { get; set; }
        public int? SID { get; set; }
        public string DEALERCODE { get; set; }
        public string USEDBY { get; set; }
        public string SUBSRIBEAMOUNT { get; set; }
    }


    [Table("tblSUBSUCRIBE_FINAL")]
    public class SUBSUCRIBE_FINAL
    {
        [Key]
        public int ID { get; set; }
        public int FID { get; set; }
        public int SID { get; set; }
        public string DEALERCODE { get; set; }
        public string USEDBY { get; set; }
        public Nullable<DateTime> STARTTIME { get; set; }
        public Nullable<DateTime> ENDTIME { get; set; }
        public Nullable<DateTime> CREATEDDATE { get; set; }
        public Nullable<DateTime> MODIFEDEFDATE { get; set; }
        public string TotalTime { get; set; }
        public string DEVICENAME { get; set; }

    }


    public class FileUploadModel
    {
        
        [Display(Name = "Runing")]
        [Required(ErrorMessage = "{0} is required.")]
        public string ROSTATUS { get; set; }

        [Display(Name = "Model")]
        [Required(ErrorMessage = "{0} is required.")]
        public string MID { get; set; }

        [Display(Name = "Service")]
        [Required(ErrorMessage = "{0} is required.")]
        public int SID { get; set; }

        [Display(Name = "Languages")]
        [Required(ErrorMessage = "{0} is required.")]
        public string LID { get; set; }


        //[AllowFileSize(FileSize = 5 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 5 MB")]
        // [FileTypes("jpeg,png,jpg,gif,mp4,pdf")]
        // [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.mp4|.pdf)$", ErrorMessage = "Only pdf,video(.mp4) files allowed.")]
        [Required(ErrorMessage = "Please select file.")]
       // [RegularExpression(@"([/^[ A-Za-z0-9_@./#&+-]*$/])+(.mp4|.pdf)$", ErrorMessage = "Only pdf,video(.mp4) files allowed.")]
        public HttpPostedFileBase PostedFile { get; set; }

        public string FID { get; set; }

        public string PSTATUS { get; set; }
        
      
    }
	public class FileUploadPublishUpdate
	{
		public string ID { get; set; }
		public string ISPUBLISH { get; set; }
		public bool ISACTIVEDELETE { get; set; }
		public bool ISACTIVE { get; set; }

		public static implicit operator FileUploadPublishUpdate(Employee v)
		{
			throw new NotImplementedException();
		}
	}
	[Table("tblFeedbacks")]
	public class Feedbacks
	{
		[Key]
		public int ID { get; set; }
		public string DEALERCODE { get; set; }

        [Display(Name = "Feedback")]
        [Required(ErrorMessage = "{0} is required.")]
        public string FEEDBACK { get; set; }
		public string MID { get; set; }
		public string LID { get; set; }
		public string SID { get; set; }
		public string FEEDBACKMODE { get; set; }
		public string USERID { get; set; }
		//public DateTime CREATEDDATE { get; set; }
		//public DateTime MODEFIEDDATE { get; set; }

	}

	public class FeedbackData
	{
	   public string DEALERCODE { get; set; }
		public string FEEDBACK { get; set; }
		public string MID { get; set; }
		public string LID { get; set; }
		public string SID { get; set; }
		public string FEEDBACKMODE { get; set; }
		public string USERID { get; set; }
	}

    [Table("tblDropdownList")]
    public class DropdownValues
    {
        [Key]
        public int Id { get; set; }
        public string DropdownValue { get; set; }
        public string Parent { get; set; }
    }
}