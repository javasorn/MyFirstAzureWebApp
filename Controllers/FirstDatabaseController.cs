using Entity;
using Microsoft.AspNetCore.Mvc;
using MyFirstAzureWebApp.Business.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstDatabaseController : Controller
    {
        private readonly IEmployees employees;
        public FirstDatabaseController(IEmployees emp)
        {
            employees = emp;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [Route("NewEmployee")]
        [HttpPost]
        public async Task<IActionResult> NewEmployee(Employee emp)
        {
            await employees.New(emp);
            return Ok();
        }
        [Route("ResignsEmployee")]
        [HttpPost]
        public async Task<IActionResult> ResignsEmployee(Employee emp)
        {
            await employees.Resigns(emp);
            return Ok();
        }
    }
}
