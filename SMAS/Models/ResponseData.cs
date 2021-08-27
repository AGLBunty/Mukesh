using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SMAS.Models
{
    public sealed class ResponseData<T> where T : class
    {
        //public ResponseData();

        public string code { get; set; }
        public string message { get; set; }
        public object ObjectResponse { get; set; }

        
        public T Result { get; set; }
        /// <summary>
        /// Property to get set validation messages
        /// </summary>
     
        public string[] FailedValidations
        {
            get;
            set;
        }

        /// <summary>
        /// Response message will be either for success or unsuccess
        /// </summary>
     
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Response Status Code
        /// </summary>
     
        public string StatusCode
        {
            get;
            set;
        }

     
        public IList<T> Results
        {
            get;
            set;
        }

        public IEnumerable<T> Resultslist
        {
            get;
            set;
        }



        public DataTable ResultsTable
        {
            get;
            set;
        }

        /// <summary>
        /// True, If bussiness logic return successful response
        /// </summary>
     
        public bool IsSuccess
        {
            get;
            set;
        }

     
        public int RecordCount { get; set; }

     
        public decimal ProgramVersion { get; set; }
    }
}