using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class SpecAttrController : Controller
    {
        ISpecAttrService _ISpecAttrService;
        public SpecAttrController(ISpecAttrService _ISpecAttrService)
        {
            this._ISpecAttrService = _ISpecAttrService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _ISpecAttrService.Where(null, true, false, o => o.Spec).Result;
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


        public IActionResult InsertOrUpdate(SpecAttr postModel)
        {
            var result = _ISpecAttrService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public SpecAttr Get(int id)
        {
            var result = _ISpecAttrService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _ISpecAttrService.Delete(id);
            _ISpecAttrService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "SpecAttr";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
