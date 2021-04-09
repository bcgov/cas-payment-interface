using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CASInterfaceService.Pages.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace CASInterfaceService.Pages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentRetriveController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public List<Student> GetAllStudents()
        {
            return StudentRegistration.getInstance().getAllStudent();
        }
        [HttpGet("GetAllStudentRecords")]
        public JsonResult GetAllStudentRecords()
        {
            return Json(StudentRegistration.getInstance().getAllStudent());
        }
    }
}
