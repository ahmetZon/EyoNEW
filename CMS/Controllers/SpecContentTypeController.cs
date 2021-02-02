using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class SpecContentTypeController : Controller
    {
        ISpecContentTypeService _ISpecContentTypeService;
        public SpecContentTypeController(ISpecContentTypeService _ISpecContentTypeService)
        {
            this._ISpecContentTypeService = _ISpecContentTypeService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _ISpecContentTypeService.Where(null, true, false, o => o.Spec, o => o.ContentTypes).Result;
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


        public IActionResult InsertOrUpdate(SpecContentType postModel)
        {
            var result = _ISpecContentTypeService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public SpecContentType Get(int id)
        {
            var result = _ISpecContentTypeService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _ISpecContentTypeService.Delete(id);
            _ISpecContentTypeService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "SpecContentType";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
