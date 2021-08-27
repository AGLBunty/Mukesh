using SMAS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;

namespace SMAS.Controllers
{

    [Authorize]
    public class EmployeesController : Controller
    {
        readonly string apiBaseAddress = ConfigurationManager.AppSettings["apiBaseAddress"];
        public async Task<ActionResult> Index(string sortOrder ,  string Filter_Value, int? Page_No, string Search_Data)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (!string.IsNullOrEmpty(Search_Data))
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;
            IEnumerable<Employee> employees = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);
                var result = await client.GetAsync("employees/get");
                if (result.IsSuccessStatusCode)
                {
                    employees = await result.Content.ReadAsAsync<IList<Employee>>();
                    if (!String.IsNullOrEmpty(Search_Data))
                    {
                        employees = employees.Where(stu => stu.Name.ToUpper().Contains(Search_Data.ToUpper())
                                    || stu.Company.ToUpper().Contains(Search_Data.ToUpper()));
                    }
                    switch (sortOrder)
                    {
                        case "name_desc":
                            employees = employees.OrderByDescending(s => s.Name);
                            break;
                        default:
                            employees = employees.OrderBy(s => s.Name);
                            break;
                    }


                }
                else
                {
                    employees = Enumerable.Empty<Employee>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            int Size_Of_Page = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
            int No_Of_Page = (Page_No ?? 1);
            return View(employees.ToPagedList(No_Of_Page, Size_Of_Page));

            //return Json(employees.ToPagedList(No_Of_Page, Size_Of_Page), JsonRequestBehavior.AllowGet);
            //return View(employees);
        }

        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                var result = await client.GetAsync($"employees/details/{id}");

                if (result.IsSuccessStatusCode)
                {
                    employee = await result.Content.ReadAsAsync<Employee>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Address,Gender,Company,Designation")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseAddress);

                    var response = await client.PostAsJsonAsync("employees/Create", employee);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View(employee);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                var result = await client.GetAsync($"employees/details/{id}");

                if (result.IsSuccessStatusCode)
                {
                    employee = await result.Content.ReadAsAsync<Employee>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Address,Gender,Company,Designation")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseAddress);
                    var response = await client.PutAsJsonAsync("employees/edit", employee);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                var result = await client.GetAsync($"employees/details/{id}");

                if (result.IsSuccessStatusCode)
                {
                    employee = await result.Content.ReadAsAsync<Employee>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                var response = await client.DeleteAsync($"employees/delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View();
        }

        public ActionResult ManageUsers(string UserTypeId)
        {
            ViewBag.UserTypeId = UserTypeId;
            return View();
        }

        public async Task<ActionResult> UserListPartial(string UserId, string UserTypeId, string SearchText, int? PageNumber, int? PageSize, string SortBy)
        {

            IEnumerable<Employee> employees = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);
                var result = await client.GetAsync("employees/get");
                if (result.IsSuccessStatusCode)
                {
                    employees = await result.Content.ReadAsAsync<IList<Employee>>();
                }
                    //    if (!String.IsNullOrEmpty(Search_Data))
                    //    {
                    //        employees = employees.Where(stu => stu.Name.ToUpper().Contains(Search_Data.ToUpper())
                    //                    || stu.Company.ToUpper().Contains(Search_Data.ToUpper()));
                    //    }
                    //    switch (sortOrder)
                    //    {
                    //        case "name_desc":
                    //            employees = employees.OrderByDescending(s => s.Name);
                    //            break;
                    //        default:
                    //            employees = employees.OrderBy(s => s.Name);
                    //            break;
                    //    }


                    //}
                }


            //    IList<UserDetailsResult> resultlist = new List<UserDetailsResult>();
            //OrionReturnsClient client = new OrionReturnsClient();
           // resultlist = client.GetUserDetails(UserId, UserTypeId, SearchText, PageNumber, PageSize, SortBy);
            if (employees.Count() > 0)
            {
                ViewBag.TotalRecord = employees.Count();

            }
            ViewBag.PageNo = PageNumber ?? 1;
            ViewBag.PageSize = PageSize ?? 5;

            int Size_Of_Page = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
            int No_Of_Page = (PageNumber ?? 1);
            // return View(employees.ToPagedList(No_Of_Page, Size_Of_Page));
           // employees= 

            return PartialView("~/Views/Employees/_ManageUsers.cshtml", employees);

        }


    }
}
