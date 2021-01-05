using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Request.User;
using LM.Domain.Response.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.BAL.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<SaveUserRes> Login(LoginReq request)
        {
            return await userRepository.Login(request);
        }

        public async Task<SaveUserRes> Create(SaveUserReq request)
        {
            return await userRepository.Create(request);
        }

        public async Task<UserView> Get(string id)
        {
            return await userRepository.Get(id);
        }

        public async Task<IEnumerable<UserView>> Gets()
        {
            return await userRepository.Gets();
        }

        public async Task<SaveUserRes> Edit(SaveUserReq request)
        {
            return await userRepository.Edit(request);
        }

        public async Task<SaveUserRes> ChangeStatus(StatusUserReq request)
        {
            return await userRepository.ChangeStatus(request);
        }
    }
}
