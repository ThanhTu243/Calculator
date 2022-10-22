using Model.BaseModel;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public interface IScoreService
    {
        #region Crawl
        Task<int> CrawlListUniversity();
        Task<int> CrawlListHrefMajor();
        Task<int> CrawlListHrefScore();
        Task<int> CrawlListMajor();
        Task<int> CrawlCombination();
        Task<int> CrawlListMajorAndCombination();
        Task<int> CrawlScoreUniversity(string year);
        #endregion

        #region Get value
        Task<IEnumerable<ViewUniversity>> GetUniversity();
        Task<IEnumerable<Province>> GetProvince();
        Task<IEnumerable<University_Type>> GetUniversityType();
        Task<IEnumerable<ViewUniversity_Score>> GetScoreByUniversity(string CodeUniversity,string Year);
        Task<IEnumerable<ViewMajor_Combination>> GetMajorCombination(string CodeMajor);
        Task<IEnumerable<ViewUniversity>> GetUniversityByTypeAndProvince(int? IdType, int? IdProvince);
        Task<IEnumerable<ViewUniversity>> GetUniversityByCodeOrName(string keyword);
        Task<IEnumerable<ViewMajor>> GetMajor();
        Task<IEnumerable<ViewMajor>> GetMajorByQuantity(int quantity);
        Task<IEnumerable<ViewUniversity_Score_and_University>> GetTopMajorByYear(string year,int quantity);
        Task<IEnumerable<ViewUniversity_Score>> GetScoreByMajor(string CodeMajor);
        Task<IEnumerable<ViewCombination>> GetCombination();
        Task<IEnumerable<ViewMajorGroup>> GetGroupMajor();
        Task<IEnumerable<ViewMajorGroup>> GetGroupMajor_Major();
        Task<IEnumerable<ViewPredictScore>> PredictScoreByCodeMajor(float FromPoint, float ToPoint, string CodeMajor, string CodeCombination, int? Province);
        Task<IEnumerable<DetailPredictScore>> DetailPredictScoreByCodeMajor(float FromPoint, float ToPoint, string CodeUniversity, string CodeMajor, string CodeCombination);
        Task<IEnumerable<ViewPredictScore>> PredictScoreByGroupMajor(float FromPoint, float ToPoint, int GroupMajor, string CodeCombination, int? Province);
        Task<IEnumerable<DetailPredictScore>> DetailPredictScoreByGroupMajor(float FromPoint, float ToPoint, string CodeUniversity, int GroupMajor, string CodeCombination);
        #endregion

        #region
        Task<int> CreateUniversity(string Code, string Name, int Type, int Province,string Address, string CreatedBy);
        Task<int> UpdateUniversity(int Id, string Code, string Name, int Type, int Province,string Address, string UpdatedBy);
        Task<int> DeleteUniversity(int Id, string DeletedBy);
        Task<int> CreateGroupMajor(string NameGroupMajor, string CreatedBy);
        Task<int> UpdateGroupMajor(int Id, string NameGroupMajor, string UpdatedBy);
        Task<int> DeleteGroupMajor(int Id, string DeletedBy);
        Task<int> CreateMajor(string CodeMajor, string NameMajor,bool Hot, string CreatedBy, int IdGroupMajor);
        Task<int> UpdateMajor(int Id, string CodeMajor, string NameMajor,bool Hot, string UpdatedBy,int IdGroupMajor);
        Task<int> DeleteMajor(int Id, string DeletedBy);
        Task<int> CreateCombination(string CodeCombination, string NameCombination, string CreatedBy);
        Task<int> UpdateCombination(int Id, string CodeCombination, string NameCombination, string UpdatedBy);
        Task<int> DeleteCombination(int Id, string DeletedBy);
        Task<int> CreateScore(string CodeMajor, string NameMajor, string CodeCombination, double Score, string Description, string Year, string CodeUniversity, string CreatedBy);
        Task<int> UpdateScore(int Id, string CodeMajor, string NameMajor, string CodeCombination, double Score, string Description, string Year, string CodeUniversity, string UpdatedBy);
        Task<int> DeleteScore(int Id, string DeletedBy);
        Task<int> CreateMajor_Combination(string CodeMajor, string CodeCombination, string CreatedBy);
        Task<int> UpdateMajor_Combination(int Id, string CodeMajor, string CodeCombination, string UpdatedBy);
        Task<int> DeleteMajor_Combination(int Id, string DeletedBy);
        #endregion
    }
}
