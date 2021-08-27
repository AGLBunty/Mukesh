using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMAS.Models.ExceptionDT
{
    public class ExceptionDetails : IExceptionDetails
    {
        private readonly SqlDbContext db = new SqlDbContext();
        public void WriteException(Exception ex)
        {
            using (var dbcontext = new SqlDbContext())
            {
                var exceptionDetails = new EXCEPTION_DETAILS
                {
                    EXCEPTION_MESSAGE = ex.Message,
                    EXCEPTION_DESC = Convert.ToString(ex),
                    EXCEPTION_DATE = DateTime.Now
                };
                dbcontext.EXCEPTION_DETAILS.Add(exceptionDetails);
                dbcontext.SaveChanges();
            }
        }
    }


    public class EXCEPTION_DETAILS
    {
        [Key]
        public int ID { get; set; }
        public string EXCEPTION_MESSAGE { get; set; }
        public string EXCEPTION_DESC { get; set; }
        public Nullable<System.DateTime> EXCEPTION_DATE { get; set; }
    }

}