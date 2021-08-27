using PagedList;
using SMAS.Models;
using SMAS.Models.List;
using SMAS.Models.UserLogin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SMAS.Controllers
{
    public class HomeController : Controller
    {
        readonly string apiBaseAddress = ConfigurationManager.AppSettings["apiBaseAddress"];
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult sendotp(Requestsendotp req)
        {
            string json = "{\"PN_MSPIN\":\"" + req.PN_MSPIN + "\",\"PN_OTP\":\"" + req.PN_MSPIN + "\",\"PN_OS_TYPE\":\"3\",\"PN_PMC\":\"1\"}";
            var response = WebHTTP.CallForDMSServicesEnquiries("Send_OTP", json);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Responsesendotp objresult = new Responsesendotp();
            try
            {
                objresult = js.Deserialize<Responsesendotp>(response.ToString());
            }
            catch (Exception)
            {

                ViewBag.Message = "202";
            }
            if (objresult.message == "SUCCESS" && objresult.code == "200")
            {
                ViewBag.Message = objresult.result.PN_OTP;
            }

            return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
        }



        //  [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Index(Userlogin login, string ReturnUrl = "")
        {
            string message = "";

            //string ss = Crypto.Hash(login.Password);

            string json = "{\"PN_USER_ID\":\""+login.EmailID+"\",\"PN_PWD\":\""+login.Password+ "\",\"PN_USER_TYPE\":\"" + login.UserType + "\"}";
            string usedbyid = string.Empty;
            if(!string.IsNullOrEmpty(login.EmailID))
            {
                usedbyid = login.EmailID;
            }
           
            var response = WebHTTP.CallForDMSServicesEnquiries("SMASUSERLOGIN", json);
            JavaScriptSerializer js = new JavaScriptSerializer();
            Response<LoginResult> objresult = new Response<LoginResult>();
            try
            {
                 objresult = js.Deserialize<Response<LoginResult>>(response.ToString());
            }
            catch (Exception)
            {

                objresult.code = "202";
            }

            if (objresult.message == "SUCCESS" && objresult.code == "200")
            {
                if(objresult.Result.PO_EMP_CD.Contains("$"))
                {
                    login.EmailID = objresult.Result.PO_EMP_CD.Split('$')[1];
                }
                else
                {
                    login.EmailID = objresult.Result.PO_EMP_CD;
                }
              
                int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                string encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                System.Web.Security.FormsAuthentication.SetAuthCookie(login.EmailID, false);

                Session["DealerCode"] = objresult.Result.PO_DEALER_CODE;
                if(!string.IsNullOrEmpty(usedbyid))
                {
                    Session["Usedby"] = usedbyid;
                }
                Session.Timeout = 36000;


                //System.Web.Security.FormsAuthentication.SetAuthCookie(objresult.Result.PO_DEALER_CODE, false);
                cookie.Expires = DateTime.Now.AddMinutes(timeout);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl != "/Home/Logout")
                {
                    ViewBag.Message = ReturnUrl;
                   // return Redirect(ReturnUrl);
                }
                else
                {
                    ViewBag.Message = "Dealer/Index";
                    //return RedirectToAction("Index", "Dealer");
                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseAddress);

                    var result = await client.PostAsJsonAsync("Admin/CheckLogin", login);
               if(result.IsSuccessStatusCode)
                    {

                        var resultdata = await result.Content.ReadAsAsync<Userlogin>();

                        if (resultdata != null && string.Compare(Crypto.Hash(login.Password), resultdata.Password) == 0)
                        {

                            int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                            var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            System.Web.Security.FormsAuthentication.SetAuthCookie(login.EmailID, false);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);


                            if (Url.IsLocalUrl(ReturnUrl)&& ReturnUrl!= "/Home/Logout")
                            {
                                //return Redirect(ReturnUrl);
                                ViewBag.Message = ReturnUrl;
                            }
                            else
                            {
                                ViewBag.Message = "Admin/Index";
                               // return RedirectToAction("Index", "Admin");
                            }
                        }
                        else
                        {
                            // message = "Invalid credential provided";
                            ViewBag.Message = "Invalid credential provided";
                        }



                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
           // ViewBag.Message = message;

            return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
            //return View();
        }


        //Logout
        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
        }

        [HttpGet]
        public async Task<ActionResult> Modeldetails(string id,string sortOrder, string Filter_Value, int? Page_No, string Search_Data,string langugeid )
        {

            IEnumerable<Model> models = null;
            ResponceServiceManuals stype = null;
            IEnumerable<FileVersions> fversion = null;
            IEnumerable<Languages> language = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                var modellist = await client.GetAsync("List/GetModel");
                models = await modellist.Content.ReadAsAsync<IList<Model>>();
                ViewBag.Modellist = new SelectList(models.Where(s => s.MSTATUS == "running"), "MID", "MODEL");
                ViewBag.Modellistobsultue = new SelectList(models.Where(s => s.MSTATUS == "obsolete"), "MID", "MODEL");

                var servicemanuals = await client.GetAsync("List/GetServiceManuals");
                stype = await servicemanuals.Content.ReadAsAsync<ResponceServiceManuals>();
                if(stype.code=="200")
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

                var languagelist = await client.GetAsync("List/GetLanguages");
                language = await languagelist.Content.ReadAsAsync<IList<Languages>>();
                ViewBag.Languagelist = new SelectList(language, "LID", "LANGUAGE");
            }

            var rostatus = new List<SelectListItem> { new SelectListItem { Text = "Running", Value = "R",Selected = true },
                new SelectListItem { Text = "Obsolete", Value = "O" }, };
            ViewData["rostatus"] = rostatus;

            var list = new List<SelectListItem> { new SelectListItem { Text = "Option 1", Value = "1" },
                new SelectListItem { Text = "Option 2", Value = "2" },
                new SelectListItem { Text = "Option 3", Value = "3", Selected = true }, };
            ViewData["foorBarList"] = list;




            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;
            ViewBag.langugeid = langugeid;
            FileResponce employees = null;
            IEnumerable<FileUpload> employeesdata = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);
                Request objreq = new Request();
                objreq.MID = string.IsNullOrEmpty(id) ? "01" : id;
                objreq.LID = string.IsNullOrEmpty(langugeid)? "03" : langugeid;
                var result = await client.PostAsJsonAsync("List/GetDatabyMId", objreq);
                if (result.IsSuccessStatusCode)
                {
                    employees = await result.Content.ReadAsAsync<FileResponce>();
                    if (!String.IsNullOrEmpty(Search_Data))
                    {
                        employeesdata = employees.result.Where(stu => stu.ROSTATUS.ToUpper().Contains(Search_Data.ToUpper())
                                    || stu.ROSTATUS.ToUpper().Contains(Search_Data.ToUpper()));
                    }
                    switch (sortOrder)
                    {
                        case "name_desc":
                            employeesdata = employees.result.OrderByDescending(s => s.ID);
                            break;
                        default:
                            employeesdata = employees.result.OrderBy(s => s.ID);
                            break;
                    }


                }
                else
                {
                    employeesdata = Enumerable.Empty<FileUpload>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            int Size_Of_Page = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
            int No_Of_Page = (Page_No ?? 1);
            return View(employeesdata.ToPagedList(No_Of_Page, Size_Of_Page));




            //return View();
        }

        

        [HttpPost]
        public async Task<JsonResult> GetModeldashbaord(string mstatus,string search)
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
                    request.MSTATUS = mstatus;
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



        [HttpPost]
        public async Task<ActionResult> Modeldetails(FileUpload filedata, HttpPostedFileBase file_Uploader)
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
                    ViewBag.Modellist = new SelectList(models.Where(s => s.MSTATUS == "running"), "MID", "MODEL");

                    ViewBag.Modellistobsultue = new SelectList(models.Where(s => s.MSTATUS == "obsolete"), "MID", "MODEL");

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

                var rostatus = new List<SelectListItem> { new SelectListItem { Text = "Running", Value = "R",Selected = true },
                new SelectListItem { Text = "Obsolete", Value = "O" }, };
                ViewData["rostatus"] = rostatus;

                var list = new List<SelectListItem> { new SelectListItem { Text = "Option 1", Value = "1" },
                new SelectListItem { Text = "Option 2", Value = "2" },
                new SelectListItem { Text = "Option 3", Value = "3", Selected = true }, };
                ViewData["foorBarList"] = list;

                if (file_Uploader != null)
                {

                    string fileName = string.Empty;
                    string destinationPath = string.Empty;
                   

                    if(string.IsNullOrEmpty(filedata.FID))
                    {
                        fileName = Path.GetFileName(file_Uploader.FileName);
                    }
                    else
                    {
                        fileName = string.Format("{0}-{1}{2}"
                                   , Path.GetFileNameWithoutExtension(file_Uploader.FileName)
                                   , filedata.FID
                                   , Path.GetExtension(file_Uploader.FileName));

                    }

                    if(filedata.SID==1)
                    {
                        destinationPath = Path.Combine(Server.MapPath("~/MyFiles/pdf"), fileName);
                    }
                    else if(filedata.SID == 2)
                    {
                        destinationPath = Path.Combine(Server.MapPath("~/MyFiles/video"), fileName);
                    }

                    


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



        public ActionResult Viewpdf()
        {
            return View();
        }
        public ActionResult ViewVideo()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Sarathlal Saseendran";

            return View();
        }
    }
}