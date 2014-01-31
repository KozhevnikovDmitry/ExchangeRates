using System.Web.Mvc;
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
            if (!exchangeRatesVm.IsSuccesfull)
            {
                ModelState.AddModelError(string.Empty, exchangeRatesVm.ErrorMessage);
                exchangeRatesVm.Rates = null;
            }
            return View("Index", exchangeRatesVm);
        }

    }
}
