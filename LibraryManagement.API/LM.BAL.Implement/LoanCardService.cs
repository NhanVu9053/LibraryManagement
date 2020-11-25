using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Request.LoanCard;
using LM.Domain.Response.LoanCard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Implement
{
    public class LoanCardService : ILoanCardService
    {
        private readonly ILoanCardRepository loanCardRepository;

        public LoanCardService(ILoanCardRepository loanCardRepository)
        {
            this.loanCardRepository = loanCardRepository;
        }
        public async Task<SaveLoanCardRes> ChangeStatus(StatusLoanCardReq request)
        {
            return await loanCardRepository.ChangeStatus(request);
        }

        public async Task<LoanCardDetailView> Get(int id)
        {
            var loanCardDetail = new LoanCardDetailView();
            loanCardDetail.loanCard =  await loanCardRepository.Get(id);
            loanCardDetail.bookList = await loanCardRepository.GetBookList(id);
            return loanCardDetail;
        }

        public async Task<IEnumerable<LoanCardView>> Gets()
        {
            return await loanCardRepository.Gets();
        }

        public async Task<SaveLoanCardRes> Save(SaveLoanCardReq request)
        {
            return await loanCardRepository.Save(request);
        }
    }
}
