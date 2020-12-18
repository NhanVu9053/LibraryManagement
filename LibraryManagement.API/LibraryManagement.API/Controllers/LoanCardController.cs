using LM.BAL.Interface;
using LM.Domain.Request.LoanCard;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            var loanCards = await loanCardService.Gets();
            return Ok(loanCards);
        }
        [HttpGet("api/loanCard/get/{id}")]
        public async Task<OkObjectResult> Get(int id)
        {
            var loanCard = await loanCardService.Get(id);
            return Ok(loanCard);
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
        [HttpPatch]
        [Route("api/loanCard/extendLoanCard")]
        public async Task<OkObjectResult> ExtendLoanCard(ExtendLoanCardReq request)
        {
            var result = await loanCardService.ExtendLoanCard(request);
            return Ok(result);
        }
    }
}
