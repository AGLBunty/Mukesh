using SMAS.Models.ExceptionDT;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace SMAS.Models.List
{
    public class ListRepository : IListRepository
    {
        private readonly SqlDbContext db = new SqlDbContext();
        public  IEnumerable<Assign> GetAssign()
        {
            try
            {
                var Assigns =  db.Assigns.ToList();
                return Assigns;
            }
            catch 
            {
                throw;
            }
        }


        public IEnumerable<FileUpload> GetFileUploaddata()
        {
            try
            {
                var tblFileUploads = db.tblFileUpload.ToList();
                return tblFileUploads;
            }
            catch
            {
                throw;
            }
        }


        

        public IEnumerable<Company> GetCompany()
        {
            try
            {
                var Companys =  db.tblcompany.ToList();
                return Companys.AsQueryable();
            }
            catch 
            {
                throw;
            }
        }
       public async Task<IEnumerable<Designation>> GetDesignation()
        {
            try
            {
                var designations = await db.tbldesignation.ToListAsync();
                return designations.AsQueryable();
            }
            catch
            {
                throw;
            }
        }

		public async Task<IEnumerable<FileVersions>> GetFileVersions()
		{
			try
			{
				var FileVersions = await db.tblFileVersions.ToListAsync();
				return FileVersions.AsQueryable();
			}
			catch
			{
				throw;
			}
		}

		public async Task<IEnumerable<Languages>> GetLanguages()
		{
			try
			{
				var Languages = await db.tblLanguages.ToListAsync();
				return Languages.AsQueryable();
			}
			catch
			{
				throw;
			}
		}

		public async Task<IEnumerable<Model>> GetModel()
		{
			try
			{
				var Model = await db.tblModel.ToListAsync();
				return Model.AsQueryable();
			}
			catch
			{
				throw;
			}
		}
		public async Task<IEnumerable<FileVersions>> GetFileVersionsbyId(string SID)
		{
			try
			{


				var FileVersions = await db.tblFileVersions.ToListAsync();

				
				if (FileVersions == null)
				{
					return null;
				}
				return FileVersions.Where(st => st.SID == int.Parse(SID));
			}
			catch
			{
				throw;
			}
		}

		public async Task<IEnumerable<Vertical>> GetVertical()
        {
            try
            {
                var verticals = await db.tblvertical.ToListAsync();
                return verticals.AsQueryable();
            }
            catch
            {
                throw;
            }
        }

		public  IEnumerable<ModelCount> GetDataCountbyMid(string MSTATUS)
		{
			try
			{
				List<ModelCount> groups = new List<ModelCount>();
				//groups = (from g in db.tblModel
				//		  join gu in db.tblFileUpload on g.MID equals gu.MID into vs
				//		  from gu in vs.DefaultIfEmpty()
				//		  where g.MSTATUS == MSTATUS
				//		  select new ModelCount
				//		  {
				//			  MID = g.MID,
				//			  MODEL = g.MODEL,
				//			  MSTATUS=g.MSTATUS,
				//			  PDF = (from c in db.tblFileUpload where c.MID == g.MID && c.SID==1 select c).Count(),
				//			  Video = (from d in db.tblFileUpload where d.MID == g.MID && d.SID == 2 select d).Count()
				//		  }).ToList<ModelCount>();

				return  groups;
			}
			
			catch
			{
				throw;
			}
			
		}

		public async Task<ModelMainResponce> GetDataCountbyMidDataa(string MSTATUS, string LID)
		{
			ModelMainResponce Modaldetails = new ModelMainResponce();
			try
			{
				string json = "{\"PN_PMC\":\"1\",\"PN_SVAR_TYPE\":\"" + MSTATUS + "\"}";
				var response = WebHTTP.CallForDMSServicesEnquiries("SMAS_SVAR", json);
				JavaScriptSerializer js = new JavaScriptSerializer();
				ResponseDataList<Modaldetails> objresults = js.Deserialize<ResponseDataList<Modaldetails>>(response.ToString());
				var FileUplods = await db.tblFileUpload.ToListAsync();
				List<Modaldetails> groups = new List<Modaldetails>();
				groups = (from order in objresults.Result
						  //join plan in FileUplods on order.SMAS_MODEL_CD equals plan.MID into vs
						  //from plans in vs.DefaultIfEmpty()
						  select new Modaldetails
						  {
							  SMAS_MODEL_CD = order.SMAS_MODEL_CD,
							  SERVICE_CODE = order.SERVICE_CODE,
							  SMAS_MODEL_DESC = order.SMAS_MODEL_DESC,
							  LID =LID.Replace("\0", string.Empty),


				PDF = (from c in FileUplods where c.MID == order.SMAS_MODEL_CD && c.SID == 1 &&c.ISACTIVE==false && c.ROSTATUS== MSTATUS && c.LID== LID select c).Count(),
							  Video = (from d in FileUplods where d.MID == order.SMAS_MODEL_CD && d.SID == 2 && d.ISACTIVE == false && d.ROSTATUS == MSTATUS &&d.LID== LID select d).Count()

						  }).ToList<Modaldetails>();
				Modaldetails.result=groups;
				Modaldetails.code = "200";
				Modaldetails.message = "success";
			
			}
			catch (Exception ex)
			{
				BalExceptionDetails.WriteException(ex);

				Modaldetails.result = new List<Modaldetails>(); 
				Modaldetails.code = "202";
				Modaldetails.message = "failure";
			}
			return Modaldetails;
		}

		public async Task<DealerFileResponce> GetDatabydealercode(string MID, string LID, int SID,string DEALERCODE, string ROSTATUS)
		{

            DealerFileResponce _objFileResponce = new DealerFileResponce();
			try
			{
				IEnumerable<FileUpload> _objfilelist = new List<FileUpload>();
				var FileUploads = await db.tblFileUpload.ToListAsync();
				if (FileUploads == null)
				{
					return null;
				}
				if (Convert.ToInt32(MID) != 0 && !string.IsNullOrEmpty(LID) && SID != 0 )
				{
					_objfilelist = FileUploads.Where(st => st.MID == MID && st.LID == LID && st.SID == SID&&st.ISPUBLISH=="Y" && st.ISACTIVE==false && st.ROSTATUS== ROSTATUS);
				}
				else if (Convert.ToInt32(MID) != 0 && !string.IsNullOrEmpty(LID))
				{
					_objfilelist = FileUploads.Where(st => st.MID == MID && st.LID == LID && st.ISPUBLISH == "Y" && st.ISACTIVE == false && st.ROSTATUS == ROSTATUS);
				}
				else
				{
					_objfilelist = FileUploads.Where(st => st.MID == MID && st.ISPUBLISH == "Y" && st.ISACTIVE == false && st.ROSTATUS == ROSTATUS);
				}
				var Subsribelist= await db.tblSubsribe.ToListAsync();
                if (Subsribelist == null)
                {
                    return null;
                }
                List<SubscribeModel> _objdealerlistdata = new List<SubscribeModel>();
                _objdealerlistdata = (from filedata in _objfilelist
                                      select new SubscribeModel
                                      {
                                          ID = filedata.ID,
                                          TITLE = filedata.TITLE,
                                          ROSTATUS = filedata.ROSTATUS,
                                          SID = filedata.SID,
                                              FILEPATH = filedata.FILEPATH,
                                              ISSUBSCRIBED = (from s in Subsribelist where s.FID == filedata.ID && s.DEALERCODE == DEALERCODE select s).Any()
                                          }).ToList<SubscribeModel>();
				_objFileResponce.code = "200";
				_objFileResponce.message = "success";
				_objFileResponce.result = _objdealerlistdata;

			}
			catch (Exception ex)
			{
				  BalExceptionDetails.WriteException(ex);
				_objFileResponce.code = "202";
				_objFileResponce.message = "failare";
				_objFileResponce.result = new List<SubscribeModel>();

			}
			return _objFileResponce;
		}
		public async Task<FileResponce> GetDatabyMId(string MID, string LID, int SID, string ROSTATUS)
		{
			FileResponce _objFileResponce = new FileResponce();
			try
			{
				IEnumerable<FileUpload> _objfilelist = new List<FileUpload>();
				var FileUploads = await db.tblFileUpload.ToListAsync();
				if (FileUploads == null)
				{
					return null;
				}
				if (Convert.ToInt32(MID) != 0 && !string.IsNullOrEmpty(LID) && SID != 0)
				{
					_objfilelist= FileUploads.Where(st => st.MID == MID && st.LID == LID && st.SID == SID && st.ROSTATUS == ROSTATUS);
				}
				else if (Convert.ToInt32(MID) != 0 && !string.IsNullOrEmpty(LID))
				{
					_objfilelist= FileUploads.Where(st => st.MID == MID && st.LID == LID && st.ROSTATUS == ROSTATUS);
				}
				else
				{
					_objfilelist= FileUploads.Where(st => st.MID == MID && st.ROSTATUS == ROSTATUS);
				}
				_objFileResponce.code = "200";
				_objFileResponce.message = "success";
				_objFileResponce.result = _objfilelist;

			}
			catch (Exception ex)
			{
				BalExceptionDetails.WriteException(ex);
				_objFileResponce.code = "202";
				_objFileResponce.message= "failare";
				_objFileResponce.result = new List<FileUpload>();
				
			}
			return _objFileResponce;
		}
		public async Task<ResponceServiceManuals> GetServiceManuals()
		{
			ResponceServiceManuals _objservicemannualresponce = new ResponceServiceManuals();
			try
			{

				IEnumerable<ServiceManuals> _objservicesfilelist = new List<ServiceManuals>();
				var ServiceManuals = await db.tblServiceManuals.ToListAsync();
				_objservicesfilelist= ServiceManuals.AsQueryable();
				_objservicemannualresponce.code = "200";
				_objservicemannualresponce.message = "success";
				_objservicemannualresponce.result = _objservicesfilelist;
			}
			catch (Exception ex)
			{
				BalExceptionDetails.WriteException(ex);
				_objservicemannualresponce.code = "202";
				_objservicemannualresponce.message = "failare";
				_objservicemannualresponce.result = new List<ServiceManuals>();
			}
			return _objservicemannualresponce;
		}
		public async Task<UpdateResponce> FileUploadModelUpdate(int ID)
		{
			UpdateResponce responce = new UpdateResponce();
			try
			{
				//FileUpload tblFileUpload = db.tblFileUpload.Where(F => F.ID == ID).SingleOrDefault();
				FileUpload tblFileUpload = await db.tblFileUpload.FindAsync(ID);
				tblFileUpload.ISPUBLISH = "Y";
				DateTime modifeddate = DateTime.Now;
				tblFileUpload.MODEFIEDDATE = modifeddate;
				db.Entry(tblFileUpload).State = EntityState.Modified;
				await db.SaveChangesAsync();
				responce.result = true;
				responce.code = "200";
				responce.message = "success";
			}
			catch(Exception ex)
			{
				BalExceptionDetails.WriteException(ex);
				responce.code = "202";
				responce.message = "failare";
				responce.result = false;
			}

			return responce;



		}
		//public async Task<UpdateResponce> SubscribeSaveData(int FID, string DEALERCODE)
		//{
		//	UpdateResponce responce = new UpdateResponce();
		//	try
		//	{
		//		Subscribe _objdata = new Subscribe();
		//		_objdata.FID = FID;
		//		_objdata.DEALERCODE = DEALERCODE;
		//		db.tblSubsribe.Add(_objdata);
		//		await db.SaveChangesAsync();
		//		responce.result = true;
		//		responce.code = "200";
		//		responce.message = "success";
		//	}
		//	catch (Exception ex)
		//	{
		//		BalExceptionDetails.WriteException(ex);
		//		responce.code = "202";
		//		responce.message = "failare";
		//		responce.result = false;
		//	}
		//	//return responce;
		//	return responce;
		//}
		public async Task<FeedbackResponce> GetFeedback()
		{
			FeedbackResponce _objFeedbackResponce = new FeedbackResponce();
			try
			{
				
				IEnumerable<Feedbacks> _objfeedbacklist = new List<Feedbacks>();
				_objfeedbacklist = await db.tblFeedbacks.ToListAsync(); //db.tblModel.ToListAsync();
																		//return _objfilelist.AsQueryable();
				_objFeedbackResponce.code = "200";
				_objFeedbackResponce.message = "success";
				_objFeedbackResponce.result = _objfeedbacklist;
			}
			catch(Exception ex)
			{
				BalExceptionDetails.WriteException(ex);
				_objFeedbackResponce.code = "202";
				_objFeedbackResponce.message = "failare";
				_objFeedbackResponce.result = new List<Feedbacks>();
			}
			return _objFeedbackResponce;
		}
		public async Task<UpdateResponce> FeedbackSaveData(Feedbacks feedsubdata)
		{
			UpdateResponce responce = new UpdateResponce();
			try
			{
				 Feedbacks _objfeedbackdata = new Feedbacks();
				_objfeedbackdata.DEALERCODE = feedsubdata.DEALERCODE;
				_objfeedbackdata.FEEDBACK = feedsubdata.FEEDBACK;
				_objfeedbackdata.MID = feedsubdata.MID;
				_objfeedbackdata.LID = feedsubdata.LID;
				_objfeedbackdata.SID = feedsubdata.SID;
				_objfeedbackdata.FEEDBACKMODE = feedsubdata.FEEDBACKMODE;
				_objfeedbackdata.USERID = feedsubdata.USERID;
				db.tblFeedbacks.Add(_objfeedbackdata);
				await db.SaveChangesAsync();
				responce.result = true;
				responce.code = "200";
				responce.message = "success";
			}
			catch(Exception ex)
			{
				responce.code = "202";
				responce.message = "failare";
				responce.result = false;
			}
			//return responce;
			return responce;
		}
		public async Task<UpdateResponce> FileUploadDelete(int ID)
		{
			UpdateResponce responce = new UpdateResponce();
			try
			{
				//FileUpload tblFileUpload = db.tblFileUpload.Where(F => F.ID == ID).SingleOrDefault();
				FileUpload tblFileUpload = await db.tblFileUpload.FindAsync(ID);
				//tblFileUpload.ISPUBLISH = "Y";
				tblFileUpload.ISACTIVE =true;
				DateTime modifeddate = DateTime.Now;
				tblFileUpload.MODEFIEDDATE = modifeddate;
				db.Entry(tblFileUpload).State = EntityState.Modified; //EntityState.Modified;
				await db.SaveChangesAsync();
				responce.result = true;
				responce.code = "200";
				responce.message = "success";
			}
			catch (Exception ex)
			{
				BalExceptionDetails.WriteException(ex);
				responce.code = "202";
				responce.message = "failare";
				responce.result = false;
			}

			return responce;
		}
		public async Task<FileResponce> GetDataFilePath(FileUpload objreq)
		{
			FileResponce _objFileResponce = new FileResponce();
			try
			{
				IEnumerable<FileUpload> _objfilelist = new List<FileUpload>();
				var FileUploads = await db.tblFileUpload.ToListAsync();
				if (FileUploads == null)
				{
					return null;
				}
				else if (Convert.ToInt32(objreq.ID) != 0)
				{
					_objfilelist = FileUploads.Where(st => st.ID== objreq.ID);
				}
				_objFileResponce.code = "200";
				_objFileResponce.message = "success";
				_objFileResponce.result = _objfilelist;

			}
			catch (Exception ex)
			{
				BalExceptionDetails.WriteException(ex);
				_objFileResponce.code = "202";
				_objFileResponce.message = "failare";
				_objFileResponce.result = new List<FileUpload>();

			}
			return _objFileResponce;
		}
		public async Task<ModelMainResponce> GetDealermodellist(string MSTATUS, string LID)
		{
			ModelMainResponce Modaldetails = new ModelMainResponce();
			try
			{
				string json = "{\"PN_PMC\":\"1\",\"PN_SVAR_TYPE\":\"" + MSTATUS + "\"}";
				var response = WebHTTP.CallForDMSServicesEnquiries("SMAS_SVAR", json);
				JavaScriptSerializer js = new JavaScriptSerializer();
				ResponseDataList<Modaldetails> objresults = js.Deserialize<ResponseDataList<Modaldetails>>(response.ToString());
				var FileUplods = await db.tblFileUpload.ToListAsync();
				List<Modaldetails> groups = new List<Modaldetails>();
				groups = (from order in objresults.Result
							  //join plan in FileUplods on order.SMAS_MODEL_CD equals plan.MID into vs
							  //from plans in vs.DefaultIfEmpty()
						  select new Modaldetails
						  {
							  SMAS_MODEL_CD = order.SMAS_MODEL_CD,
							  SERVICE_CODE = order.SERVICE_CODE,
							  SMAS_MODEL_DESC = order.SMAS_MODEL_DESC,
							  LID = LID.Replace("\0", string.Empty),


							  PDF = (from c in FileUplods where c.MID == order.SMAS_MODEL_CD && c.SID == 1 && c.ISACTIVE == false && c.ISPUBLISH=="Y" && c.ROSTATUS == MSTATUS && c.LID == LID select c).Count(),
							  Video = (from d in FileUplods where d.MID == order.SMAS_MODEL_CD && d.SID == 2 && d.ISACTIVE == false && d.ISPUBLISH == "Y" && d.ROSTATUS == MSTATUS && d.LID == LID select d).Count()

						  }).ToList<Modaldetails>();
				Modaldetails.result = groups;
				Modaldetails.code = "200";
				Modaldetails.message = "success";

			}
			catch (Exception ex)
			{
				BalExceptionDetails.WriteException(ex);

				Modaldetails.result = new List<Modaldetails>();
				Modaldetails.code = "202";
				Modaldetails.message = "failure";
			}
			return Modaldetails;
		}

        public async Task<UsageReportResponce> GetUsageReport(Reportrequest _ObjReport)
        {
            UsageReportResponce UsageReport = new UsageReportResponce();
            try
            {
                //string json = "{\"PN_PMC\":\"1\",\"PN_REGN_CD\":\"" + _ObjReport.Regioncode + "\"}";//regin code nhi need h
                //var response = WebHTTP.CallForDMSServicesEnquiries("GET_DEALER_DTL", json);
                //JavaScriptSerializer js = new JavaScriptSerializer();
                //ResponseDataList<UsageReport> objresults = js.Deserialize<ResponseDataList<UsageReport>>(response.ToString());

                //string jsonModel = "{\"PN_PMC\":\"1\",\"PN_SVAR_TYPE\":\"" + _ObjReport.ROSTATUS + "\"}";
                //var responseModel = WebHTTP.CallForDMSServicesEnquiries("SMAS_SVAR", jsonModel);
                //JavaScriptSerializer jsModel = new JavaScriptSerializer();
                //ResponseDataList<UsageReportList> objresultsModel = js.Deserialize<ResponseDataList<UsageReportList>>(responseModel.ToString());

                //string jsonLanguage = "{\"PN_PMC\":\"1\"}";
                //var responselanguages = WebHTTP.CallForDMSServicesEnquiries("SMAS_LANGUAGE", jsonLanguage);
                //JavaScriptSerializer jsLanguage = new JavaScriptSerializer();
                //ResponseDataList<DMSLanguages> objlangresults = jsLanguage.Deserialize<ResponseDataList<DMSLanguages>>(responselanguages.ToString());

                //var _objsubscribelist = await db.tblSubsribe.ToListAsync();
                var FileUplods = await db.tblFileUpload.ToListAsync();
                var _objsubscribeFinal = await db.tblSUBSUCRIBE_FINAL.Where(x=>x.TotalTime!=null && (DbFunctions.TruncateTime(x.STARTTIME)>= DbFunctions.TruncateTime(_ObjReport.Fromdate) && DbFunctions.TruncateTime(x.STARTTIME) <= DbFunctions.TruncateTime(_ObjReport.Todate))&& x.DEALERCODE == _ObjReport.DealerCode).ToListAsync();
                //var FileUpload = await db.tblFileUpload.ToListAsync();

                UsageReport groups = new UsageReport();
                groups.WORKSHOP_NAME = _ObjReport.DealerName; //objresults.Result.Where(x => x.WORKSHOP_CD == "1311-13-02").Select(y => y.WORKSHOP_NAME).SingleOrDefault();//input dealercode
                groups.WORKSHOP_CD = _ObjReport.DealerCode;
                groups.TOTAL_NUMBER_USERS = (from d in _objsubscribeFinal where d.DEALERCODE == _ObjReport.DealerCode select d.USEDBY).Distinct().Count();
                groups.REPORTGENERATEDDATE = DateTime.Now;
                groups.TOTAL_NUMBER_SUBSCRIBES = (from c in _objsubscribeFinal where c.DEALERCODE == _ObjReport.DealerCode && c.SID == 1 select c.FID).Distinct().Count();
                List<UsageReportList> ReportDatabyList = new List<UsageReportList>();


                List<UsageReportList> result = _objsubscribeFinal
    //.Where(c => c.DEALERCODE == "1311-13-02")
    .GroupBy(l => new { l.DEALERCODE, l.FID, l.USEDBY })
    .Select(cl => new UsageReportList
    {
        FID = cl.First().FID.ToString(),
        NO_OF_TIMES_USED = cl.Count(),
        TOTAL_TIME_USED = cl.Sum(c => Convert.ToDecimal(c.TotalTime.Replace("Hours", "").Replace(":", "."))),
        DATE = cl.First().CREATEDDATE,
        USEDBY = cl.First().USEDBY,
        ROSTATUS = (from c in FileUplods where c.ID == cl.First().FID && c.SID == 1 && c.ISACTIVE == false && c.ISPUBLISH == "Y" select new { c.ROSTATUS }).Select(x => x.ROSTATUS).SingleOrDefault() == "Y" ? "Running" : "Obsolete",
        //Language = (from c in FileUplods
        //join lang in objlangresults.Result on c.LID equals lang.LANGUAGE_CODE
        //where c.ID == cl.First().FID && c.SID == 1 && c.ISACTIVE == false && c.ISPUBLISH == "Y" select new { lang.LANGUAGE_DESC}).Select(x => x.LANGUAGE_DESC).SingleOrDefault(),

        Language = clsBLLHttpUtility.Getlanguagebyid((from c in FileUplods
                    where c.ID == cl.First().FID && c.SID == 1 && c.ISACTIVE == false && c.ISPUBLISH == "Y"
                    select new { c.LID }).Select(x => x.LID).SingleOrDefault()),
        Models = clsBLLHttpUtility.Getmodelname((from c in FileUplods where c.ID == cl.First().FID && c.SID == 1 && c.ISACTIVE == false && c.ISPUBLISH == "Y" select new { c.MID }).Select(x => x.MID).SingleOrDefault()),
        MANUALS = (from c in FileUplods where c.ID == cl.First().FID && c.SID == 1 && c.ISACTIVE == false && c.ISPUBLISH == "Y" select new { c.TITLE }).Select(x => x.TITLE).SingleOrDefault(),
    }).ToList();

                groups.UsageReportData = result;
                UsageReport.result = groups;
                UsageReport.code = "200";
                UsageReport.message = "success";
            }
            catch (Exception ex)
            {
                BalExceptionDetails.WriteException(ex);
                UsageReport.result = new UsageReport();
                UsageReport.code = "202";
                UsageReport.message = "failure";
            }
            return UsageReport;
        }





        public async Task<consolidateresponse> GetCosolidateReport(ReportrequestConsolidate _ObjReport)
        {
            consolidateresponse UsageReport = new consolidateresponse();
            try
            {
               
               var FileUplods = await db.tblFileUpload.ToListAsync();
                var _objsubscribeFinal = await db.tblSUBSUCRIBE_FINAL.Where(x=>x.TotalTime != null && (DbFunctions.TruncateTime(x.STARTTIME) >= DbFunctions.TruncateTime(_ObjReport.Fromdate) && DbFunctions.TruncateTime(x.STARTTIME) <= DbFunctions.TruncateTime(_ObjReport.Todate))).ToListAsync();


     List<ConsolidateReportList> result = _objsubscribeFinal
    .GroupBy(l => new { l.DEALERCODE, l.FID, l.USEDBY })
    .Select(cl => new ConsolidateReportList
    {

      //  Region = clsBLLHttpUtility.GetdearlerdeatilsRegion(cl.First().DEALERCODE, cl.First().USEDBY),
       // DealerName = clsBLLHttpUtility.Getdearlerdeatilsdealername(cl.First().DEALERCODE, cl.First().USEDBY),

        //ForCode = clsBLLHttpUtility.Getdearlerdeatilsforcode(cl.First().DEALERCODE, cl.First().USEDBY),
        //Outletcode = clsBLLHttpUtility.Getdearlerdeatilsoutcode(cl.First().DEALERCODE, cl.First().USEDBY),
        //Location = clsBLLHttpUtility.Getdearlerdeatilslocation(cl.First().DEALERCODE, cl.First().USEDBY),
       // Manuals = (from c in FileUplods where c.ID == cl.First().FID && c.SID == 1 && c.ISACTIVE == false && c.ISPUBLISH == "Y" select new { c.TITLE }).Select(x => x.TITLE).SingleOrDefault(),
       // Region = "",
       // DealerName = "",
        ForCode = "",
        Outletcode = "",
        Location = "",
        Manuals ="",
       // UsageTime =0.0M,
        DealerCode = cl.First().DEALERCODE,
        UserID = cl.First().USEDBY,
        Language ="",
        Model ="",
        FID= cl.First().FID,
        //Language = clsBLLHttpUtility.Getlanguagebyid((from c in FileUplods
        //                                              where c.ID == cl.First().FID && c.SID == 1 && c.ISACTIVE == false && c.ISPUBLISH == "Y"
        //                                              select new { c.LID }).Select(x => x.LID).SingleOrDefault()),
        //Model = clsBLLHttpUtility.Getmodelname((from c in FileUplods where c.ID == cl.First().FID && c.SID == 1 && c.ISACTIVE == false && c.ISPUBLISH == "Y" select new { c.MID }).Select(x => x.MID).SingleOrDefault()),
         UsageTime =cl.Sum(c => Convert.ToDecimal(c.TotalTime.Replace("Hours", "").Replace(":","."))),
    }).ToList();


               


                UsageReport.result = result;
                UsageReport.code = "200";
                UsageReport.message = "success";
            }
            catch (Exception ex)
            {
                BalExceptionDetails.WriteException(ex);
                UsageReport.result = new List<ConsolidateReportList>();
                UsageReport.code = "202";
                UsageReport.message = "failure";
            }
            return UsageReport;
        }






        public async Task<UpdateResponce> SubscribeSaveFinalData(int FID, int SID, string DEALERCODE, string USEDBY, string DEVICENAME)
        {
            UpdateResponce responce = new UpdateResponce();
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(FID)))
                {
                    responce.result = false;
                    responce.code = "202";
                    responce.message = "Please Enter FID";
                    return responce;
                }
                if (FID == 0)
                {
                    responce.result = false;
                    responce.code = "202";
                    responce.message = "Please Enter FID";
                    return responce;
                }
                if (string.IsNullOrEmpty(DEALERCODE))
                {
                    responce.result = false;
                    responce.code = "202";
                    responce.message = "Please Enter Dealer Code";
                    return responce;
                }
                if (string.IsNullOrEmpty(USEDBY))
                {
                    responce.result = false;
                    responce.code = "202";
                    responce.message = "Please Enter User Id ";
                    return responce;
                }
                if (string.IsNullOrEmpty(DEVICENAME))
                {
                    DEVICENAME = "";
                }
                string Final = "Hours";
                SUBSUCRIBE_FINAL _objdata_Final = new SUBSUCRIBE_FINAL();
                var result = db.tblSUBSUCRIBE_FINAL.OrderByDescending(Z => Z.ID).Where(X => X.FID == FID && X.DEALERCODE == DEALERCODE && X.USEDBY == USEDBY).FirstOrDefault();
                DateTime date = DateTime.Now;//Convert.ToDateTime(result.STARTTIME);
                if (result == null)
                {
                    _objdata_Final.SID = SID;
                    _objdata_Final.FID = FID;
                    _objdata_Final.DEALERCODE = DEALERCODE;
                    _objdata_Final.USEDBY = USEDBY;
                    _objdata_Final.DEVICENAME = DEVICENAME;
                    _objdata_Final.STARTTIME = Convert.ToDateTime(DateTime.Now);
                    _objdata_Final.CREATEDDATE = DateTime.Now;
                    db.tblSUBSUCRIBE_FINAL.Add(_objdata_Final);
                    await db.SaveChangesAsync();
                    responce.result = true;
                    responce.code = "200";
                    responce.message = "success";
                }
                else if (result.ENDTIME != null && date.Date == Convert.ToDateTime(result.STARTTIME).Date)
                {
                    _objdata_Final.SID = SID;
                    _objdata_Final.FID = FID;
                    _objdata_Final.DEALERCODE = DEALERCODE;
                    _objdata_Final.USEDBY = USEDBY;
                    _objdata_Final.DEVICENAME = DEVICENAME;
                    _objdata_Final.STARTTIME = DateTime.Now;
                    _objdata_Final.CREATEDDATE = DateTime.Now;
                    db.tblSUBSUCRIBE_FINAL.Add(_objdata_Final);
                    await db.SaveChangesAsync();
                    responce.result = true;
                    responce.code = "200";
                    responce.message = "success";
                }
                else
                {
                    if (date.Date == Convert.ToDateTime(result.STARTTIME).Date)
                    {

                        if (result.STARTTIME > date)
                        {
                            var ts = Convert.ToDateTime(result.STARTTIME).Subtract(date);
                            //int days = ts.Days;
                            int hours = ts.Hours;
                            int minutes = ts.Minutes;
                            int seconds = ts.Seconds;
                            SUBSUCRIBE_FINAL _objdataSubfINAL = await db.tblSUBSUCRIBE_FINAL.FindAsync(result.ID);
                            _objdataSubfINAL.ENDTIME = DateTime.Now;
                            _objdataSubfINAL.TotalTime = Convert.ToString(ts).Substring(0, 5) + "" + Convert.ToString(Final);
                            db.Entry(_objdataSubfINAL).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            responce.result = true;
                            responce.code = "200";
                            responce.message = "Update success";
                        }
                        else
                        {
                            var ts = date.Subtract(Convert.ToDateTime(result.STARTTIME));
                            //int days = ts.Days;
                            int hours = ts.Hours;
                            int minutes = ts.Minutes;
                            int seconds = ts.Seconds;
                            SUBSUCRIBE_FINAL _objdataSubfINAL = await db.tblSUBSUCRIBE_FINAL.FindAsync(result.ID);
                            _objdataSubfINAL.ENDTIME = DateTime.Now;
                            //_objdataSubfINAL.TotalTime = Convert.ToString(ts + "" + Final);
                            _objdataSubfINAL.TotalTime = Convert.ToString(ts).Substring(0, 5) + "" + Convert.ToString(Final);
                            db.Entry(_objdataSubfINAL).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            responce.result = true;
                            responce.code = "200";
                            responce.message = "Update success";
                        }
                    }
                    else
                    {
                        _objdata_Final.SID = SID;
                        _objdata_Final.FID = FID;
                        _objdata_Final.DEALERCODE = DEALERCODE;
                        _objdata_Final.USEDBY = USEDBY;
                        _objdata_Final.DEVICENAME = DEVICENAME;
                        _objdata_Final.STARTTIME = DateTime.Now;
                        _objdata_Final.CREATEDDATE = DateTime.Now;
                        db.tblSUBSUCRIBE_FINAL.Add(_objdata_Final);
                        await db.SaveChangesAsync();
                        responce.result = true;
                        responce.code = "200";
                        responce.message = "success";
                    }

                }

            }
            catch (Exception ex)
            {
                BalExceptionDetails.WriteException(ex);
                responce.code = "202";
                responce.message = "failare";
                responce.result = false;
            }
            return responce;

        }




        public async Task<SubscribeResponce> GetSubsribeData()
        {
            SubscribeResponce _objSubscribeResponce = new SubscribeResponce();
            try
            {
                IEnumerable<Subscribe> _objsubscribelist = new List<Subscribe>();
                _objsubscribelist = await db.tblSubsribe.ToListAsync(); //db.tblModel.ToListAsync();
                _objSubscribeResponce.code = "200";
                _objSubscribeResponce.message = "success";
                _objSubscribeResponce.result = _objsubscribelist;
            }
            catch (Exception ex)
            {
                BalExceptionDetails.WriteException(ex);
                _objSubscribeResponce.code = "202";
                _objSubscribeResponce.message = "failare";
                _objSubscribeResponce.result = new List<Subscribe>();
            }
            return _objSubscribeResponce;
        }

        public async Task<SubscribeResponce_Final> GetSubsribe_FinalData()
        {

            SubscribeResponce_Final _objSubscribeDataResponce = new SubscribeResponce_Final();
            try
            {
                IEnumerable<SUBSUCRIBE_FINAL> _objsubscribeFinallist = new List<SUBSUCRIBE_FINAL>();
                _objsubscribeFinallist = await db.tblSUBSUCRIBE_FINAL.ToListAsync();//await db.tblSubsribe.ToListAsync(); //db.tblModel.ToListAsync();
                _objSubscribeDataResponce.code = "200";
                _objSubscribeDataResponce.message = "success";
                _objSubscribeDataResponce.result = _objsubscribeFinallist;
            }
            catch (Exception ex)
            {
                BalExceptionDetails.WriteException(ex);
                _objSubscribeDataResponce.code = "202";
                _objSubscribeDataResponce.message = "failare";
                _objSubscribeDataResponce.result = new List<SUBSUCRIBE_FINAL>();
            }
            return _objSubscribeDataResponce;
        }



        public async Task<UpdateResponce> SubscribeSaveData(int FID, int? SID, string DEALERCODE, string USEDBY)
        {
            UpdateResponce responce = new UpdateResponce();
            try
            {


                if (string.IsNullOrEmpty(Convert.ToString(FID)))
                {
                    responce.result = false;
                    responce.code = "202";
                    responce.message = "Please Enter FID";
                    return responce;
                }
                if (FID == 0)
                {
                    responce.result = false;
                    responce.code = "202";
                    responce.message = "Please Enter FID";
                    return responce;
                }
                if (string.IsNullOrEmpty(DEALERCODE))
                {
                    responce.result = false;
                    responce.code = "202";
                    responce.message = "Please Enter Dealer Code";
                    return responce;
                }
                if (string.IsNullOrEmpty(USEDBY))
                {
                    responce.result = false;
                    responce.code = "202";
                    responce.message = "Please Enter User Id ";
                    return responce;
                }



                var _objsubscribeFinallist = await db.tblSubsribe.ToListAsync();
                Subscribe _objdata_Final = new Subscribe();
                //var result = db.tblSubsribe.Where(X => X.FID == FID && X.DEALERCODE == DEALERCODE && X.USEDBY == USEDBY).SingleOrDefault();
                DateTime date = DateTime.Now;//Convert.ToDateTime(result.STARTTIME);
                _objdata_Final.SID = SID;
                _objdata_Final.FID = FID;
                _objdata_Final.DEALERCODE = DEALERCODE;
                _objdata_Final.USEDBY = USEDBY;
                //_objdata_Final.STARTTIME = Convert.ToDateTime(DateTime.Now);
                db.tblSubsribe.Add(_objdata_Final);
                await db.SaveChangesAsync();
                responce.result = true;
                responce.code = "200";
                responce.message = "success";

                //else
                //{
                //	if (date.Date == Convert.ToDateTime(result.STARTTIME).Date)
                //	{
                //		Subscribe _objdataSub = await db.tblSubsribe.FindAsync(result.ID);
                //		_objdataSub.ENDTIME = DateTime.Now;
                //		_objdataSub.TotalTime = Convert.ToString(UI);
                //		db.Entry(_objdataSub).State = EntityState.Modified;
                //		await db.SaveChangesAsync();
                //		responce.result = true;
                //		responce.code = "200";
                //		responce.message = "Update success";
                //	}
                //	else
                //	{
                //		_objdata_Final.SID = SID;
                //		_objdata_Final.FID = FID;
                //		_objdata_Final.DEALERCODE = DEALERCODE;
                //		_objdata_Final.USEDBY = USEDBY;
                //		_objdata_Final.STARTTIME = Convert.ToDateTime(DateTime.Now);
                //		db.tblSubsribe.Add(_objdata_Final);
                //		await db.SaveChangesAsync();
                //		responce.result = true;
                //		responce.code = "200";
                //		responce.message = "success";
                //	}

                //}

            }
            catch (Exception ex)
            {
                BalExceptionDetails.WriteException(ex);
                responce.code = "202";
                responce.message = "failare";
                responce.result = false;
            }
            //return responce;
            return responce;
        }

        // add 25-08-2021
        public async Task<List<DropdownValues>> GetValue(string Parent)
        {
            try
            {
                List<DropdownValues> DropdownValue = await db.tblDropdownList.Where(a => a.Parent == Parent).ToListAsync();

                if (DropdownValue == null)
                {
                    return null;
                }
                return DropdownValue;
            }
            catch
            {
                
                throw;
            }
        }


    }

}