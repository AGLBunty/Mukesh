using PagedList;
using SMAS.Models;
using SMAS.Models.List;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SMAS.Controllers.Dealer
{
    [Authorize]
    public class DealerController : Controller
    {

        readonly string apiBaseAddress = ConfigurationManager.AppSettings["apiBaseAddress"];

        // GET: Dealer
        [HttpPost]
        public JsonResult modellist(string status)
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
        public async Task<JsonResult> GetModeldashbaord(string mstatus, string lid, string search)
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
                    request.LID = !string.IsNullOrEmpty(lid) ? lid : "02";
                    request.MSTATUS = !string.IsNullOrEmpty(mstatus) ? mstatus : "Y";
                    client.BaseAddress = new Uri(apiBaseAddress);
                    var modellist = await client.PostAsJsonAsync("List/GetDealermodellist", request);
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


        public async Task<ActionResult> Modeldetails(string id, string sortOrder, string Filter_Value, int? Page_No, string Search_Data, string langugeid,string status)
        {
            ResponceServiceManuals stype = null;
            IEnumerable<FileVersions> fversion = null;
            ViewBag.languageid = !string.IsNullOrEmpty(langugeid) ? langugeid : "03";
            ViewBag.language = clsBLLHttpUtility.Getlanguagebyid(ViewBag.languageid);
            string modelcode = string.IsNullOrEmpty(id) ? "01" : id;
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
            DealerFileResponce Modaldataresponse = null;
            IEnumerable<SubscribeModel> _objfileuploadlist = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);
                Request objreq = new Request();
                objreq.MID = modelcode;
                objreq.LID = string.IsNullOrEmpty(langugeid) ? "03" : langugeid;
                objreq.SID = string.IsNullOrEmpty(Filter_Value) ? 1 : Convert.ToInt16(Filter_Value);
                objreq.DEALERCODE = Convert.ToString(Session["DealerCode"]);
                objreq.ROSTATUS = statusstr;
                var result = await client.PostAsJsonAsync("List/GetDatabydealercode", objreq);
                if (result.IsSuccessStatusCode)
                {
                    Modaldataresponse = await result.Content.ReadAsAsync<DealerFileResponce>();
                    if (!String.IsNullOrEmpty(Search_Data))
                    {
                        _objfileuploadlist = Modaldataresponse.result.Where(stu => stu.TITLE.ToUpper().Contains(Search_Data.ToUpper())
                                    || stu.TITLE.ToUpper().Contains(Search_Data.ToUpper()));
                    }
                    else
                    {
                        _objfileuploadlist = Modaldataresponse.result;
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
                    _objfileuploadlist = Enumerable.Empty<SubscribeModel>();
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
                ViewBag.id = id;
                var result = await client.PostAsJsonAsync("List/GetDataFilePath", objreq);
                resultdata = await result.Content.ReadAsAsync<FileResponce>();
                if (resultdata.code == "200")
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

        public async Task<ActionResult> Viewfile(int id)
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


        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Feedback([Bind(Include = "DEALERCODE,FEEDBACK,MID,LID,SID,FEEDBACKMODE,USERID")] Feedbacks feedbacks)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseAddress);
                    feedbacks.DEALERCODE = Convert.ToString(Session["DealerCode"]);
                    feedbacks.FEEDBACKMODE = "WEB";
                    var response = await client.PostAsJsonAsync("List/FeedbackSaveData", feedbacks);
                    if (response.IsSuccessStatusCode)
                    {
                        // return RedirectToAction("Index","dealer");

                        ViewBag.message = "200";
                        feedbacks.FEEDBACK = "";
                    }
                    else
                    {
                        ViewBag.message = "202";
                        // ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View(feedbacks);
        }


    }
}