using System.Collections.Generic;
using System.Threading.Tasks;
using SMAS.Models.List;

namespace SMAS.Models
{
    public interface IEmployeeRepository
    {
        Task Add(Employee employee);
        Task Update(Employee employee);
        Task Delete(string ID);
        Task<Employee> GetEmployee(string id);
        Task<IEnumerable<Employee>> GetEmployees();
        // add 26-08-2021
        Task<tblFeedback_Form_Reuturn> SaveFeedback(tblFeedback_Form tblFeedback_Form);

        Task<dynamic> GetAllFeedback(string Dealercode);
    }
}
