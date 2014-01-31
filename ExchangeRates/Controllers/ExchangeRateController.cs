using System.Web.Mvc;
using ExchangeRates.Models;

namespace ExchangeRates.Controllers
{
    /// <summary>
    /// Controller, that provide exchange rates reports
    /// </summary>
    public class ExchangeRateController : Controller
    {
        /// <summary>
        /// Shows form for exchange rates request
        /// </summary>
        [HttpGet]
        public ActionResult Index(ExchangeRatesVm exchangeRatesVm)
        {
            return View("Index", exchangeRatesVm);
        }

        /// <summary>
        /// Shows exchange rates report
        /// </summary>
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
            }

            return View("Index", exchangeRatesVm);
        }

    }
}
