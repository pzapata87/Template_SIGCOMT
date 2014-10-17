using System.Web.Mvc;
using SIGCOMT.Web.Core;

namespace SIGCOMT.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            return View("Error");
        }
    }
}