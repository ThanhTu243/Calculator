using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Service;
using Microsoft.AspNetCore.Cors;
using PublicApi.Models;
using Ultilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Model.ViewModel;
using Model.BaseModel;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Net.Sockets;

namespace PublicApi.Controllers
{
    [EnableCors("mypolicy")]
    [ApiController]
    [Route("[controller]")]
    public class ApiScoreController : ControllerBase
    {

        private readonly ILogger<ApiScoreController> _logger;
        private readonly IScoreService _scoreService;
        private readonly ICacheHelper _cacheHelper;

        public ApiScoreController(IScoreService scoreService, ILogger<ApiScoreController> logger,ICacheHelper cacheHelper)
        {
            _logger = logger;
            _scoreService = scoreService;
            _cacheHelper = cacheHelper;
        }

        // GET /apiscore/getuniversity 
        /// <summary>    
        /// Lấy danh sách đại học - cao đẳng
        /// </summary>    
        /// <returns></returns>  
        [HttpGet("GetUniversity")]       
        public async Task<ActionResult> GetUniversity()
        {
            try
            {
                string key = $"university_result";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new ViewUniversity();
                        //info = JsonConvert.DeserializeObject<ViewUniversity>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetUniversity();
                
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));                  
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>    
        /// Lấy danh sách tổ hợp môn
        /// </summary>    
        /// <returns></returns>  
        [HttpGet("GetCombination")]
        public async Task<ActionResult> GetCombination()
        {
            try
            {
                string key = $"combination_result";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new Combination();
                        //info = JsonConvert.DeserializeObject<Combination>(((JObject)dataCache);
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetCombination();
                
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>    
        /// Tìm kiếm trường
        /// -----------
        /// Input : Tên trường hoặc mã trường (người dùng nhập)
        /// </summary>   

        [HttpGet("GetUniversityByCodeOrName")]
        public async Task<ActionResult> GetUniversityByCodeOrName(string keyword)
        {
            try
            {
                string key = $"university_by_keyword_result_{keyword}";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new ViewUniversity();
                        //info = JsonConvert.DeserializeObject<ViewUniversity>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetUniversityByCodeOrName(keyword);
                
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));                   
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>    
        /// Tìm kiếm trường bằng tỉnh hoặc loại trường
        /// -------------
        /// Input : Id tỉnh thành (getprovince) và Id loại trường (getuniversitytype)
        /// </summary>   
        [HttpGet("GetUniversityByTypeAndProvince")]
        public async Task<ActionResult> SearchUniversityByTypeAndProvince(int? IdType,int? IdProvince)
        {
            try
            {
                if (IdType == null)
                {
                    IdType = 0;
                }
                if (IdProvince == null)
                {
                    IdProvince = 0;
                }
                string key = $"university_by_type_province_result_{IdType}_{IdProvince}";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new ViewUniversity();
                        //info = JsonConvert.DeserializeObject<ViewUniversity>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetUniversityByTypeAndProvince(IdType,IdProvince);
                
                if (result == null || !result.Any())
                {
                   
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>    
        /// Lấy danh sách tỉnh thành
        /// </summary>   
        [HttpGet("GetProvince")]
        public async Task<ActionResult> GetProvince()
        {
            try
            {
                string key = $"province_result";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new Province();
                        //info = JsonConvert.DeserializeObject<Province>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetProvince();
                
                if (result == null || !result.Any())
                {                   
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>    
        /// Lấy danh sách loại trường
        /// </summary>   
        [HttpGet("GetUniversityType")]
        public async Task<ActionResult> GetUniversityType()
        {
            try
            {
                string key = $"university_type_result";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new University_Type();
                        //info = JsonConvert.DeserializeObject<University_Type>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetUniversityType();
                
                if (result == null || !result.Any())
                {                    
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>    
        /// Lấy điểm theo trường
        /// -------------
        /// Input : mã trường (getuniversity) và năm (từ 2017 đến 2021)
        /// </summary>   
        [HttpGet("GetScoreByUniversity")]
        public async Task<ActionResult> GetScoreByUniversity(string CodeUniversity,string Year)
        {
            try
            {
                string key = $"university_score_result_{CodeUniversity}_{Year}";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new University_Score();
                        //info = JsonConvert.DeserializeObject<University_Score>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetScoreByUniversity(CodeUniversity,Year);
               
                if (result == null || !result.Any()) 
                {                 
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("GetScoreByMajor")]
        public async Task<ActionResult> GetScoreByMajor(string CodeMajor)
        {
            try
            {
                string key = $"university_score_by_major_result_{CodeMajor}";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new University_Score();
                        //info = JsonConvert.DeserializeObject<University_Score>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetScoreByMajor(CodeMajor);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        /// <summary>    
        /// Tìm kiếm tổ hợp môn theo ngành
        /// -------------
        /// Input : mã ngành (getmajor) và năm
        /// </summary> 
        [HttpGet("GetMajorCombination")]
        public async Task<ActionResult> GetMajorCombination(string CodeMajor)
        {
            try
            {
                string key = $"major_combination_result_{CodeMajor}";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new ViewMajor_Combination();
                        //info = JsonConvert.DeserializeObject<ViewMajor_Combination>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetMajorCombination(CodeMajor);
                
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));                   
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>    
        /// Lấy danh sách mã ngành
        /// </summary> 
        [HttpGet("GetMajor")]
        public async Task<ActionResult> GetMajor()
        {
            try
            {
                string key = $"major_result";
                var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                if (dataCache != null)
                {
                    //var info = new Major();
                    //info = JsonConvert.DeserializeObject<Major>(((JObject)dataCache).ToString());
                    return Ok(new BaseResponse<object>(dataCache));
                }
                var result = await _scoreService.GetMajor();
                
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));              
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>    
        /// Hiển thị danh sách ngành theo số lượng yêu cầu
        /// --------------
        /// Input : Số lượng các item major muốn hiển thị 
        /// </summary> 
        [HttpGet("GetMajorByQuantity")]
        public async Task<ActionResult> GetMajorByQuantity(int quantity)
        {
            try
            {
                string key = $"get_major_by_{quantity}";
                var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                if (dataCache != null)
                {
                    //var info = new ViewUniversity();
                    //info = JsonConvert.DeserializeObject<ViewUniversity>(((JObject)dataCache).ToString());
                    return Ok(new BaseResponse<object>(dataCache));
                }
                var result = await _scoreService.GetMajorByQuantity(quantity);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>    
        /// Hiển thị danh sách ngành có điểm chuẩn cao nhất từng năm
        /// --------------
        /// Input : Năm mong muốn để hiển thị danh sách các ngành có điểm cao nhất
        /// </summary> 
        [HttpGet("GetTopMajorByYear")]
        public async Task<ActionResult> GetTopMajorByYear(string year,int quantity)
        {
            try
            {
                string key = $"get_top_major_by_{year}_{quantity}";
                var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                if (dataCache != null)
                {
                    //var info = new ViewUniversity();
                    //info = JsonConvert.DeserializeObject<ViewUniversity>(((JObject)dataCache).ToString());
                    return Ok(new BaseResponse<object>(dataCache));
                }
                var result = await _scoreService.GetTopMajorByYear(year,quantity);

                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>    
        /// Lấy danh sách tổ hợp ngành
        /// </summary> 
        [HttpGet("GetGroupMajor")]
        public async Task<ActionResult> GetGroupMajor()
        {
            try
            {
                string key = $"group_major_result";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new Major();
                        //info = JsonConvert.DeserializeObject<Major>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.GetGroupMajor_Major();
                
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>    
        /// Dự toán điểm theo mã ngành
        /// --------------
        /// Input : Từ điểm - Đến điểm - Mã ngành (getmajor) - Mã tổ hợp (getcombination) - Id tỉnh thành (getprovince)
        /// </summary> 
        [HttpGet("PredictScoreByCodeMajor")]
        public async Task<ActionResult> PredictScoreByCodeMajor(float FromPoint, float ToPoint, string CodeMajor, string CodeCombination, int? Province)
        {
            try
            {
                if (Province == null)
                {
                    Province = 0;
                }
                string key = $"predict_score_code_major_result_{FromPoint}_{ToPoint}_{CodeMajor}_{CodeCombination}_{Province}";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new Major();
                        //info = JsonConvert.DeserializeObject<Major>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                } 
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.PredictScoreByCodeMajor(FromPoint,ToPoint,CodeMajor,CodeCombination,Province);
               
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>    
        /// Dự toán điểm theo mã ngành
        /// --------------
        /// Input : Lấy dữ liệu input từ dự toán điểm theo ngành trừ Idprovince - Truyền thêm mã trường (CodeUniversity) từ output dự toán điểm theo ngành  
        /// </summary> 
        [HttpGet("DetailPredictScoreByCodeMajor")]
        public async Task<ActionResult> DetailPredictScoreByCodeMajor(float FromPoint, float ToPoint,string CodeUniversity, string CodeMajor, string CodeCombination)
        {
            try
            {
                string key = $"detail_predict_score_code_major_result_{FromPoint}_{ToPoint}_{CodeMajor}_{CodeCombination}_{CodeUniversity}";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new Major();
                        //info = JsonConvert.DeserializeObject<Major>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.DetailPredictScoreByCodeMajor(FromPoint, ToPoint, CodeUniversity, CodeMajor, CodeCombination);
                
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>    
        /// Dự toán điểm theo mã tổ hợp ngành
        /// --------------
        /// Input : Từ điểm - Đến điểm - Mã tổ hợp ngành (getgroupmajor) - Mã tổ hợp (getcombination) - Id tỉnh thành (getprovince)
        /// </summary> 
        [HttpGet("PredictScoreByGroupMajor")]
        public async Task<ActionResult> PredictScoreByGroupMajor(float FromPoint, float ToPoint, int GroupMajor, string CodeCombination, int? Province)
        {
            try
            {
                if (Province == null)
                {
                    Province = 0;
                }
                string key = $"predict_score_group_major_result_{FromPoint}_{ToPoint}_{GroupMajor}_{CodeCombination}_{Province}";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new Major();
                        //info = JsonConvert.DeserializeObject<Major>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.PredictScoreByGroupMajor(FromPoint, ToPoint, GroupMajor, CodeCombination, Province);
                
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>    
        /// Dự toán điểm theo mã tổ hợp ngành
        /// --------------
        /// Input : Lấy dữ liệu input từ dự toán điểm theo tổ hợp ngành trừ Idprovince - Truyền thêm mã trường (CodeUniversity) từ output dự toán điểm theo tổ hợp ngành  
        /// </summary> 
        [HttpGet("DetailPredictScoreByGroupMajor")]
        public async Task<ActionResult> DetailPredictScoreByGroupMajor(float FromPoint, float ToPoint, string CodeUniversity, int GroupMajor, string CodeCombination)
        {
            try
            {
                string key = $"detail_predict_score_group_major_result_{FromPoint}_{ToPoint}_{GroupMajor}_{CodeCombination}_{CodeUniversity}";
                try
                {
                    var dataCache = await _cacheHelper.GetAsync<dynamic>(key);
                    if (dataCache != null)
                    {
                        //var info = new Major();
                        //info = JsonConvert.DeserializeObject<Major>(((JObject)dataCache).ToString());
                        return Ok(new BaseResponse<object>(dataCache));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                var result = await _scoreService.DetailPredictScoreByGroupMajor(FromPoint, ToPoint, CodeUniversity, GroupMajor, CodeCombination);
               
                if (result == null || !result.Any())
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                await _cacheHelper.SetAsync(key, result, TimeSpan.FromDays(1));
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        

        [HttpGet("TestHTML")]
        public async Task<string> TestHTML()
        {
            return PartialViewResult();
        }

        private string PartialViewResult()
        {
            string a = Request.Headers["User-Agent"].ToString();
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString() + " + " + a;
                }
            }
            return a;
        }

    }
}
