using System.Web.Mvc;
using ExchangeRates.Models;

namespace ExchangeRates.Controllers
{
    public class ExchangeRateController : Controller
    {
        [HttpGet]
        public ActionResult Index(ExchangeRatesVm exchangeRatesVm)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Rate(ExchangeRatesVm exchangeRatesVm)
        {
            return View("Index");
        }

    }
}
