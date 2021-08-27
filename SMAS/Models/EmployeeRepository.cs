using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SMAS.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SqlDbContext db = new SqlDbContext();
        public async Task Add(Employee employee)
        {
            employee.Id = Guid.NewGuid().ToString();
            db.Employees.Add(employee);
            try
            {
                await db.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<Employee> GetEmployee(string id)
        {
            try
            {
                Employee employee = await db.Employees.FindAsync(id);
                if (employee == null)
                {
                    return null;
                }
                return employee;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            try
            {
                var employees = await db.Employees.ToListAsync();
                return employees.AsQueryable();
            }
            catch
            {
                throw;
            }
        }
        public async Task Update(Employee employee)
        {
            try
            {
                db.Entry(employee).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task Delete(string id)
        {
            try
            {
                Employee employee = await db.Employees.FindAsync(id);
                db.Employees.Remove(employee);
                await db.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        private bool EmployeeExists(string id)
        {
            return db.Employees.Count(e => e.Id == id) > 0;
        }


        // add 26-08-2021
        public async Task<tblFeedback_Form_Reuturn> SaveFeedback(tblFeedback_Form tblFeedback_Form)
        
        {
            tblFeedback_Form_Reuturn tblFeedback_Form_Reuturn = new tblFeedback_Form_Reuturn();

            tblFeedback_Form.CreatedOn = Convert.ToDateTime(DateTime.Now);
            //tblFeedback_Form.UpdatedOn = Convert.ToDateTime(DateTime.Now);
            tblFeedback_Form.Createdby = tblFeedback_Form.DealerCode;
            tblFeedback_Form.Status = false;
           // tblFeedback_Form.Updatedby = "abc";
            db.tblFeedback_Form.Add(tblFeedback_Form);

           
            try
            {
                await db.SaveChangesAsync();
                tblFeedback_Form_Reuturn.code = "200";
                tblFeedback_Form_Reuturn.message = "Success";
                tblFeedback_Form_Reuturn.result = "Success";

            }
            catch(Exception ex)
            {
                throw;
            }

            return tblFeedback_Form_Reuturn;
        }

        public async Task<dynamic> GetAllFeedback(string Dealercode)
        {
            List<tblFeedback_Form> Feedback_Form = new List<tblFeedback_Form>();
            try
            {
                if (Dealercode == null || Dealercode=="")
                {
                    Feedback_Form = await db.tblFeedback_Form.ToListAsync();
                }
                else
                {
                   Feedback_Form = await db.tblFeedback_Form.Where(a => a.DealerCode == Dealercode).ToListAsync();
                }
                if (Feedback_Form == null)
                {
                    return null;
                }
                
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return Feedback_Form;
        }
    }
}