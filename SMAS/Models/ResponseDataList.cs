using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMAS.Models
{
    public class ResponseDataList<T> where T : class
    {
        public string code { get; set; }
        public string message { get; set; }
        public IEnumerable<T> Result
		{
            get;
            set;
        }
		
    }
}