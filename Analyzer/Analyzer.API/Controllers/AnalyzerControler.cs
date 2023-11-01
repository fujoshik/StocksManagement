using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;

namespace Analyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzerControler : ControllerBase
    {
        private readonly IService iservice;
        public AnalyzerControler(IService iservice)
        {
            this.iservice = iservice;
        }

        [HttpGet("check-accounts")]
        public async Task<IActionResult> GetApiData(int id)
        {
            UserData data = await iservice.GetInfoFromAccount(id);
            if (data != null)
            {
                return Ok(data);
            }
            return StatusCode(500, "Woopsie Daisy! Looks like something went completely wrong. You can try again later. ;)");
        }


        //[HttpGet("get-user-by-id")]
        //public async Task<IActionResult> GetUserById(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid id.");
        //    }

        //    string data = await iservice.GetUserById(id);

        //    if (data != null)
        //    {
        //        return Ok(data);
        //    }

        //    return StatusCode(500, "Woopsie Dasy!   Looks like something went completely wrogg. You can try again later. ;)");
        //}
    }
}
