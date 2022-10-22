using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DataAccess.Repositories
{
    public interface IScoreRepository
    {
        #region Crawl
        Task<GridReader> GetListProvince();
        Task<GridReader> GetListTypeUniversity();
        Task<int> ImportUniversity(string jsonResults);
        Task<int> ImportMajor(string jsonResults);
        Task<int> ImportCombination(string jsonResults);
        Task<int> ImportMajor_Combination(string jsonResults);
        Task<int> ImportScore(string jsonResults);
        Task<int> CustomizeNameMajorToCode();
        Task<int> CustomizeCodeMajorToId();
        #endregion

        #region GetValue
        Task<GridReader> GetListCombination();
        Task<GridReader> GetListMajor_Combination(string CodeMajor);
        Task<GridReader> GetListUniversity();
        Task<GridReader> GetListMajor();
        Task<GridReader> GetTopMajorByYear(string year,int quantity);

        Task<GridReader> GetListMajorByQuantity(int quantity);
        Task<GridReader> GetGroupMajor();
        Task<GridReader> GetListGroupMajor(int IdGroupMajor);
        Task<GridReader> GetScoreByUniversity(string CodeUniversity,string Year);
        Task<GridReader> GetScoreByMajor(string CodeMajor);
        Task<GridReader> GetUniversityByTypeAndProvince(int? IdType, int? IdProvince);
        Task<GridReader> PredictScoreByCodeMajor(float FromPoint, float ToPoint, string CodeMajor,string CodeCombination,int? Province);
        Task<GridReader> DetailPredictScoreByCodeMajor(float FromPoint, float ToPoint,string CodeUniversity, string CodeMajor,string CodeCombination);
        Task<GridReader> PredictScoreByGroupMajor(float FromPoint, float ToPoint, int GroupMajor,string CodeCombination, int? Province);
        Task<GridReader> DetailPredictScoreByGroupMajor(float FromPoint, float ToPoint, string CodeUniversity, int GroupMajor,string CodeCombination);
        #endregion

        #region Crud
        Task<int> CreateUniversity(string Code, string Name, int Type, int Province, string Address, string CreatedBy);
        Task<int> UpdateUniversity(int Id,string Code, string Name, int Type, int Province, string Address, string UpdatedBy);
        Task<int> DeleteUniversity(int Id, string DeletedBy);
        Task<int> CreateGroupMajor(string NameGroupMajor, string CreatedBy);
        Task<int> UpdateGroupMajor(int Id,string NameGroupMajor, string UpdatedBy);
        Task<int> DeleteGroupMajor(int Id, string DeletedBy);
        Task<int> CreateMajor(string CodeMajor,string NameMajor, bool Hot, string CreatedBy, int IdGroupMajor);
        Task<int> UpdateMajor(int Id, string CodeMajor,string NameMajor, bool Hot, string UpdatedBy, int IdGroupMajor);
        Task<int> DeleteMajor(int Id, string DeletedBy);
        Task<int> CreateCombination(string CodeCombination, string NameCombination, string CreatedBy);
        Task<int> UpdateCombination(int Id, string CodeCombination, string NameCombination, string UpdatedBy);
        Task<int> DeleteCombination(int Id, string DeletedBy);
        Task<int> CreateScore(string CodeMajor,string NameMajor, string CodeCombination,double Score,string Description, string Year,string CodeUniversity, string CreatedBy);
        Task<int> UpdateScore(int Id,string CodeMajor, string NameMajor, string CodeCombination, double Score, string Description, string Year, string CodeUniversity, string UpdatedBy);
        Task<int> DeleteScore(int Id, string DeletedBy);
        Task<int> CreateMajor_Combination(string CodeMajor, string CodeCombination, string CreatedBy);
        Task<int> UpdateMajor_Combination(int Id, string CodeMajor, string CodeCombination, string UpdatedBy);
        Task<int> DeleteMajor_Combination(int Id, string DeletedBy);
        #endregion
    }
}
