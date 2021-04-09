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
    public class StudentDeleteController : Controller
    {
        [Route("student/remove/{regdNum}")]
        // DELETE: api/<controller>
        [HttpDelete]
        public IActionResult DeleteStudentRecord(String regdNum)
        {
            Console.WriteLine("In deleteStudentRecord");
            return Ok(StudentRegistration.getInstance().Remove(regdNum));
        }
    }
}
