using Microsoft.AspNetCore.Mvc;

namespace Student_management.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult DeleteError(string entityName, int id, string returnController)
        {
            ViewBag.EntityName = entityName;
            ViewBag.Id = id;
            ViewBag.Controller = returnController;
            return View();
        }
    }

}
