using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SMAS.Models
{
	public class ServiceHelper
	{
		public static ServiceHeaderInfo Authenticate(IncomingWebRequestContext requestContext)
		{
			ServiceHeaderInfo headerInfo = new ServiceHeaderInfo();
			System.Net.WebHeaderCollection headerCollection = requestContext.Headers;
			headerInfo.Token = headerCollection["Token"];

			if (headerInfo.Token == "dR$545#h^jjmanJ")
			{
				headerInfo.IsAuthenticated = true;
			}
			else
			{
				headerInfo.IsAuthenticated = false;
			}

			return headerInfo;
		}

		internal static ServiceHeaderInfo Authenticate(object incomingRequest)
		{
			throw new NotImplementedException();
		}
	}
	public class ServiceHeaderInfo
	{
		public bool IsAuthenticated { get; set; }
		public string Token { get; set; }
	}
	public class IncomingWebRequestContext
	{
		public WebHeaderCollection Headers { get; }
	}
}