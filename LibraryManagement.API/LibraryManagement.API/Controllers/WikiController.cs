using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.BAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{   
    [ApiController]
    public class WikiController : ControllerBase
    {
        private readonly IWikiService wikiService;

        public WikiController(IWikiService wikiService)
        {
            this.wikiService = wikiService;
        }
        [HttpGet("api/wiki/status/{id},{isUpdate}")]
        public async Task<OkObjectResult> GetStatus(int id, bool isUpdate)
        {
            return Ok(await wikiService.GetStatus(id, isUpdate));
        }
    }
}
