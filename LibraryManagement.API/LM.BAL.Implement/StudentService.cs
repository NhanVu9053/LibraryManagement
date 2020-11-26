using LM.BAL.Interface;
using LM.DAL.Interface;
using LM.Domain.Request.Student;
using LM.Domain.Response.Student;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LM.BAL.Implement
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        public async Task<SaveStudentRes> ChangeStatus(StatusStudentReq request)
        {
            return await studentRepository.ChangeStatus(request);
        }

        public async Task<StudentView> Get(int id)
        {
            return await studentRepository.Get(id);
        }

        public async Task<IEnumerable<StudentView>> Gets()
        {
            return await studentRepository.Gets();
        }

        public async Task<SaveStudentRes> Save(SaveStudentReq request)
        {
            return await studentRepository.Save(request);
        }
    }
}
