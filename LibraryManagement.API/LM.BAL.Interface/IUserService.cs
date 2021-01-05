using LM.Domain.Request.User;
using LM.Domain.Response.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.BAL.Interface
{
    public interface IUserService
    {
        Task<SaveUserRes> Login(LoginReq request);
        Task<UserView> Get(string id);
        Task<IEnumerable<UserView>> Gets();
        Task<SaveUserRes> Create(SaveUserReq request);
        Task<SaveUserRes> Edit(SaveUserReq request);
        Task<SaveUserRes> ChangeStatus(StatusUserReq request);
    }
}
