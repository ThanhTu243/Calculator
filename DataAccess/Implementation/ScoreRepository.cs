using Dapper;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DataAccess.Implementation
{
    public class ScoreRepository : BaseRepository, IScoreRepository
    {
        public ScoreRepository(IDbTransaction transaction)
        : base(transaction)
        {
        }

        #region Crawl
        public async Task<int> ImportScore(string jsonResults)
        {
            try
            {
                var result = await ExecuteAsync("ImportScore", new
                {
                    @JsonData = jsonResults
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ImportUniversity(string jsonResults)
        {
            try
            {
                int result = await ExecuteAsync("ImportUniversity", new
                {
                    @JsonData = jsonResults
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ImportCombination(string jsonResults)
        {
            try
            {
                int result = await ExecuteAsync("ImportCombination", new
                {
                    @JsonData = jsonResults
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ImportMajor(string jsonResults)
        {
            try
            {
                int result = await ExecuteAsync("ImportMajor", new
                {
                    @JsonData = jsonResults
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ImportMajor_Combination(string jsonResults)
        {
            try
            {
                int result = await ExecuteAsync("ImportMajor_Combination", new
                {
                    @JsonData = jsonResults
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CustomizeNameMajorToCode()
        {
            try
            {
                int result = await ExecuteAsync("ChangeNameMajorToCode", new
                {

                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> CustomizeCodeMajorToId()
        {
            try
            {
                int result = await ExecuteAsync("ChangeCodeMajorToId", new
                {

                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Get value
        public async Task<GridReader> GetListProvince()
        {
            try
            {
                var result = await QueryMultipleAsync("SelectProvince", new
                {

                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridReader> GetListTypeUniversity()
        {
            try
            {
                var result = await QueryMultipleAsync("SelectUniversityType", new
                {

                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridReader> GetListMajor()
        {
            try
            {
                var result = await QueryMultipleAsync("SelectMajor", new
                {

                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridReader> GetListUniversity()
        {
            try
            {
                var result = await QueryMultipleAsync("SelectUniversity", new
                {

                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridReader> GetListCombination()
        {
            try
            {
                var result = await QueryMultipleAsync("SelectCombination", new
                {

                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridReader> GetUniversityByTypeAndProvince(int? IdType, int? IdProvince)
        {
            try
            {
                var result = await QueryMultipleAsync("SelectUniversityByTypeAndProvince", new
                {
                    @IdType = IdType,
                    @IdProvince = IdProvince
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridReader> GetScoreByUniversity(string CodeUniversity,string Year)
        {
            try
            {
                var result = await QueryMultipleAsync("SelectScoreByUniversity", new
                {
                    @CodeUniversity = CodeUniversity,
                    @Year = Year
                }) ;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridReader> GetScoreByMajor(string CodeMajor)
        {
            try
            {
                var result = await QueryMultipleAsync("SelectScoreByMajor", new
                {
                    @CodeMajor = CodeMajor
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridReader> GetListMajor_Combination(string CodeMajor)
        {
            try
            {
                var result = await QueryMultipleAsync("SelectMajorCombination", new
                {
                    @CodeMajor = CodeMajor
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public async Task<GridReader> GetGroupMajor()
        {
            try
            {
                var result = await QueryMultipleAsync("SelectGroupMajor", new
                {

                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GridReader> GetListMajorByQuantity(int quantity)
        {
            try
            {
                var result = await QueryMultipleAsync("SelectMajorByQuantity", new
                {
                    @Quantity=quantity
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GridReader> GetTopMajorByYear(string year, int quantity)
        {
            try
            {
                var result = await QueryMultipleAsync("SelectTopMajorByYear", new
                {
                    @Year = year,@Quantity=quantity
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GridReader> GetListGroupMajor(int IdGroupMajor)
        {
            try
            {
                var result = await QueryMultipleAsync("SelectGroupMajor_Major", new
                {
                    @IdGroupMajor = IdGroupMajor
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GridReader> PredictScoreByCodeMajor(float FromPoint, float ToPoint, string CodeMajor, string CodeCombination, int? Province)
        {
            try
            {
                var result = await QueryMultipleAsync("PredictScoreByCodeMajor", new
                {
                    @FromPoint = FromPoint,
                    @ToPoint = ToPoint,
                    @CodeMajor = CodeMajor,
                    @IdProvince = Province,
                    @CodeCombination = CodeCombination
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GridReader> DetailPredictScoreByCodeMajor(float FromPoint, float ToPoint, string CodeUniversity, string CodeMajor,string CodeCombination)
        {
            try
            {
                var result = await QueryMultipleAsync("DetailPredictScoreByCodeMajor", new
                {
                    @FromPoint = FromPoint,
                    @ToPoint = ToPoint,
                    @CodeUniversity = CodeUniversity,
                    @CodeMajor = CodeMajor,
                    @CodeCombination = CodeCombination
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GridReader> PredictScoreByGroupMajor(float FromPoint, float ToPoint, int GroupMajor,string CodeCombination, int? Province)
        {
            try
            {
                var result = await QueryMultipleAsync("PredictScoreByGroupMajor", new
                {
                    @FromPoint = FromPoint,
                    @ToPoint = ToPoint,
                    @GroupMajor = GroupMajor,
                    @IdProvince = Province,
                    @CodeCombination = CodeCombination
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GridReader> DetailPredictScoreByGroupMajor(float FromPoint, float ToPoint, string CodeUniversity, int GroupMajor,string CodeCombination)
        {
            try
            {
                var result = await QueryMultipleAsync("DetailPredictScoreByGroupMajor", new
                {
                    @FromPoint = FromPoint,
                    @ToPoint = ToPoint,
                    @CodeUniversity = CodeUniversity,
                    @GroupMajor = GroupMajor,
                    @CodeCombination = CodeCombination
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CRUD
        public async Task<int> CreateUniversity(string Code, string Name, int Type, int Province,string Address, string CreatedBy)
        {
            try
            {
                int result = await ExecuteAsync("CreateUniversity", new
                {
                   @Code = Code,
                   @Name = Name,
                   @Type = Type,
                   @Province = Province,
                   @Address = Address,
                   @CreatedDay = DateTime.Now,
                   @CreatedBy = CreatedBy,
                   @IsDeleted = 0
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> UpdateUniversity(int Id,string Code, string Name, int Type, int Province, string Address, string UpdatedBy)
        {
            try
            {
                int result = await ExecuteAsync("UpdateUniversity", new
                {
                    @Id = Id,
                    @Code = Code,
                    @Name = Name,
                    @Type = Type,
                    @Province = Province,
                    @Address = Address,
                    @UpdatedDay = DateTime.Now,
                    @UpdatedBy = UpdatedBy
                }) ;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> DeleteUniversity(int Id, string DeletedBy)
        {
            try
            {
                int result = await ExecuteAsync("DeleteUniversity", new
                {
                    @Id = Id,
                    @DeletedDay = DateTime.Now,
                    @DeletedBy = DeletedBy,
                    @IsDeleted = 1
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateGroupMajor(string NameGroupMajor, string CreatedBy)
        {
            try
            {
                int result = await ExecuteAsync("CreateGroupMajor", new
                {                   
                    @NameGroupMajor = NameGroupMajor,
                    @CreatedDay = DateTime.Now,
                    @CreatedBy = CreatedBy,
                    @IsDeleted = 0
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateGroupMajor(int Id, string NameGroupMajor, string UpdatedBy)
        {
            try
            {
                int result = await ExecuteAsync("UpdateGroupMajor", new
                {
                    @Id = Id,
                    @NameGroupMajor = NameGroupMajor,
                    @UpdatedBy = UpdatedBy,
                    @UpdatedDay = DateTime.Now
                }) ;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteGroupMajor(int Id, string DeletedBy)
        {
            try
            {
                int result = await ExecuteAsync("DeleteGroupMajor", new
                {
                    @Id = Id,
                    @DeletedDay = DateTime.Now,
                    @DeletedBy = DeletedBy,
                    @IsDeleted = 1
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateMajor(string CodeMajor, string NameMajor,bool Hot, string CreatedBy,int IdGroupMajor)
        {
            try
            {
                int result = await ExecuteAsync("CreateMajor", new
                {
                    @CodeMajor = CodeMajor,
                    @NameMajor = NameMajor,
                    @Hot=Hot,
                    @CreatedDay = DateTime.Now,
                    @CreatedBy = CreatedBy,
                    @IsDeleted = 0,
                    @IdGroupMajor = IdGroupMajor
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateMajor(int Id, string CodeMajor, string NameMajor,bool Hot, string UpdatedBy, int IdGroupMajor)
        {
            try
            {
                int result = await ExecuteAsync("UpdateMajor", new
                {
                    @Id = Id,
                    @CodeMajor = CodeMajor,
                    @NameMajor = NameMajor,
                    @Hot=Hot,
                    @UpdatedBy = UpdatedBy,
                    @UpdatedDay = DateTime.Now,
                    @IdGroupMajor = IdGroupMajor
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteMajor(int Id, string DeletedBy)
        {
            try
            {
                int result = await ExecuteAsync("DeleteMajor", new
                {
                    @Id = Id,
                    @DeletedDay = DateTime.Now,
                    @DeletedBy = DeletedBy,
                    @IsDeleted = 1
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateCombination(string CodeCombination, string NameCombination, string CreatedBy)
        {
            try
            {
                int result = await ExecuteAsync("CreateCombination", new
                {
                    @CodeCombination = CodeCombination,
                    @NameCombination = NameCombination,
                    @CreatedDay = DateTime.Now,
                    @CreatedBy = CreatedBy,
                    @IsDeleted = 0
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateCombination(int Id, string CodeCombination, string NameCombination, string UpdatedBy)
        {
            try
            {
                int result = await ExecuteAsync("UpdateCombination", new
                {
                    @Id = Id,
                    @CodeCombination = CodeCombination,
                    @NameCombination = NameCombination,
                    @UpdatedBy = UpdatedBy,
                    @UpdatedDay = DateTime.Now
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteCombination(int Id, string DeletedBy)
        {
            try
            {
                int result = await ExecuteAsync("DeleteCombination", new
                {
                    @Id = Id,
                    @DeletedDay = DateTime.Now,
                    @DeletedBy = DeletedBy,
                    @IsDeleted = 1
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateScore(string CodeMajor, string NameMajor, string CodeCombination, double Score, string Description, string Year, string CodeUniversity, string CreatedBy)
        {
            try
            {
                int result = await ExecuteAsync("CreateScore", new
                {
                    @CodeMajor = CodeMajor,
                    @NameMajor = NameMajor,
                    @CodeCombination = CodeCombination,
                    @Score = Score,
                    @Description = Description,
                    @Year = Year,
                    @CodeUniversity = CodeUniversity,
                    @CreatedDay = DateTime.Now,
                    @CreatedBy = CreatedBy,
                    @IsDeleted = 0
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateScore(int Id, string CodeMajor, string NameMajor, string CodeCombination, double Score, string Description, string Year, string CodeUniversity, string UpdatedBy)
        {
            try
            {
                int result = await ExecuteAsync("UpdateScore", new
                {
                    @Id = Id,
                    @CodeMajor = CodeMajor,
                    @NameMajor = NameMajor,
                    @CodeCombination = CodeCombination,
                    @Score = Score,
                    @Description = Description,
                    @Year = Year,
                    @CodeUniversity = CodeUniversity,
                    @UpdatedBy = UpdatedBy,
                    @UpdatedDay = DateTime.Now
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteScore(int Id, string DeletedBy)
        {
            try
            {
                int result = await ExecuteAsync("DeleteScore", new
                {
                    @Id = Id,
                    @DeletedDay = DateTime.Now,
                    @DeletedBy = DeletedBy,
                    @IsDeleted = 1
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateMajor_Combination(string CodeMajor, string CodeCombination, string CreatedBy)
        {
            try
            {
                int result = await ExecuteAsync("CreateMajor_Combination", new
                {
                    @CodeMajor = CodeMajor,                  
                    @CodeCombination = CodeCombination,               
                    @CreatedDay = DateTime.Now,
                    @CreatedBy = CreatedBy,
                    @IsDeleted = 0
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateMajor_Combination(int Id, string CodeMajor, string CodeCombination, string UpdatedBy)
        {
            try
            {
                int result = await ExecuteAsync("UpdateMajor_Combination", new
                {
                    @Id = Id,
                    @CodeMajor = CodeMajor,
                    @CodeCombination = CodeCombination,                 
                    @UpdatedBy = UpdatedBy,
                    @UpdatedDay = DateTime.Now
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteMajor_Combination(int Id, string DeletedBy)
        {
            try
            {
                int result = await ExecuteAsync("DeleteMajor_Combination", new
                {
                    @Id = Id,
                    @DeletedDay = DateTime.Now,
                    @DeletedBy = DeletedBy,
                    @IsDeleted = 1
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
