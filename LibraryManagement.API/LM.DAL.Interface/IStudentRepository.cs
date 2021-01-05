using LM.Domain.Request.Student;
using LM.Domain.Response.Student;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LM.DAL.Interface
{
    public interface IStudentRepository
    {
        Task<IEnumerable<StudentView>> Gets();
        Task<StudentView> Get(int id);
        Task<SaveStudentRes> Save(SaveStudentReq request);
        Task<SaveStudentRes> ChangeStatus(StatusStudentReq request);
    }
}
