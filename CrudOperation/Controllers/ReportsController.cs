using CrudOperation.Migrations;
using CrudOperation.Models;
using CrudOperation.Reprositry;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace CrudOperation.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReprositryReports reports;

        public IToastMessage ToastMessage { get; }
        public IToastNotification Toasnoty { get; }

        public ReportsController(IReprositryReports Reports , IToastNotification _Toasnoty) 
        {
            reports = Reports;
            Toasnoty = _Toasnoty;
        }
        public async Task<IActionResult> Index()
        {
            return View( reports.GetAll());
        }
        public IActionResult Delete(int id) 
        {
            reports.Delete(id);
            reports.Save();
            return Ok();
        }
        public IActionResult Reports()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Reports( Models.Reports Vm)
        {
            if(ModelState.IsValid==true)
            {
                reports.Create(Vm);
                reports.Save();
                Toasnoty.AddSuccessToastMessage("Reoprts approved");
                return RedirectToAction("Index","Home");
            }
            else
            {
                return View(Vm);
            }
        }
    }
}
