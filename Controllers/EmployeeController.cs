using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TestApp.DAL;
using TestApp.Models;
using TestApp.Models.DBEntities;

namespace TestApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _context;
        public EmployeeController(EmployeeDbContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    var EmployeeViewModel = new EmployeeViewModel()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Address = employee.Address,
                        GrossSalary = employee.GrossSalary,
                        Position = employee.Position
                    };
                    employeeList.Add(EmployeeViewModel);
                }
                return View(employeeList);
            }
            return View(employeeList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employee = new Employee()
                    {
                        FirstName = employeeData.FirstName,
                        LastName = employeeData.LastName,
                        Address = employeeData.Address,
                        GrossSalary = employeeData.GrossSalary,
                        Position = employeeData.Position
                    };
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Employee created succesfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["erorrMessage"] = "Modal is not valid!!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                   TempData["erorrMessage"] = "Modal is not valid!!";

                    return View();
            }
        }
        public IActionResult ExportToCSV()
        {
            var builder = new StringBuilder();
            builder.AppendLine("FirstName,LastName,Address,GrossSalary,Position");
            foreach (var employee in _context.Employees)
            {
                builder.AppendLine($"{employee.FirstName},{employee.LastName},{employee.Address},{employee.GrossSalary},{employee.Position}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "employee.csv");
        }
        public IActionResult ExportToExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employee");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "FirstName";
                worksheet.Cell(currentRow, 2).Value = "LastName";
                worksheet.Cell(currentRow, 3).Value = "Address";
                worksheet.Cell(currentRow, 4).Value = "GrossSalary";
                worksheet.Cell(currentRow, 5).Value = "Position";
                foreach (var employee in _context.Employees)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = employee.FirstName;
                    worksheet.Cell(currentRow, 2).Value = employee.LastName;
                    worksheet.Cell(currentRow, 3).Value = employee.Address;
                    worksheet.Cell(currentRow, 4).Value = employee.GrossSalary;
                    worksheet.Cell(currentRow, 5).Value = employee.Position;
                }
                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.speadsheetml.sheet", "employee.xlsx");
                }
            }
        }
            [HttpGet]
            public IActionResult View (int id)
            {
            try
            {
                var employee = _context.Employees.SingleOrDefault(x => x.Id == id);

                if (employee != null)
                {
                    var employeeView = new EmployeeViewModel()
                    {
                        Id = id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Address = employee.Address,
                        GrossSalary = employee.GrossSalary,
                        Position = employee.Position
                    };
                    return View(employeeView);
                }
                else
                {
                    TempData["errorMessage"] = $"Employee details not available with the ID:{id}";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                    return RedirectToAction("Index");
            }
       }
    }
}
