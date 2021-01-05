using LM.Domain.Request.LoanCard;
using LM.Domain.Response.LoanCard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.BAL.Interface
{
    public interface ILoanCardService
    {
        Task<IEnumerable<LoanCardView>> Gets();
        Task<LoanCardDetailView> Get(int id);
        Task<SaveLoanCardRes> Save(SaveLoanCardReq request);
        Task<SaveLoanCardRes> ChangeStatus(StatusLoanCardReq request);
        Task<SaveLoanCardRes> ExtendLoanCard(ExtendLoanCardReq request);
    }
}
