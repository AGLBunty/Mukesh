using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMAS.Models.ExceptionDT
{
    public class BalExceptionDetails
    {
        public static void WriteException(Exception ex)
        {
            IExceptionDetails objExceptionDetails = new ExceptionDetails();
            objExceptionDetails.WriteException(ex);
			
        }
    }
}