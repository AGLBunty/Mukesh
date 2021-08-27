using SMAS.Models.List;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace SMAS.Models
{
    public static class clsBLLHttpUtility
    {

        public static string Getlanguagebyid(string code)
        {
            string laugname = string.Empty;
            string jsonlanguages = "{\"PN_PMC\":\"1\"}";
            var responselanguages = WebHTTP.CallForDMSServicesEnquiries("SMAS_LANGUAGE", jsonlanguages);
            JavaScriptSerializer jslang = new JavaScriptSerializer();
            ResponseDataList<DMSLanguages> objlangresults = jslang.Deserialize<ResponseDataList<DMSLanguages>>(responselanguages.ToString());

            if (objlangresults.code == "200")
            {
                laugname = objlangresults.Result.Where(l => l.LANGUAGE_CODE == code).Select(x => x.LANGUAGE_DESC).FirstOrDefault();
            }
           

            return laugname;

        }


        public static string GetdearlerdeatilsRegion(string dealercode,string usedby)
        {
            //List<DLRdeatilslist>  _objdlrlist = new List<DLRdeatilslist>()

            string region = string.Empty;
            string json = "{\"PN_PMC\":\"1\",\"PN_DEALER_CD\":\""+ dealercode + "\",\"PN_USER_ID\":\""+ usedby + "\"}";
            var response = WebHTTP.CallForDMSServicesEnquiries("GET_DLR_USAGE_DTL", json);
            JavaScriptSerializer jsdlr = new JavaScriptSerializer();
            DLRdeatils objDLRresults = jsdlr.Deserialize<DLRdeatils>(response.ToString());

            if (objDLRresults.code == "200")
            {
                region = objDLRresults.result.Select(x => x.REGION_CD).FirstOrDefault();
               // laugname = objlangresults.Result.Where(l => l.LANGUAGE_CODE == code).Select(x => x.LANGUAGE_DESC).FirstOrDefault();
            }
            return region;

        }


        public static string Getdearlerdeatilsdealername(string dealercode, string usedby)
        {
            //List<DLRdeatilslist>  _objdlrlist = new List<DLRdeatilslist>()

            string region = string.Empty;
            string json = "{\"PN_PMC\":\"1\",\"PN_DEALER_CD\":\"" + dealercode + "\",\"PN_USER_ID\":\"" + usedby + "\"}";
            var response = WebHTTP.CallForDMSServicesEnquiries("GET_DLR_USAGE_DTL", json);
            JavaScriptSerializer jsdlr = new JavaScriptSerializer();
            DLRdeatils objDLRresults = jsdlr.Deserialize<DLRdeatils>(response.ToString());

            if (objDLRresults.code == "200")
            {
                region = objDLRresults.result.Select(x => x.DEALER_NAME).FirstOrDefault();
                // laugname = objlangresults.Result.Where(l => l.LANGUAGE_CODE == code).Select(x => x.LANGUAGE_DESC).FirstOrDefault();
            }
            return region;

        }

        public static string Getdearlerdeatilsforcode(string dealercode, string usedby)
        {
            //List<DLRdeatilslist>  _objdlrlist = new List<DLRdeatilslist>()

            string region = string.Empty;
            string json = "{\"PN_PMC\":\"1\",\"PN_DEALER_CD\":\"" + dealercode + "\",\"PN_USER_ID\":\"" + usedby + "\"}";
            var response = WebHTTP.CallForDMSServicesEnquiries("GET_DLR_USAGE_DTL", json);
            JavaScriptSerializer jsdlr = new JavaScriptSerializer();
            DLRdeatils objDLRresults = jsdlr.Deserialize<DLRdeatils>(response.ToString());

            if (objDLRresults.code == "200")
            {
                region = objDLRresults.result.Select(x => x.FOR_CD).FirstOrDefault();
                // laugname = objlangresults.Result.Where(l => l.LANGUAGE_CODE == code).Select(x => x.LANGUAGE_DESC).FirstOrDefault();
            }
            return region;

        }


        public static string Getdearlerdeatilsoutcode(string dealercode, string usedby)
        {

            string region = string.Empty;
            string json = "{\"PN_PMC\":\"1\",\"PN_DEALER_CD\":\"" + dealercode + "\",\"PN_USER_ID\":\"" + usedby + "\"}";
            var response = WebHTTP.CallForDMSServicesEnquiries("GET_DLR_USAGE_DTL", json);
            JavaScriptSerializer jsdlr = new JavaScriptSerializer();
            DLRdeatils objDLRresults = jsdlr.Deserialize<DLRdeatils>(response.ToString());

            if (objDLRresults.code == "200")
            {
                region = objDLRresults.result.Select(x => x.OUTLET_CD).FirstOrDefault();
                // laugname = objlangresults.Result.Where(l => l.LANGUAGE_CODE == code).Select(x => x.LANGUAGE_DESC).FirstOrDefault();
            }
            return region;

        }

        public static string Getdearlerdeatilslocation(string dealercode, string usedby)
        {

            string region = string.Empty;
            string json = "{\"PN_PMC\":\"1\",\"PN_DEALER_CD\":\"" + dealercode + "\",\"PN_USER_ID\":\"" + usedby + "\"}";
            var response = WebHTTP.CallForDMSServicesEnquiries("GET_DLR_USAGE_DTL", json);
            JavaScriptSerializer jsdlr = new JavaScriptSerializer();
            DLRdeatils objDLRresults = jsdlr.Deserialize<DLRdeatils>(response.ToString());

            if (objDLRresults.code == "200")
            {
                region = objDLRresults.result.Select(x => x.LOC_DESC).FirstOrDefault();
                // laugname = objlangresults.Result.Where(l => l.LANGUAGE_CODE == code).Select(x => x.LANGUAGE_DESC).FirstOrDefault();
            }
            return region;

        }


        public static DLRdeatilslist Getdearlerdeatils(string dealercode, string usedby)
        {
            DLRdeatilslist objdata = new DLRdeatilslist();
            string region = string.Empty;
            string json = "{\"PN_PMC\":\"1\",\"PN_DEALER_CD\":\"" + dealercode + "\",\"PN_USER_ID\":\"" + usedby + "\"}";
            var response = WebHTTP.CallForDMSServicesEnquiries("GET_DLR_USAGE_DTL", json);
            JavaScriptSerializer jsdlr = new JavaScriptSerializer();
            DLRdeatils objDLRresults = jsdlr.Deserialize<DLRdeatils>(response.ToString());

            if (objDLRresults.code == "200")
            {
                objdata = objDLRresults.result.FirstOrDefault();
                
            }
            return objdata;

        }




        public static string Getmodelname(string modelcode)
        {
            string modelname = string.Empty;
            string json = "{\"PN_PMC\":\"1\",\"PN_SVAR_TYPE\":\"Y\"}";
            var response = WebHTTP.CallForDMSServicesEnquiries("SMAS_SVAR", json);
            JavaScriptSerializer js = new JavaScriptSerializer();
            ResponseDataList<Modaldetails> objresults = js.Deserialize<ResponseDataList<Modaldetails>>(response.ToString());

            if (objresults.code == "200")
            {
                //ViewBag.Modellist = new SelectList(objresults.Result, "SMAS_MODEL_CD", "SMAS_MODEL_DESC");

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
            return modelname;
        }



        public static string GetValidFileName(string fileName)
        {
            // remove any invalid character from the filename.
        String ret = Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "");
        return ret.Replace(" ", String.Empty);
        }

        #region Domin Name
        public static string GetDomin()
        {
             string strDomin = HttpContext.Current.Request.Url.Host.ToString();
            string strUrl = string.Empty;
            if (strDomin != "localhost")
            {
                //strUrl = "http://" + strDomin + "/";

                strUrl= WebConfigurationSettings.Mainurl;
            }
            else
            {
                strUrl = "http://" + strDomin + ":" + HttpContext.Current.Request.ServerVariables["SERVER_PORT"] + "/";
            }
            return strUrl;
        }
        #endregion

        //This function used to convert list into datatable.
        public static DataTable ConvertionToDataTable<T>(System.Collections.Generic.IEnumerable<T> varList)
        {
            DataTable dataTable = new DataTable();

            // Variable for column names.
            PropertyInfo[] tableColumns = null;

            // To check whether more than one elements there in varList.
            foreach (T rec in varList)
            {
                // Use reflection to get column names, to create table.
                if (tableColumns == null)
                {
                    tableColumns = ((Type)rec.GetType()).GetProperties();

                    foreach (PropertyInfo pi in tableColumns)
                    {
                        Type columnType = pi.PropertyType;

                        if ((columnType.IsGenericType) && (columnType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            columnType = columnType.GetGenericArguments()[0];
                        }
                        dataTable.Columns.Add(new DataColumn(pi.Name, columnType));
                    }
                }

                // Copying the IEnumerable value to DataRow and then added into DataTable.
                DataRow dataRow = dataTable.NewRow();

                foreach (PropertyInfo pi in tableColumns)
                {
                    dataRow[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }
                dataTable.Rows.Add(dataRow);
            }
            return (dataTable);
        }

        #region Request Object

        public static int GetRequestParameterAsInt(string id)
        {
            try
            {
                object o = HttpContext.Current.Request[id];

                if (o != null) return Int32.Parse(o.ToString());
            }
            catch
            {
            }

            return 0;
        }

        public static Int16 GetRequestParameterAsInt16(string id)
        {
            try
            {
                object o = HttpContext.Current.Request[id];

                if (o != null) return Int16.Parse(o.ToString());
            }
            catch
            {
            }

            return 0;
        }
        public static Int64 GetRequestParameterAsLong(string id)
        {
            try
            {
                object o = HttpContext.Current.Request[id];

                if (o != null) return Int64.Parse(o.ToString());
            }
            catch
            {
            }

            return 0;
        }

        public static bool GetRequestParameterAsBool(string id)
        {
            try
            {
                object o = HttpContext.Current.Request[id];

                if (o.ToString().ToUpper() == "ON")
                {
                    o = true;
                }
                if (o != null) return Boolean.Parse(o.ToString());
            }
            catch
            {
            }

            return false;
        }

        public static string GetRequestParameterAsString(string id)
        {
            try
            {
                object o = HttpContext.Current.Request[id];

                if (o != null) return o.ToString();
            }
            catch
            {
            }

            return "";
        }

        public static string[] GetRequestParameterValues(string id)
        {
            try
            {
                string[] o = HttpContext.Current.Request.QueryString.GetValues(id);

                if (o != null) return o;
            }
            catch
            {
            }

            string[] s = new string[1];

            return s;
        }

        public static DateTime GetRequestParameterAsDateTime(string id)
        {
            try
            {
                object o = HttpContext.Current.Request[id];

                if (o != null)
                {
                    DateTime d = DateTime.Parse(o.ToString());
                    return d;
                }
            }
            catch
            {
            }

            return DateTime.Today;
        }

        public static bool HasRequestParameter(string id)
        {
            try
            {
                object o = HttpContext.Current.Request[id];

                if (o != null) return true;
            }
            catch
            {
            }

            return false;
        }

        #endregion Request Object



        #region TryParse Object

        public static int TryParseInt16(object o)
        {
            try
            {
                if (o != null) return Int16.Parse(o.ToString());
            }
            catch
            {
            }

            return 0;
        }
        public static int TryParseInt32(object o)
        {
            try
            {
                if (o != null) return Int32.Parse(o.ToString());
            }
            catch
            {
            }

            return 0;
        }
        public static Int64 TryParseInt64(object o)
        {
            try
            {
                if (o != null) return Int64.Parse(o.ToString());
            }
            catch
            {
            }

            return 0;
        }

        public static DateTime TryParseDateTime(object o)
        {
            try
            {
                CultureInfo provider = new CultureInfo("en-IN");
                if (o != null) return DateTime.Parse(o.ToString(), provider);
            }
            catch
            {
                try
                {
                    CultureInfo provider = CultureInfo.CurrentCulture;
                    if (o != null) return DateTime.Parse(o.ToString(), provider);
                }
                catch
                {
                    try
                    {
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        if (o != null) return DateTime.Parse(o.ToString(), provider);
                    }
                    catch
                    {
                        return DateTime.MinValue;
                    }
                }
            }

            return DateTime.MinValue;

        }


        public static string TryParseString(object o)
        {
            try
            {
                if (o != null) return Convert.ToString(o);
            }
            catch
            {
            }

            return "";
        }

        public static bool TryParseBool(object o)
        {
            try
            {
                if (o != null) return bool.Parse(o.ToString());
            }
            catch
            {
            }

            return false;
        }

        public static DateTime TryParseUTCDateTime(object o)
        {
            try
            {
                CultureInfo provider = CultureInfo.CurrentUICulture;
                if (o != null) return DateTime.Parse(o.ToString(), provider).ToUniversalTime();
            }
            catch
            {
                try
                {
                    CultureInfo provider = CultureInfo.CurrentCulture;
                    if (o != null) return DateTime.Parse(o.ToString(), provider).ToUniversalTime();
                }
                catch
                {
                    try
                    {
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        if (o != null) return DateTime.Parse(o.ToString(), provider).ToUniversalTime();
                    }
                    catch
                    {
                        return DateTime.MinValue;
                    }
                }
            }

            return DateTime.MinValue;
        }

        #endregion TryParse Object

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }


    }
}