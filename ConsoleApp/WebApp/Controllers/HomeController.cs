using System.Web.Mvc;
using WebApp.Helper;
using XmlReader = WebApp.Helper.XmlReader;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private AppCache appCache;

        public HomeController()
        {
            appCache = new AppCache();
        }

        public ActionResult Index()
        {
            var result = appCache.GetValue();
            if (result == null)
            {
                result = XmlReader.GetWordModelList();
                appCache.Add(result);
            }
            return View(result);
        }
    }
}