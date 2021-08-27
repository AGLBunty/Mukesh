using SMAS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace SMAS.Controllers
{
    public class EmployeesApiController : ApiController
    {
        private readonly IEmployeeRepository _iEmployeeRepository = new EmployeeRepository();

        [HttpGet]
        [Route("api/Employees/Get")]
        public async Task<IEnumerable<Employee>> Get()
        {
            return await _iEmployeeRepository.GetEmployees();
        }

        [HttpPost]
        [Route("api/Employees/Create")]
        public async Task CreateAsync([FromBody]Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _iEmployeeRepository.Add(employee);
            }
        }

        [HttpGet]
        [Route("api/Employees/Details/{id}")]
        public async Task<Employee> Details(string id)
        {
            var result = await _iEmployeeRepository.GetEmployee(id);
            return result;
        }

        [HttpPut]
        [Route("api/Employees/Edit")]
        public async Task EditAsync([FromBody]Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _iEmployeeRepository.Update(employee);
            }
        }

        [HttpDelete]
        [Route("api/Employees/Delete/{id}")]
        public async Task DeleteConfirmedAsync(string id)
        {
            await _iEmployeeRepository.Delete(id);
        }


        // add 26-08-2021

        [HttpPost]
        [Route("api/tblFeedbacks/Create")]
        public async Task<tblFeedback_Form_Reuturn> CreateFeedback([FromBody]tblFeedback_Form tblFeedback_Form)
        {
            //if (ModelState.IsValid)
           // {
              var result=  await _iEmployeeRepository.SaveFeedback(tblFeedback_Form);

                return result;

           // }
        }

        [HttpGet]
        [Route("api/tblFeedbacks/GetDetails")]
        public async Task<dynamic> GetAllFeedback(GetDetailsModel model)
        {
            return await _iEmployeeRepository.GetAllFeedback(model.Dealercode);
        }
    }
    
}