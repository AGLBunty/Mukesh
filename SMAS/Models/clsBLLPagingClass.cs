using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SMAS.Models
{
    public class clsBLLPagingClass
    {

        string strQuery = string.Empty;
        public string QueryString
        {
            get
            {
                return strQuery;
            }
            set
            {
                strQuery = value;
            }
        }


        #region ---- Static Pageing Variable-----
        public static int PageNo
        {
            /*
             get
             {
                 if (HttpContext.Current.Request.QueryString["page"] != null)
                 {
                     if (HttpContext.Current.Request.QueryString["page"] != "")
                     { return Convert.ToInt32(HttpContext.Current.Request.QueryString["page"].ToString()); }
                     else
                     { return 1; }
                 }
                 else
                 { return 1; }
             }
             */

            get
            {
                if (clsBLLHttpUtility.TryParseInt32(clsBLLHttpUtility.GetRequestParameterAsInt("PageNumber")) == 0)
                {
                    return 0;
                }
                else
                {
                    return clsBLLHttpUtility.TryParseInt32(clsBLLHttpUtility.GetRequestParameterAsInt("PageNumber"));
                }
            }

        }

        public static int PageSizeValue
        {
            /*
            get
            {
                if (HttpContext.Current.Request.QueryString["pagesize"] != null)
                {
                    if (HttpContext.Current.Request.QueryString["pagesize"] != "")
                    {
                        try
                        {
                            return Convert.ToInt32(HttpContext.Current.Request.QueryString["pagesize"].ToString());
                        }
                        catch
                        {
                            return 10;
                        }
                    }
                    else
                    { return 10; }
                }
                else
                { return 10; }
            }*/

            //if (HttpContext.Current.Request.QueryString["pagesize"] != null)
            get
            {
                if (clsBLLHttpUtility.TryParseInt32(clsBLLHttpUtility.GetRequestParameterAsInt("PageSize")) == 0)
                {
                    return 0;
                }
                else
                {
                    return clsBLLHttpUtility.TryParseInt32(clsBLLHttpUtility.GetRequestParameterAsInt("PageSize"));
                }
            }

        }
        public static string SortField
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["sort"] != null)
                {
                    if (HttpContext.Current.Request.QueryString["sort"] != "")
                    { return HttpContext.Current.Request.QueryString["sort"].ToString(); }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
        }


        public static string SearchString
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["searchstring"] != null)
                {
                    if (HttpContext.Current.Request.QueryString["searchstring"] != "")
                    { return HttpContext.Current.Request.QueryString["searchstring"].ToString(); }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
        }

        public static string SortDir
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["sortDir"] != null)
                {
                    if (HttpContext.Current.Request.QueryString["sortDir"] != "")
                    { return HttpContext.Current.Request.QueryString["sortDir"].ToString(); }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
        }

        #endregion

        private Int64 mstrPageSize = 1;
        private Int64 mstrTotalRecords = 1;
        private int mstrCurrentPage = 1;
        private string query = String.Empty;
        private string strSearchTerm = String.Empty;
        private string mstrPagingFunctionName = "Page(";//this is used to make pageing function name dynamic.

        public string PagingFunctionName //this is used to make pageing function name dynamic.
        {
            get
            {
                if (clsBLLHttpUtility.TryParseString(clsBLLHttpUtility.GetRequestParameterAsString("PagingFunctionName")) != "")
                {
                    return clsBLLHttpUtility.TryParseString(clsBLLHttpUtility.GetRequestParameterAsString("PagingFunctionName"));
                }
                else
                {
                    return mstrPagingFunctionName;
                }
            }

            set
            {
                mstrPagingFunctionName = value;
            }

        }


        public Int64 PageSize
        {
            get
            {
                return mstrPageSize;
            }
            set
            {
                mstrPageSize = value;
            }
        }

        public string SearchQuery
        {
            get
            {
                return query;
            }
            set
            {
                query = value;
            }
        }

        public string SearchTerm
        {
            get
            {
                return strSearchTerm;
            }
            set
            {
                strSearchTerm = value;
            }
        }
        public Int64 TotalRecords
        {
            get
            {
                return mstrTotalRecords;
            }
            set
            {
                mstrTotalRecords = value;
            }
        }

        public int CurrentPage
        {
            get
            {
                return mstrCurrentPage;
            }
            set
            {
                mstrCurrentPage = value;
            }
        }

        public StringBuilder GeneratePaging()
        {
            //generate querystring


            //generate querystring

            StringBuilder strPageHtml = new StringBuilder();
            Int64 intNoOfPages = mstrTotalRecords / mstrPageSize;
            if (mstrTotalRecords > mstrPageSize && (mstrTotalRecords % mstrPageSize != 0))
            {
                intNoOfPages++;
            }
            int intPageLimit = 2;
            int intPageDifference = 2;
            Int64 intMinPage = 1;
            Int64 intMaxPage = 5;
            if (intMaxPage > intNoOfPages)
            {
                intMaxPage = intNoOfPages;
            }
            if (mstrCurrentPage > intPageLimit)
            {

                if (mstrCurrentPage < intNoOfPages)
                {
                    intMinPage = mstrCurrentPage - intPageDifference;
                    if ((mstrCurrentPage + intPageDifference) > intNoOfPages)
                    {
                        intMaxPage = intNoOfPages;
                    }
                    else
                    {

                        intMaxPage = mstrCurrentPage + intPageDifference;
                    }
                }
                else
                {
                    intMinPage = mstrCurrentPage - intPageLimit;
                    intMaxPage = intNoOfPages;
                }
            }


            for (Int64 intPage = intMinPage; intPage <= intMaxPage; intPage++)
            {

                if (intPage == intMinPage)
                {
                    if (mstrCurrentPage > 1)
                    {
                        strPageHtml.Append("<li> <a href=\"javascript:void(0);\" onclick=\"return " + PagingFunctionName + "1)\" class=\"NameLink\">|<</a></li>");
                        //strPageHtml.Append("<a href=\"javascript:void(0);\" onclick=\"return " + PagingFunctionName + Convert.ToString(mstrCurrentPage - 1) + ")\" class=\"NameLink\">Previous</a>");
                        strPageHtml.Append("<li><a href=\"javascript:void(0);\" onclick=\"return " + PagingFunctionName + Convert.ToString(mstrCurrentPage - 1) + ")\" class=\"prev\"><<</a></li>");
                    }
                    else
                    {
                        strPageHtml.Append("<li> <a href=\"javascript:void(0);\" class=\"NameLink disabled\">|<</a></li>");
                        strPageHtml.Append("<li> <a href=\"javascript:void(0);\" class=\"prev disabled\"><<</a></li> ");
                    }
                }
                if (intPage == mstrCurrentPage)
                {
                    strPageHtml.Append(" <li><a href=\"javascript:void(0);\" class=\"active\" >[" + intPage + "]</a></li> ");
                }
                else
                {
                    strPageHtml.Append("<li><a href=\"javascript:void(0);\" onclick=\"return " + PagingFunctionName + intPage + ")\" class=\"NameLink\">" + intPage + "</a></li> ");

                }
                if (intPage == intMaxPage)
                {
                    if (mstrCurrentPage < intNoOfPages)
                    {
                        strPageHtml.Append("<li><a href=\"javascript:void(0);\" onclick=\"return " + PagingFunctionName + Convert.ToString(mstrCurrentPage + 1) + ")\" class=\"next\">>></a></li> ");
                        strPageHtml.Append("<li> <a href=\"javascript:void(0);\" onclick=\"return " + PagingFunctionName + intNoOfPages + ")\" class=\"NameLink\">>|</a> </li>");
                    }
                    else
                    {
                        strPageHtml.Append("<li><a href=\"javascript:void(0);\" class=\"next disabled\">>></a></li> ");
                        strPageHtml.Append("<li> <a href=\"javascript:void(0);\" class=\"NameLink disabled\">>|</a></li> ");
                    }
                }

            }

            return strPageHtml;
        }






        static public string RemoveCurrency(string strString)
        {
            strString = strString.Replace("$", "");
            // strString = strString.Replace("_", "");
            strString = strString.Replace("£", "");
            strString = strString.Replace("Rs.", "");

            return strString;
        }

        static public string RemoveNumeric(string strString)
        {
            strString = strString.Replace("0", "");
            strString = strString.Replace("1", "");
            strString = strString.Replace("2", "");
            strString = strString.Replace("3", "");
            strString = strString.Replace("4", "");
            strString = strString.Replace("5", "");
            strString = strString.Replace("6", "");
            strString = strString.Replace("7", "");
            strString = strString.Replace("8", "");
            strString = strString.Replace("9", "");


            return strString;
        }

        public static string makeRandomPassword()
        {
            System.Random myRandom = new System.Random();
            int myLength = myRandom.Next(6, 10);
            string myPassword = "";
            while (myPassword.Length < myLength)
            {
                int myPick = myRandom.Next(1, 3);

                if (myPick == 1)
                {
                    // Add a random number to the string.
                    myPassword += (char)myRandom.Next(48, 57);
                }
                else if (myPick == 2)
                {
                    // Add a random lower case letter to the string.

                    myPassword += (char)myRandom.Next(97, 122);
                }
                else if (myPick == 3)
                {
                    // Add a random upper case letter to the string.

                    myPassword += (char)myRandom.Next(65, 90);
                }
            }
            return myPassword;
        }

        static public string StripHTML(string source)
        {

            try
            {

                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating speces becuase browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<( )head([^>])>", "<head>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"(<( )*(/)( )*head( )*>)", "</head>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                "(<head>).*(</head>)", string.Empty,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<( )script([^>])>", "<script>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"(<( )*(/)( )*script( )*>)", "</script>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                // @"(<script>)([^(<script>\.</script>)])*(</script>)",
                // string.Empty,
                // System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"(<script>).*(</script>)", string.Empty,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<( )style([^>])>", "<style>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"(<( )*(/)( )*style( )*>)", "</style>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                "(<style>).*(</style>)", string.Empty,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<( )td([^>])>", "\t",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<( )*br( )*>", "\r",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<( )*li( )*>", "\r",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<( )div([^>])>", "\r\r",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<( )tr([^>])>", "\r\r",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<( )p([^>])>", "\r\r",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything thats enclosed inside < >
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"<[^>]*>", string.Empty,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @" ", " ",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&bull;", " * ",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&lsaquo;", "<",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&rsaquo;", ">",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&trade;", "(tm)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&frasl;", "/",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&lt;", "<",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&gt;", ">",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&copy;", "(c)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&reg;", "(r)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = System.Text.RegularExpressions.Regex.Replace(result,
                @"&(.{2,6});", string.Empty,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testng
                //System.Text.RegularExpressions.Regex.Replace(result,
                // this.txtRegex.Text,string.Empty,
                // System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces inbetween
                // the escaped characters and remove redundant tabs inbetween linebreaks
                result = System.Text.RegularExpressions.Regex.Replace(result,
                "(\r)( )+(\r)", "\r\r",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                "(\t)( )+(\t)", "\t\t",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                "(\t)( )+(\r)", "\t\r",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                "(\r)( )+(\t)", "\r\t",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result,
                "(\r)(\t)+(\r)", "\r\r",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multible tabs followind a linebreak with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result,
                "(\r)(\t)+", "\r\t",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for linebreaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                // Thats it.
                return result;

            }
            catch
            {
                //MessageBox.Show("Error");
                return source;
            }
        }


    }
}