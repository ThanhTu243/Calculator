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
    public class CrudScoreController : ControllerBase
    {

        private readonly ILogger<ApiScoreController> _logger;
        private readonly IScoreService _scoreService;
        private readonly ICacheHelper _cacheHelper;

        public CrudScoreController(IScoreService scoreService, ILogger<ApiScoreController> logger, ICacheHelper cacheHelper)
        {
            _logger = logger;
            _scoreService = scoreService;
            _cacheHelper = cacheHelper;
        }
        [HttpPost("CreateUniversity")]
        public async Task<IActionResult> CreateUniversity(University us)
        {
            try
            {
                int result = await _scoreService.CreateUniversity(us.Code,us.Name,us.Type,us.Province,us.Address,us.CreatedBy);

                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("UpdateUniversity")]
        public async Task<IActionResult> UpdateUniversity(University us)
        {
            try
            {
                int result = await _scoreService.UpdateUniversity(us.Id,us.Code, us.Name, us.Type, us.Province,us.Address, us.UpdatedBy);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("DeleteUniversity")]
        public async Task<IActionResult> DeleteUniversity(University us)
        {
            try
            {
                int result = await _scoreService.DeleteUniversity(us.Id, us.DeletedBy);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("CreateGroupMajor")]
        public async Task<IActionResult> CreateGroupMajor(GroupMajor gm)
        {
            try
            {
                int result = await _scoreService.CreateGroupMajor(gm.NameGroupMajor,gm.CreatedBy);

                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("UpdateGroupMajor")]
        public async Task<IActionResult> UpdateGroupMajor(GroupMajor gm)
        {
            try
            {
                int result = await _scoreService.UpdateGroupMajor(gm.IdGroupMajor, gm.NameGroupMajor, gm.UpdatedBy);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("DeleteGroupMajor")]
        public async Task<IActionResult> DeleteGroupMajor(GroupMajor gm)
        {
            try
            {
                int result = await _scoreService.DeleteGroupMajor(gm.IdGroupMajor, gm.DeletedBy);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("CreateMajor")]
        public async Task<IActionResult> CreateMajor(Major major)
        {
            try
            {
                int result = await _scoreService.CreateMajor(major.CodeMajor,major.NameMajor,major.Hot,major.CreatedBy,major.IdGroupMajor);

                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("UpdateMajor")]
        public async Task<IActionResult> UpdateMajor(Major major)
        {
            try
            {
                int result = await _scoreService.UpdateMajor(major.IdMajor,major.CodeMajor,major.NameMajor,major.Hot,major.UpdatedBy,major.IdGroupMajor);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("DeleteMajor")]
        public async Task<IActionResult> DeleteMajor(Major major)
        {
            try
            {
                int result = await _scoreService.DeleteMajor(major.IdMajor,major.DeletedBy);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("CreateCombination")]
        public async Task<IActionResult> CreateCombination(Combination combination)
        {
            try
            {
                int result = await _scoreService.CreateCombination(combination.CodeCombination, combination.NameCombination, combination.CreatedBy);

                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("UpdateCombination")]
        public async Task<IActionResult> UpdateCombination(Combination combination)
        {
            try
            {
                int result = await _scoreService.UpdateCombination(combination.IdCombination, combination.CodeCombination, combination.NameCombination, combination.UpdatedBy);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("DeleteCombination")]
        public async Task<IActionResult> DeleteCombination(Combination combination)
        {
            try
            {
                int result = await _scoreService.DeleteCombination(combination.IdCombination, combination.DeletedBy);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("CreateScore")]
        public async Task<IActionResult> CreateScore(University_Score us)
        {
            try
            {
                int result = await _scoreService.CreateScore(us.CodeMajor,us.NameMajor,us.CodeCombination,us.Score,us.Description,us.Year,us.CodeUniversity,us.CreatedBy);

                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("UpdateScore")]
        public async Task<IActionResult> UpdateScore(University_Score us)
        {
            try
            {
                int result = await _scoreService.UpdateScore(us.IdSocre,us.CodeMajor, us.NameMajor, us.CodeCombination, us.Score, us.Description, us.Year, us.CodeUniversity, us.UpdatedBy);

                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("DeleteScore")]
        public async Task<IActionResult> DeleteScore(University_Score us)
        {
            try
            {
                int result = await _scoreService.DeleteScore(us.IdSocre, us.DeletedBy);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("CreateMajor_Combination")]
        public async Task<IActionResult> CreateMajor_Combination(Major_Combination mc)
        {
            try
            {
                int result = await _scoreService.CreateMajor_Combination(mc.CodeMajor,mc.CodeCombination,mc.CreatedBy);

                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("UpdateMajor_Combination")]
        public async Task<IActionResult> UpdateMajor_Combination(Major_Combination mc)
        {
            try
            {
                int result = await _scoreService.UpdateMajor_Combination(mc.IdMajor_Combination, mc.CodeMajor, mc.CodeCombination, mc.UpdatedBy);

                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPatch("DeleteMajor_Combination")]
        public async Task<IActionResult> DeleteMajor_Combination(Major_Combination mc)
        {
            try
            {
                int result = await _scoreService.DeleteMajor_Combination(mc.IdMajor_Combination, mc.DeletedBy);
                if (result == -1)
                {
                    return Ok(new BaseResponse<object>(" ", "data not found"));
                }
                return Ok(new BaseResponse<object>(result));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
