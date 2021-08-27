using SMAS.Models;
using SMAS.Models.List;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SMAS.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {

        readonly string apiBaseAddress = ConfigurationManager.AppSettings["apiBaseAddress"];
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> uploadfiledata(FileUploadModel filedatamodel, HttpPostedFileBase PostedFile)
        {
            string responsecode = string.Empty;

            if (clsBLLHttpUtility.CheckForInternetConnection())
            {

               

                if (PostedFile != null)
                {

                    FileUpload filedata = new FileUpload();

                    string modelcode = string.IsNullOrEmpty(filedatamodel.MID) ? "01" : filedatamodel.MID;
                    string modelname = string.Empty;

                    string languageCode = string.IsNullOrEmpty(filedatamodel.LID) ? "01" : filedatamodel.LID;
                    string language = string.Empty;

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
                                    modelname = objresultsobs.Result.Where(x => x.SMAS_MODEL_CD == modelcode).Select(s => s.SMAS_MODEL_DESC).FirstOrDefault();
                                }

                            }



                        }


                        string jsonlanguages = "{\"PN_PMC\":\"1\"}";
                        var responselanguages = WebHTTP.CallForDMSServicesEnquiries("SMAS_LANGUAGE", jsonlanguages);
                        JavaScriptSerializer jslang = new JavaScriptSerializer();
                        ResponseDataList<DMSLanguages> objlangresults = jslang.Deserialize<ResponseDataList<DMSLanguages>>(responselanguages.ToString());

                        if (objlangresults.code == "200")
                        {

                            language = objlangresults.Result.Where(l => l.LANGUAGE_CODE == languageCode).Select(x => x.LANGUAGE_DESC).FirstOrDefault();

                        }

                    }






                    if (PostedFile != null)
                    {

                        string fileName = string.Empty;
                        string destinationPath = string.Empty;
                        string fpath = string.Empty;


                        if (string.IsNullOrEmpty(filedatamodel.FID))
                        {
                            

                            fileName = Path.GetFileName(PostedFile.FileName);
                           //fileName = clsBLLHttpUtility.GetValidFileName(fileName);
                            //fileName.Replace(" ", String.Empty);
                        }
                        else
                        {
                            fileName = string.Format("{0}-{1}{2}"
                                       , Path.GetFileNameWithoutExtension(PostedFile.FileName)
                                       , filedatamodel.FID
                                       , Path.GetExtension(PostedFile.FileName));
                            //fileName = clsBLLHttpUtility.GetValidFileName(fileName);
                            //fileName.Replace(" ", String.Empty);



                        }
                        string imagepath = string.Empty;
                        if (filedatamodel.SID == 1)
                        {
                            destinationPath = Path.Combine(Server.MapPath("~/MyFiles/pdf/" + language + "/" + modelname), fileName);
                            imagepath = clsBLLHttpUtility.GetDomin() + "MyFiles/pdf/" + language + "/" + modelname + "/" + fileName;
                            fpath = Path.Combine(Server.MapPath("~/MyFiles/pdf/" + language + "/" + modelname));
                        }
                        else if (filedatamodel.SID == 2)
                        {
                            destinationPath = Path.Combine(Server.MapPath("~/MyFiles/video/" + language + "/" + modelname), fileName);
                            imagepath = clsBLLHttpUtility.GetDomin() + "MyFiles/video/" + language + "/" + modelname + "/" + fileName;
                            fpath = Path.Combine(Server.MapPath("~/MyFiles/video/" + language + "/" + modelname));
                        }

                        if (!Directory.Exists(fpath))
                        {
                            Directory.CreateDirectory(fpath);
                        }

                        PostedFile.SaveAs(destinationPath);
                        filedata.FID = !string.IsNullOrEmpty(filedatamodel.FID) ? filedatamodel.FID : " ";
                        filedata.TITLE = fileName;
                        filedata.ISPUBLISH = "N";
                        filedata.FILEPATH = imagepath;
                        filedata.CREATEDATE = DateTime.UtcNow;
                        filedata.MODEFIEDDATE = DateTime.UtcNow;
                        filedata.ROSTATUS = filedatamodel.ROSTATUS;
                        filedata.MID = filedatamodel.MID;
                        filedata.SID = filedatamodel.SID;
                        filedata.LID = filedatamodel.LID;
                        filedata.PSTATUS = filedatamodel.PSTATUS;
                        filedata.ISPUBLISH = "N";


                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(apiBaseAddress);

                            var response = await client.PostAsJsonAsync("Admin/AddFile", filedata);
                            if (response.IsSuccessStatusCode)
                            {
                                responsecode = "200";
                                return RedirectToAction("Index", "Admin", new { id = responsecode });
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Server error try after some time.");
                                responsecode = "202";
                            }
                        }

                        //if (Session["fileUploader"] != null)

                        //{
                        //    ViewBag.Message = "File Uploaded Successfully";
                        //    responsecode = "200";


                        //}

                        //else

                        //{

                        //    ViewBag.Message = "File is already exists";
                        //    responsecode = "204";


                        //}

                    }

                    else

                    {
                        ViewBag.Message = "File Uploaded Successfully";
                        responsecode = "200";


                    }


                }

                else
                {
                    ViewBag.Message = "File not uploaded due to some error";
                    responsecode = "201";


                }
            }
            else
            {
                ViewBag.Message = "File not uploaded due to lost internet connection.";
                responsecode = "204";


            }
            return RedirectToAction("Index", "Admin",new {id = responsecode });
            //}
            //else
            //{
            //    // Settings.  
            //    ViewBag.Message = "'" + filedatamodel.PostedFile.FileName + "' file size exceeds maximum limit. ";

            //    //return PartialView("_fileupload", filedatamodel);

            //    return RedirectToAction("Index", "Admin");
            //    //  return View(filedatamodel);
            //}
            // return View();

        }




    }
}