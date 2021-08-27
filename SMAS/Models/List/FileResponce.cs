using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMAS.Models.List
{
	public class FileResponce
	{

		public string code { get; set; }
		public string message { get; set; }
		public IEnumerable<FileUpload>result { get; set; }
	}


	public class DealerFileResponce
	{

		public string code { get; set; }
		public string message { get; set; }
		public IEnumerable<SubscribeModel> result { get; set; }
	}
	public class ResponceServiceManuals
	{
		public string code { get; set; }
		public string message { get; set; }
		public IEnumerable<ServiceManuals>result { get; set; }
	}
}