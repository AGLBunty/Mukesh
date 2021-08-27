using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMAS.Models;
using SMAS.Models.UserLogin;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Security;
using SMAS.Models.List;
using System.IO;
using System.Web.Script.Serialization;
using PagedList;
using System.Text;
using System.Globalization;

namespace SMAS.Controllers.UserLogins
{
    [Authorize]
    public class AdminController : Controller
    {
        readonly string apiBaseAddress = ConfigurationManager.AppSettings["apiBaseAddress"];


        [HttpPost]
        public  JsonResult modellist(string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                string json = "{\"PN_PMC\":\"1\",\"PN_SVAR_TYPE\":\"" + status + "\"}";
                var response = WebHTTP.CallForDMSServicesEnquiries("SMAS_SVAR", json);
                JavaScriptSerializer js = new JavaScriptSerializer();
                ResponseDataList<Modaldetails> objresults = js.Deserialize<ResponseDataList<Modaldetails>>(response.ToString());

                if (objresults.code == "200")
                {
                    return Json(new SelectList(objresults.Result, "SMAS_MODEL_CD", "SMAS_MODEL_DESC"));
                }

            }
            return null;

        }

        [HttpGet]
        public ActionResult Index()
        {
            string responsecode = string.Empty;
            if (clsBLLHttpUtility.CheckForInternetConnection())
            {
                string jsonlanguages = "{\"PN_PMC\":\"1\"}";
                var responselanguages = WebHTTP.CallForDMSServicesEnquiries("SMAS_LANGUAGE", jsonlanguages);
                JavaScriptSerializer jslang = new JavaScriptSerializer();
                ResponseDataList<DMSLanguages> objlangresults = jslang.Deserialize<ResponseDataList<DMSLanguages>>(responselanguages.ToString());

                if (objlangresults.code == "200")
                {
                    ViewBag.Languagelist = new SelectList(objlangresults.Result, "LANGUAGE_CODE", "LANGUAGE_DESC");
                }
                else
                {
                    ViewBag.Languagelists = null;
                }
                return View();
            }
            else
            {
                ViewBag.Languagelists = null;
                return View(new {id=204 });

            }
           
        }


        public ActionResult usagereport()
        {
            string jsonregion = "{\"PN_PMC\":\"1\"}";
            var responseregions = WebHTTP.CallForDMSServicesEnquiries("REGION_DTL", jsonregion);
            JavaScriptSerializer jslang = new JavaScriptSerializer();
            ResponseDataList<DMSRegion> objlangresults = jslang.Deserialize<ResponseDataList<DMSRegion>>(responseregions.ToString());

            if (objlangresults.code == "200")
            {
                ViewBag.Regionlist = new SelectList(objlangresults.Result, "REGION_CD", "REGION_DESC");
            }
            else
            {
                ViewBag.Regionlist = null;
            }


            return View();
        }



        [HttpPost]
        public JsonResult GetDealers(string Regionid)
        {

            if (!string.IsNullOrEmpty(Regionid))
            {

                IEnumerable <DMSdealerdeatails> _Objdelerlist = null;

                // dynamic _Objdelerlist = null;
                using (var client = new HttpClient())
                {

                    string json = "{\"PN_PMC\":\"1\",\"PN_REGN_CD\":\"" + Regionid + "\"}";
                    var response = WebHTTP.CallForDMSServicesEnquiries("GET_DEALER_DTL", json);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    ResponseDataList<DMSdealerdeatails> objresults = js.Deserialize<ResponseDataList<DMSdealerdeatails>>(response.ToString());

                    if (objresults.code == "200")
                    {
                        //_Objdelerlist = new SelectList(objresults.Result, "WORKSHOP_CD", "WORKSHOP_NAME");

                        _Objdelerlist = objresults.Result;
                    }
                    else
                    {
                        _Objdelerlist = null;
                    }
                   
                    return Json(_Objdelerlist, JsonRequestBehavior.AllowGet);
                  
                }

            }

            return null;


        }




        [HttpPost]
        [ActionName("DownloadReport")]
        public async Task<ActionResult> DownloadReport(ReportrequestConsolidate objreq)
        {
            consolidateresponse resultdata = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                objreq.Fromdate = !string.IsNullOrEmpty(Convert.ToString(objreq.Fromdate)) ? clsBLLHttpUtility.TryParseDateTime(objreq.Fromdate) : DateTime.Now.AddDays(-1);
                objreq.Todate = !string.IsNullOrEmpty(Convert.ToString(objreq.Todate)) ? clsBLLHttpUtility.TryParseDateTime(objreq.Todate) : DateTime.Now;

                var result = await client.PostAsJsonAsync("List/GetconsolidateReport", objreq);
                resultdata = await result.Content.ReadAsAsync<consolidateresponse>();


                if (resultdata.result.Count > 0 && resultdata.code == "200")
                {
                    DateTime dateTime = DateTime.UtcNow.Date;


                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table  style='border:1px solid black; border-collapse:collapse;'>");

                    string[] columnName = { "Region", "Dealer Name", "Dealer Code", "For Code", "Outlet code", "City / Location", "UserID", "Language", "Model", "Manuals", "Usage Time(Hours)" };
                   
                    sb.Append("<tr style='border:1px solid black; border-collapse:collapse;'>");
                    //Looping through the column names
                    foreach (var col in columnName)
                        sb.Append("<td style='font-weight:bold'>" + col + "</td>");
                    sb.Append("</tr>");
                    int i = 0;




                    List<ConsolidateReportList> resultres = new List<ConsolidateReportList>();

                    //api/List/GetFileUploaddata
                    IEnumerable<FileUpload> listFileUpload = null;

                    var FileUploadlist = await client.GetAsync("List/GetFileUploaddata");
                    listFileUpload = await FileUploadlist.Content.ReadAsAsync<IList<FileUpload>>();

                    foreach (var item in resultdata.result)
                    {
                          ConsolidateReportList objConsolidateReportList = new ConsolidateReportList();
                        objConsolidateReportList.DealerCode = item.DealerCode;
                        objConsolidateReportList.UserID = item.UserID;
                        objConsolidateReportList.UsageTime = item.UsageTime;

                        DLRdeatilslist objDLRdeatilslist = clsBLLHttpUtility.Getdearlerdeatils(item.DealerCode, item.UserID);

                        objConsolidateReportList.Region = objDLRdeatilslist.REGION_CD;
                        objConsolidateReportList.DealerName = objDLRdeatilslist.DEALER_NAME;
                        objConsolidateReportList.ForCode = objDLRdeatilslist.FOR_CD;
                        objConsolidateReportList.Outletcode = objDLRdeatilslist.OUTLET_CD;
                        objConsolidateReportList.Location = objDLRdeatilslist.LOC_DESC;
                        objConsolidateReportList.Manuals = (from c in listFileUpload where c.ID == item.FID select new { c.TITLE }).Select(x => x.TITLE).SingleOrDefault();
                        objConsolidateReportList.Language = clsBLLHttpUtility.Getlanguagebyid((from c in listFileUpload
                                                                                               where c.ID == item.FID
                                                                                               select new { c.LID }).Select(x => x.LID).SingleOrDefault());
                        objConsolidateReportList.Model = clsBLLHttpUtility.Getmodelname((from c in listFileUpload where c.ID == item.FID select new { c.MID }).Select(x => x.MID).SingleOrDefault());


                        resultres.Add(objConsolidateReportList);
                    }






                    foreach (var dc in resultres)
                    {

                        i = i + 1;
                        sb.Append("<tr style='border:1px solid black; border-collapse:collapse;'>");
                        sb.Append("<td>" + dc.Region + "</td>");
                        sb.Append("<td>" + dc.DealerName + "</td>");
                        sb.Append("<td>" + dc.DealerCode + "</td>");
                        sb.Append("<td>" + dc.ForCode + "</td>");
                        sb.Append("<td>" + dc.Outletcode + "</td>");
                        sb.Append("<td>" + dc.Location + "</td>");
                        sb.Append("<td>" + dc.UserID+ "</td>");
                        sb.Append("<td>" +dc.Language+ "</td>");
                        sb.Append("<td>" + dc.Model + "</td>");
                        sb.Append("<td>" + dc.Manuals + "</td>");
                        sb.Append("<td>" + dc.UsageTime + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    byte[] bindata = System.Text.Encoding.ASCII.GetBytes(sb.ToString());

                    return File(bindata, "application/ms-excel", "DTSMP Usage Report" + DateTime.Now.Ticks + ".xls");
                }
                else
                {
                    return null;
                }
            }

        }





        [HttpPost]
        [ActionName("DownloadExcel")]
        public async Task<ActionResult> DownloadExcel(Reportrequest objreq)
        {
            UsageReportResponce resultdata = null;
            //Fill dataset with records
            // DataSet dataSet = GetRecordsFromDatabase();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);
                // Reportrequest objreq = new Reportrequest();
                //objreq.DealerCode = "645432";
                //objreq.DealerName = "Mukesh";
                objreq.DealerCode = objreq.DealerCode;
                objreq.DealerName = objreq.DealerName;
                objreq.Fromdate= !string.IsNullOrEmpty(Convert.ToString(objreq.Fromdate))? clsBLLHttpUtility.TryParseDateTime(objreq.Fromdate) : DateTime.Now.AddDays(-7);
                objreq.Todate= !string.IsNullOrEmpty(Convert.ToString(objreq.Todate)) ? clsBLLHttpUtility.TryParseDateTime(objreq.Todate) : DateTime.Now;

                var result = await client.PostAsJsonAsync("List/GetUsageReport", objreq);
                resultdata = await result.Content.ReadAsAsync<UsageReportResponce>();


                if (resultdata.result.UsageReportData.Count > 0 && resultdata.code=="200")
                {
                    DateTime dateTime = DateTime.UtcNow.Date;


                    StringBuilder sb = new StringBuilder();

                    sb.Append("<table  style='border:1px solid black; border-collapse:collapse;'>");

                    sb.Append("<tr>");
                    sb.Append("<td></td><td></td><td></td><td></td><td style='font-weight:bold'>Service Manual Usage Report</td><td></td><td></td><td></td><td></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr style='border:1px solid black; border-collapse:collapse;'>");
                    sb.Append("<td style='font-weight:bold'>Workshop Name:</td><td>" + resultdata.result.WORKSHOP_NAME + "</td><td></td><td style='font-weight:bold'>Workshop Code:</td><td> " + resultdata.result.WORKSHOP_CD + "</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='border:1px solid black; border-collapse:collapse;'>");
                    sb.Append("<td style='font-weight:bold'>Total Number of Users:</td><td>" + resultdata.result.TOTAL_NUMBER_USERS + "</td><td></td><td style='font-weight:bold'>Total number of Subscribes:</td><td>" + resultdata.result.TOTAL_NUMBER_SUBSCRIBES + "</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr style='border:1px solid black; border-collapse:collapse;'>");
                    sb.Append("<td style='font-weight:bold'>Report Generated Date:</td><td>" + dateTime.ToString("d") + "</td><td></td><td></td><td></td>");
                    sb.Append("</tr>");


                    string[] columnName = { "S.No.", "Obsolete/Running", "Language", "Models", "Manuals", "No of times used", "Total time used(Hours)", "Date", "UsedBy" };

                    //LINQ to get Column names
                    //var columnName = dataSet.Tables[0].Columns.Cast<DataColumn>()
                    //                     .Select(x => x.ColumnName)
                    //                     .ToArray();
                    sb.Append("<tr style='border:1px solid black; border-collapse:collapse;'>");
                    //Looping through the column names
                    foreach (var col in columnName)
                        sb.Append("<td style='font-weight:bold'>" + col + "</td>");
                    sb.Append("</tr>");

                    //Looping through the records
                    //for (int i = 0; i < resultdata.result.UsageReportData.Count; i++)

                   



        //{
        int i = 0;


                    foreach (var dc in resultdata.result.UsageReportData)
                    {

                        i = i + 1;
                        sb.Append("<tr style='border:1px solid black; border-collapse:collapse;'>");
                        sb.Append("<td>" + i + "</td>");
                        sb.Append("<td>" + dc.ROSTATUS + "</td>");
                        sb.Append("<td>" + dc.Language + "</td>");
                        sb.Append("<td>" + dc.Models + "</td>");
                        sb.Append("<td>" + dc.MANUALS + "</td>");
                        sb.Append("<td>" + dc.NO_OF_TIMES_USED + "</td>");
                        sb.Append("<td>" + dc.TOTAL_TIME_USED + "</td>");
                        sb.Append("<td>" + Convert.ToDateTime(dc.DATE).ToString("dd/MM/yyyy h:mm tt") + "</td>");
                        sb.Append("<td>" + dc.USEDBY + "</td>");
                        sb.Append("</tr>");
                    }


                    //}

                    //foreach (var dr in resultdata.result.)
                    //{
                    //sb.Append("<tr>");
                    //foreach (DataColumn dc in dataSet.Tables[0].Columns)
                    //{
                    //    sb.Append("<td>" + dr[dc] + "</td>");
                    //}
                    //sb.Append("</tr>");
                    // }

                    sb.Append("</table>");

                    //Writing StringBuilder content to an excel file.
                    //Response.Clear();
                    //Response.ClearContent();
                    //Response.ClearHeaders();
                    //Response.Charset = "";
                    //Response.Buffer = true;
                    //Response.ContentType = "application/ms-excel";
                    //Response.AddHeader("content-disposition", $"attachment;filename=Service Manual Usage Report" + DateTime.Now.Ticks + ".xls");
                    //Response.Output.Write(sb.ToString());
                    ////Response.Write(sb.ToString());
                    //Response.Flush();
                    //Response.Close();

                    byte[] bindata = System.Text.Encoding.ASCII.GetBytes(sb.ToString());

                    return File(bindata, "application/ms-excel", "Service Manual Usage Report" + DateTime.Now.Ticks + ".xls");
                }
                else
                {
                    return null;
                }
            }
           // return File(bindata, "application/ms-excel", "ReportFile.xls");
           // return null;

        }



        //Login 
        [HttpGet]
        public ActionResult ModelList(string id)
        {

            ViewBag.Lid = !string.IsNullOrEmpty(id) ? id : "03";

            ViewBag.language = clsBLLHttpUtility.Getlanguagebyid(ViewBag.Lid);

            //IEnumerable<Model> models = null;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetModeldashbaord(string mstatus,string lid, string search)
        {
            //var users = GetUsers();
            //return Json(users, JsonRequestBehavior.AllowGet);

            if (!string.IsNullOrEmpty(mstatus))
            {
                ModelMainResponce models = null;

                IEnumerable<Modaldetails> _Objmodellist = null;
                using (var client = new HttpClient())
                {

                    ModelCount request = new ModelCount();
                    request.LID = !string.IsNullOrEmpty(lid)? lid:"02";
                    request.MSTATUS = !string.IsNullOrEmpty(mstatus)? mstatus:"Y";
                    client.BaseAddress = new Uri(apiBaseAddress);
                    var modellist = await client.PostAsJsonAsync("List/GetDataCountbyMidDataa", request);
                    if (modellist.IsSuccessStatusCode)
                    {
                        models = await modellist.Content.ReadAsAsync<ModelMainResponce>();
                        if (!String.IsNullOrEmpty(search))
                        {
                            _Objmodellist = models.result.Where(stu => stu.SMAS_MODEL_DESC.ToUpper().Contains(search.ToUpper()));

                        }
                        else
                        {
                            _Objmodellist = models.result;
                        }
                    }
                    return Json(_Objmodellist, JsonRequestBehavior.AllowGet);
                    //return Json(new SelectList(models.Where(s => s.MSTATUS == status), "MID", "MODEL"));
                }

            }

            return null;


        }



        [HttpGet]
		public async Task<ActionResult> FileUpload()
		{
			IEnumerable<Model> models = null;
			IEnumerable<ServiceManuals> stype = null;
			IEnumerable<FileVersions> fversion = null;
			IEnumerable<Languages> language = null;

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(apiBaseAddress);

				var modellist = await client.GetAsync("List/GetModel");
				models = await modellist.Content.ReadAsAsync<IList<Model>>();
				ViewBag.Modellist = new SelectList(models, "MID", "MODEL");

				var servicemanuals = await client.GetAsync("List/GetServiceManuals");
				stype = await servicemanuals.Content.ReadAsAsync<IList<ServiceManuals>>();
				ViewBag.Servicemanuals = new SelectList(stype, "SID", "STYPE");

				var fversionlist = await client.GetAsync("List/GetFileVersions");
				fversion = await fversionlist.Content.ReadAsAsync<IList<FileVersions>>();
				ViewBag.Fversionlist = new SelectList(fversion, "FID", "FVERSION");

				var languagelist = await client.GetAsync("List/GetLanguages");
				language = await languagelist.Content.ReadAsAsync<IList<Languages>>();
				ViewBag.Languagelist = new SelectList(language, "LID", "LANGUAGE");
			}

			
			    return View();
		}




        [HttpPost]
        public async Task<ActionResult> FileUpload(FileUpload filedata, HttpPostedFileBase file_Uploader)
        {
            if (ModelState.IsValid)
            {

                IEnumerable<Model> models = null;
                IEnumerable<ServiceManuals> stype = null;
                IEnumerable<FileVersions> fversion = null;
                IEnumerable<Languages> language = null;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseAddress);

                    var modellist = await client.GetAsync("List/GetModel");
                    models = await modellist.Content.ReadAsAsync<IList<Model>>();
                    ViewBag.Modellist = new SelectList(models, "MID", "MODEL");

                    var servicemanuals = await client.GetAsync("List/GetServiceManuals");
                    stype = await servicemanuals.Content.ReadAsAsync<IList<ServiceManuals>>();
                    ViewBag.Servicemanuals = new SelectList(stype, "SID", "STYPE");

                    var fversionlist = await client.GetAsync("List/GetFileVersions");
                    fversion = await fversionlist.Content.ReadAsAsync<IList<FileVersions>>();
                    ViewBag.Fversionlist = new SelectList(fversion, "FID", "FVERSION");

                    var languagelist = await client.GetAsync("List/GetLanguages");
                    language = await languagelist.Content.ReadAsAsync<IList<Languages>>();
                    ViewBag.Languagelist = new SelectList(language, "LID", "LANGUAGE");
                }


                if (file_Uploader != null)
                {

                    string fileName = string.Empty;
                    string destinationPath = string.Empty;
                    fileName = Path.GetFileName(file_Uploader.FileName);

                    destinationPath = Path.Combine(Server.MapPath("~/MyFiles/"), fileName);

                    file_Uploader.SaveAs(destinationPath);

                    filedata.FILEPATH = destinationPath;
                    filedata.CREATEDATE = DateTime.UtcNow;


                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiBaseAddress);

                        var response = await client.PostAsJsonAsync("Admin/AddFile", filedata);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index", "home");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        }
                    }

                    if (Session["fileUploader"] != null)

                    {
                        ViewBag.Message = "File Uploaded Successfully";

                    }

                    else

                    {

                        ViewBag.Message = "File is already exists";

                    }

                }

                else

                {
                    ViewBag.Message = "File Uploaded Successfully";

                }


            }
            return View();

        }
        public async Task<ActionResult> Modeldetails(string id, string sortOrder, string Filter_Value, int? Page_No, string Search_Data, string langugeid, string status)
        {
            ResponceServiceManuals stype = null;
            IEnumerable<FileVersions> fversion = null;
            ViewBag.languageid = !string.IsNullOrEmpty(langugeid) ? langugeid : "";
            ViewBag.language = clsBLLHttpUtility.Getlanguagebyid(ViewBag.languageid);
            string modelcode=  string.IsNullOrEmpty(id) ? "01" : id;
            string modelname = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                string json = "{\"PN_PMC\":\"1\",\"PN_SVAR_TYPE\":\"Y\"}";
                var response = WebHTTP.CallForDMSServicesEnquiries("SMAS_SVAR", json);
                JavaScriptSerializer js = new JavaScriptSerializer();
                ResponseDataList<Modaldetails> objresults = js.Deserialize<ResponseDataList<Modaldetails>>(response.ToString());

                if (objresults.code == "200")
                {
                    ViewBag.Modellist = new SelectList(objresults.Result, "SMAS_MODEL_CD", "SMAS_MODEL_DESC");

                    modelname = objresults.Result.Where(x => x.SMAS_MODEL_CD == modelcode).Select(s => s.SMAS_MODEL_DESC).FirstOrDefault();
                    if (string.IsNullOrEmpty(modelname))
                    {

                        string jsonobs = "{\"PN_PMC\":\"1\",\"PN_SVAR_TYPE\":\"N\"}";
                        var responseobs = WebHTTP.CallForDMSServicesEnquiries("SMAS_SVAR", jsonobs);
                        JavaScriptSerializer jsobs = new JavaScriptSerializer();
                        ResponseDataList<Modaldetails> objresultsobs = jsobs.Deserialize<ResponseDataList<Modaldetails>>(responseobs.ToString());

                        if (objresultsobs.code == "200")
                        {
                            ViewBag.Modelname = objresultsobs.Result.Where(x => x.SMAS_MODEL_CD == modelcode).Select(s => s.SMAS_MODEL_DESC).FirstOrDefault();
                        }
                       
                    }
                    else
                    {
                        ViewBag.Modelname = modelname;

                    }
                   

                }
                else
                {
                    ViewBag.Modellist = null;
                    ViewBag.Modelname = modelname;
                }

                string jsonlanguages = "{\"PN_PMC\":\"1\"}";
                var responselanguages = WebHTTP.CallForDMSServicesEnquiries("SMAS_LANGUAGE", jsonlanguages);
                JavaScriptSerializer jslang = new JavaScriptSerializer();
                ResponseDataList<DMSLanguages> objlangresults = jslang.Deserialize<ResponseDataList<DMSLanguages>>(responselanguages.ToString());

                if (objlangresults.code == "200")
                {
                    ViewBag.Languagelist = new SelectList(objlangresults.Result, "LANGUAGE_CODE", "LANGUAGE_DESC");
                }
                else
                {
                    ViewBag.Languagelists = null;
                }


                var servicemanuals = await client.GetAsync("List/GetServiceManuals");
                stype = await servicemanuals.Content.ReadAsAsync<ResponceServiceManuals>();
                if (stype.code == "200")
                {
                    ViewBag.Servicemanuals = new SelectList(stype.result, "SID", "STYPE");
                }
                else
                {
                    ViewBag.Servicemanuals = null;
                }


                var fversionlist = await client.GetAsync("List/GetFileVersions");
                fversion = await fversionlist.Content.ReadAsAsync<IList<FileVersions>>();
                ViewBag.Fversionlist = new SelectList(fversion, "FID", "FVERSION");
              
            }

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (!string.IsNullOrEmpty(Search_Data))
            {
                Page_No = 1;
            }
            //else
            //{
            //    Search_Data = Filter_Value;
            //}
            string statusstr = string.IsNullOrEmpty(status) ? "Y" : status;
            ViewBag.Search = Search_Data;
            ViewBag.Filter = string.IsNullOrEmpty(Filter_Value) ? 1 : Convert.ToInt16(Filter_Value); 
            ViewBag.langugeid = langugeid;
            ViewBag.status = statusstr;
            FileResponce Modaldataresponse = null;
            IEnumerable<FileUpload> _objfileuploadlist = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);
                Request objreq = new Request();
                objreq.MID = modelcode;
                objreq.LID = string.IsNullOrEmpty(langugeid) ? "03" : langugeid;
                objreq.SID = string.IsNullOrEmpty(Filter_Value)?1:Convert.ToInt16(Filter_Value);
                objreq.ROSTATUS = statusstr;
                var result = await client.PostAsJsonAsync("List/GetDatabyMId", objreq);
                if (result.IsSuccessStatusCode)
                {
                    Modaldataresponse = await result.Content.ReadAsAsync<FileResponce>();
                    if (!String.IsNullOrEmpty(Search_Data))
                    {
                        _objfileuploadlist = Modaldataresponse.result.Where(stu => stu.TITLE.ToUpper().Contains(Search_Data.ToUpper())
                                    || stu.TITLE.ToUpper().Contains(Search_Data.ToUpper()) && stu.ISACTIVE==false );
                    }
                    else
                    {
                        _objfileuploadlist = Modaldataresponse.result.Where(stu => stu.ISACTIVE == false);
                    }
                    switch (sortOrder)
                    {
                        case "name_desc":
                            _objfileuploadlist = _objfileuploadlist.OrderByDescending(s => s.ID);
                            break;
                        default:
                            _objfileuploadlist = _objfileuploadlist.OrderBy(s => s.ID);
                            break;
                    }

                }
                else
                {
                    _objfileuploadlist = Enumerable.Empty<FileUpload>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            int Size_Of_Page = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
            int No_Of_Page = (Page_No ?? 1);
            return View(_objfileuploadlist.ToPagedList(No_Of_Page, Size_Of_Page));
        }


        public async Task<ActionResult> Viewpdf(int id)
        {
            FileResponce resultdata = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);
                FileUpload objreq = new FileUpload();
                objreq.ID = id;
                var result = await client.PostAsJsonAsync("List/GetDataFilePath", objreq);
                resultdata = await result.Content.ReadAsAsync<FileResponce>();
                if(resultdata.code=="200")
                {
                    ViewBag.filepath = resultdata.result.Select(f => f.FILEPATH).FirstOrDefault();
                    ViewBag.pdfname = resultdata.result.Select(f => f.TITLE).FirstOrDefault();
                }
                else
                {
                    ViewBag.filepath = "";
                    ViewBag.pdfname = "";
                }
                
            }
                return View();
        }
        public async Task<ActionResult> ViewVideo(int id)
        {
            FileResponce resultdata = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);
                FileUpload objreq = new FileUpload();
                objreq.ID = id;
                var result = await client.PostAsJsonAsync("List/GetDataFilePath", objreq);
                resultdata = await result.Content.ReadAsAsync<FileResponce>();
                if (resultdata.code == "200")
                {
                    ViewBag.videopath = resultdata.result.Select(f => f.FILEPATH).FirstOrDefault();
                }
                else
                {
                    ViewBag.videopath = "";
                }

            }
            return View();
        }

        //Login POST


        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }




    }
}
