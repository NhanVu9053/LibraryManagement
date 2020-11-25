﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.BAL.Interface;
using LM.Domain.Request.LoanCard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class LoanCardController : ControllerBase
    {
        private readonly ILoanCardService loanCardService;

        public LoanCardController(ILoanCardService loanCardService)
        {
            this.loanCardService = loanCardService;
        }
        [HttpGet("api/loanCard/gets")]
        public async Task<OkObjectResult> Gets()
        {
            var books = await loanCardService.Gets();
            return Ok(books);
        }
        [HttpGet("api/loanCard/get/{id}")]
        public async Task<OkObjectResult> Get(int id)
        {
            var book = await loanCardService.Get(id);
            return Ok(book);
        }
        [HttpPost, HttpPatch]
        [Route("api/loanCard/save")]
        public async Task<OkObjectResult> SaveCourse(SaveLoanCardReq request)
        {
            var result = await loanCardService.Save(request);
            return Ok(result);
        }
        [HttpPatch]
        [Route("api/loanCard/changeStatus")]
        public async Task<OkObjectResult> ChangeStatus(StatusLoanCardReq request)
        {
            var result = await loanCardService.ChangeStatus(request);
            return Ok(result);
        }
    }
}
