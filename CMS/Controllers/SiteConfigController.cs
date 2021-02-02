using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class SiteConfigController : Controller
    {
        ISiteConfigService _ISiteConfigService;
        public SiteConfigController(ISiteConfigService _ISiteConfigService)
        {
            this._ISiteConfigService = _ISiteConfigService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _ISiteConfigService.Where().Result;
            orders = orders.ApplyOrdersFiltering(request.Filters);
            var total = orders.Count();
            orders = orders.ApplyOrdersSorting(request.Groups, request.Sorts);
            if (!request.Sorts.Any() && !request.Groups.Any())
                orders = orders.OrderBy(o => o.Id);
            orders = orders.ApplyOrdersPaging(request.Page, request.PageSize);
            var data = orders.ApplyOrdersGrouping(request.Groups);
            var result = new DataSourceResult()
            {
                Data = data,
                Total = total
            };

            return Json(result);
        }




        public IActionResult InsertOrUpdate(SiteConfig postModel)
        {
            var result = _ISiteConfigService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public SiteConfig Get(int id)
        {
            var result = _ISiteConfigService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _ISiteConfigService.Delete(id);
            _ISiteConfigService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "SiteConfig";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
