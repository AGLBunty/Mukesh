using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SMAS.Models.ExceptionDT
{
    public interface IExceptionDetails
    {
        /// <summary>
        /// WriteException
        /// </summary>
        /// <param name="ex"></param>
        void WriteException(Exception ex);
    }
}
