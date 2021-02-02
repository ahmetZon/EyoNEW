using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class LangController : Controller
    {
        ILangService _ILangService;
        public LangController(ILangService _ILangService)
        {
            this._ILangService = _ILangService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _ILangService.Where().Result;
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
            var result = _ILangService.Where().Result.Select(o => new { value = o.Id, text = o.Name }).ToList();
            return Json(result);
        }

        public IActionResult InsertOrUpdate(Lang postModel)
        {
            var result = _ILangService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public Lang Get(int id)
        {
            var result = _ILangService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _ILangService.Delete(id);
            _ILangService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "Lang";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
