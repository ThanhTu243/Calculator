using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Service;
using Microsoft.AspNetCore.Cors;

namespace PublicApi.Controllers
{
    [EnableCors("mypolicy")]
    [ApiController]
    [Route("[controller]")]
    public class CrawlScoreController : ControllerBase
    {
       
        private readonly ILogger<CrawlScoreController> _logger;
        private readonly IScoreService _scoreService;

        public CrawlScoreController(IScoreService scoreService,ILogger<CrawlScoreController> logger)
        {
            _logger = logger;
            _scoreService = scoreService;
        }

        [HttpGet("CrawlUniversity")]
        public async Task<ActionResult> CrawlUniversity()
        {
            try
            {
                var result = await _scoreService.CrawlListUniversity();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("CrawlHrefMajor")]
        public async Task<ActionResult> CrawlHrefMajor()
        {
            try
            {
                var result = await _scoreService.CrawlListHrefMajor();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("CrawlHrefScore")]
        public async Task<ActionResult> CrawlHrefScore()
        {
            try
            {
                var result = await _scoreService.CrawlListHrefScore();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("CrawlMajor")]
        public async Task<ActionResult> CrawlMajor()
        {
            try
            {
                var result = await _scoreService.CrawlListMajor();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("CrawlCombination")]
        public async Task<ActionResult> CrawlCombination()
        {
            try
            {
                var result = await _scoreService.CrawlCombination();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("CrawlMajorAndCombination")]
        public async Task<ActionResult> CrawlMajorAndCombination()
        {
            try
            {
                var result = await _scoreService.CrawlListMajorAndCombination();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("CrawlScore")]
        public async Task<ActionResult> CrawlScore(string year)
        {
            try
            {
                var result = await _scoreService.CrawlScoreUniversity(year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
