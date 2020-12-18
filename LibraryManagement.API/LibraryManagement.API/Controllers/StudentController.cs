using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LM.BAL.Interface;
using LM.Domain.Request.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }
        [HttpGet("api/student/gets")]
        public async Task<OkObjectResult> Gets()
        {
            var books = await studentService.Gets();
            return Ok(books);
        }
        [HttpGet("api/student/get/{id}")]
        public async Task<OkObjectResult> Get(int id)
        {
            var book = await studentService.Get(id);
            return Ok(book);
        }
        [HttpPost, HttpPatch]
        [Route("api/student/save")]
        public async Task<OkObjectResult> SaveCourse(SaveStudentReq request)
        {
            var result = await studentService.Save(request);
            return Ok(result);
        }
        [HttpPatch]
        [Route("api/student/changeStatus")]
        public async Task<OkObjectResult> ChangeStatus(StatusStudentReq request)
        {
            var result = await studentService.ChangeStatus(request);
            return Ok(result);
        }
    }
}
