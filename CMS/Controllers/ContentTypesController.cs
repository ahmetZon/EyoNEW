using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class ContentTypesController : Controller
    {
        IContentTypesService _IContentTypesService;
        public ContentTypesController(IContentTypesService _IContentTypesService)
        {
            this._IContentTypesService = _IContentTypesService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _IContentTypesService.Where().Result;
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


        [HttpPost]
        public JsonResult GetSelect()
        {
            var result = _IContentTypesService.Where().Result.Select(o => new { value = o.Id, text = o.Name }).ToList();
            return Json(result);
        }

        public IActionResult InsertOrUpdate(ContentTypes postModel)
        {
            var result = _IContentTypesService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public ContentTypes Get(int id)
        {
            var result = _IContentTypesService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _IContentTypesService.Delete(id);
            _IContentTypesService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "ContentTypes";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
