using SMAS.Models;
using SMAS.Models.ExceptionDT;
using SMAS.Models.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SMAS.Controllers
{
	public class ListApiController : ApiController
	{
		private readonly IListRepository _iListRepository = new ListRepository();

		[HttpGet]
		[Route("api/List/GetAssign")]
		public HttpResponseMessage GetAssign()
		{
			HttpResponseMessage response = null;
			try
			{
				response = new HttpResponseMessage();
				var dataAssign = _iListRepository.GetAssign();
				response = Request.CreateResponse(HttpStatusCode.OK, dataAssign);
			}
			catch (Exception ex)
			{
				BalExceptionDetails.WriteException(ex);
				response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);

			}
			return response;
		}



        [HttpGet]
        [Route("api/List/GetFileUploaddata")]
        public HttpResponseMessage GetFileUploaddata()
        {
            HttpResponseMessage response = null;
            try
            {
                response = new HttpResponseMessage();
                var datafileupload = _iListRepository.GetFileUploaddata();
                response = Request.CreateResponse(HttpStatusCode.OK, datafileupload);
            }
            catch (Exception ex)
            {
                BalExceptionDetails.WriteException(ex);
                response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);

            }
            return response;
        }


        [HttpGet]
		[Route("api/List/GetDesignation")]
		public async Task<IEnumerable<Designation>> GetDesignation()
		{
			return await _iListRepository.GetDesignation();
		}

		[HttpGet]
		[Route("api/List/GetVertical")]
		public async Task<IEnumerable<Vertical>> GetVertical()
		{
			return await _iListRepository.GetVertical();
		}
		[HttpGet]
		[Route("api/List/GetLanguages")]
		public async Task<IEnumerable<Languages>> GetLanguages()
		{
			return await _iListRepository.GetLanguages();
		}
		[HttpGet]
		[Route("api/List/GetModel")]
		public async Task<IEnumerable<Model>> GetModel()
		{
			return await _iListRepository.GetModel();
		}
		[HttpGet]
		[Route("api/List/GetFileVersions")]
		public async Task<IEnumerable<FileVersions>> GetFileVersions()
		{
			return await _iListRepository.GetFileVersions();
		}
		

		[HttpGet]
		[Route("api/List/GetFileVersionsbyId/{SID}")]
		public async Task<IEnumerable<FileVersions>> GetFileVersionsbyId(string SID)
		{
			var result = await _iListRepository.GetFileVersionsbyId(SID);
			return result;
		}
		[HttpPost]
		[Route("api/List/GetDataCountbyMidDataa")]
		public async Task<ModelMainResponce> GetDataCountbyMidDataa(ModelCount request)
		{
			var result = await _iListRepository.GetDataCountbyMidDataa(request.MSTATUS,request.LID);
			return result;
		}

		[HttpPost]
		[Route("api/List/GetDatabyMId")]
		public async Task<FileResponce> GetDatabyMId(Request req)
		{
			var result = await _iListRepository.GetDatabyMId(req.MID, req.LID, req.SID,req.ROSTATUS);
			return result;
		}
		[HttpGet]
		[Route("api/List/GetServiceManuals")]
		public async Task<ResponceServiceManuals> GetServiceManuals()
		{
			var result = await _iListRepository.GetServiceManuals();
			return result;
		}

		[HttpPost]
		[Route("api/List/PublishUpdate/{ID}")]
		public async Task<UpdateResponce> FileUploadModelUpdate(int ID)
		{
			var result= await _iListRepository.FileUploadModelUpdate(ID);

			return result;
		}
        [HttpPost]
        [Route("api/List/SubscribeSaveData")]
        public async Task<UpdateResponce> SubscribeSaveData([FromBody]Subscribe subdata)
        {
            var result = await _iListRepository.SubscribeSaveData(subdata.FID, subdata.SID, subdata.DEALERCODE, subdata.USEDBY);
            return result;

        }
        [HttpGet]
		[Route("api/List/GetFeedback")]
		public async Task<FeedbackResponce> GetFeedback()
		{
			FeedbackResponce _objdata = new FeedbackResponce();
			
		   _objdata =await _iListRepository.GetFeedback();
			
			return _objdata;
		}

		[HttpPost]
		[Route("api/List/FeedbackSaveData")]
		public async Task<UpdateResponce> FeedbackSaveData([FromBody]Feedbacks feedsubdata)
	    {
			UpdateResponce _objfeedbackdata = new UpdateResponce();
			_objfeedbackdata = await _iListRepository.FeedbackSaveData(feedsubdata);
			return _objfeedbackdata;

		}

		[HttpPost]
		[Route("api/List/DeleteFile/{ID}")]
		public async Task<UpdateResponce> FileUploadDelete(int ID)
		{
			var result = await _iListRepository.FileUploadDelete(ID);

			return result;
		}

		[HttpPost]
		[Route("api/List/GetDataFilePath/")]
		public async Task<FileResponce> GetDataFilePath(FileUpload objreq)
		{
			var result = await _iListRepository.GetDataFilePath(objreq);
			return result;
		}

		[HttpPost]
		[Route("api/List/GetDatabydealercode")]
		public async Task<DealerFileResponce> GetDatabydealercode(Request req)
		{
			var result = await _iListRepository.GetDatabydealercode(req.MID, req.LID, req.SID,req.DEALERCODE,req.ROSTATUS);
			return result;
		}
        [HttpPost]
        [Route("api/List/GetDealermodellist")]
        public async Task<ModelMainResponce> GetDealermodellist(ModelCount request)
        {
            var result = await _iListRepository.GetDealermodellist(request.MSTATUS, request.LID);
            return result;
        }

        [HttpPost]
        [Route("api/List/GetUsageReport")]
        public async Task<UsageReportResponce> GetUsageReport(Reportrequest request)
        {
            var result = await _iListRepository.GetUsageReport(request);
            return result;
        }


        [HttpPost]
        [Route("api/List/GetconsolidateReport")]
        public async Task<consolidateresponse> GetconsolidateReport(ReportrequestConsolidate request)
        {
            var result = await _iListRepository.GetCosolidateReport(request);
            return result;
        }


        [HttpGet]
        [Route("api/List/GetSubsribeData")]
        public async Task<SubscribeResponce> GetSubsribeData()
        {
            SubscribeResponce _objdata = new SubscribeResponce();

            _objdata = await _iListRepository.GetSubsribeData();

            return _objdata;
        }

        [HttpGet]
        [Route("api/List/GetSubsribe_FinalData")]
        public async Task<SubscribeResponce_Final> GetSubsribe_FinalData()
        {
            SubscribeResponce_Final _objdata = new SubscribeResponce_Final();

            _objdata = await _iListRepository.GetSubsribe_FinalData();

            return _objdata;
        }


        [HttpPost]
        [Route("api/List/SubscribeSaveFinalData")]
        public async Task<UpdateResponce> SubscribeSaveFinalData([FromBody]SUBSUCRIBE_FINAL subdata)
        {
            var result = await _iListRepository.SubscribeSaveFinalData(subdata.FID, subdata.SID, subdata.DEALERCODE, subdata.USEDBY,subdata.DEVICENAME);
            return result;

        }

        //add 25-08-2021

        [HttpGet]
        [Route("api/List/GetValue/{Parent}")]
        public async Task<List<DropdownValues>> GetValue(string Parent)
        {
            return await _iListRepository.GetValue(Parent);
        }



    }
}
