using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMAS.Models.List
{
	public class FeedbackResponce
	{
		public string code { get; set; }
		public string message { get; set; }
		public IEnumerable<Feedbacks> result { get; set; }
	}
}