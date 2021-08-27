using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMAS.Models.List
{
   public interface IListRepository
    {
        IEnumerable<Company> GetCompany();
        Task<IEnumerable<Designation>> GetDesignation();
        Task<IEnumerable<Vertical>> GetVertical();
        IEnumerable<Assign> GetAssign();
        IEnumerable<FileUpload> GetFileUploaddata();
        Task<IEnumerable<Languages>> GetLanguages();
		Task<IEnumerable<Model>> GetModel();
		Task<IEnumerable<FileVersions>> GetFileVersions();
		
		Task<IEnumerable<FileVersions>> GetFileVersionsbyId(string SID);
		
		IEnumerable<ModelCount> GetDataCountbyMid( string MSTATUS);
		Task<ModelMainResponce> GetDataCountbyMidDataa(string MSTATUS,string LID);
		Task<FileResponce> GetDatabyMId(string MID, string LID, int SID, string ROSTATUS);
		Task<ResponceServiceManuals> GetServiceManuals();
		Task<UpdateResponce> FileUploadModelUpdate(int ID);
		//Task<UpdateResponce> SubscribeSaveData(int FID,string DEALERCODE);
		Task<FeedbackResponce> GetFeedback();
		Task<UpdateResponce> FeedbackSaveData(Feedbacks feedsubdata);
		Task<UpdateResponce> FileUploadDelete(int ID);
		Task<FileResponce> GetDataFilePath(FileUpload objreq);
		Task<DealerFileResponce> GetDatabydealercode(string MID, string LID, int SID, string dealercode,string ROSTATUS);
		//DELARE WEB API
		Task<ModelMainResponce> GetDealermodellist(string MSTATUS, string LID);

        Task<UsageReportResponce> GetUsageReport(Reportrequest _ObjReport);

        Task<consolidateresponse> GetCosolidateReport(ReportrequestConsolidate _ObjReport);

        Task<UpdateResponce> SubscribeSaveFinalData(int FID, int SID, string DEALERCODE, string USEDBY, string DEVICENAME);

        Task<SubscribeResponce> GetSubsribeData();

        Task<SubscribeResponce_Final> GetSubsribe_FinalData();

        Task<UpdateResponce> SubscribeSaveData(int FID, int? SID, string DEALERCODE, string USEDBY);

        // Add 25-08-2021
        Task<List<DropdownValues>> GetValue(string Parent);
    }
}
