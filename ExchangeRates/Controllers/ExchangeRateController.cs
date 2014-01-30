using System.Web.Mvc;
using System.Web.UI;
using ExchangeRates.Models;

namespace ExchangeRates.Controllers
{
    public class ExchangeRateController : Controller
    {
        [HttpGet]
        public ActionResult Index(ExchangeRatesVm exchangeRatesVm)
        {
            return View("Index", exchangeRatesVm);
        }

        [HttpPost]
        public ActionResult Rate(ExchangeRatesVm exchangeRatesVm)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", exchangeRatesVm);
            }

            exchangeRatesVm.GetRates();
            return View("Index", exchangeRatesVm);
        }

    }
}
