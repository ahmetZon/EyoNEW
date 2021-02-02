using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class PermissionController : Controller
    {
        IPermissionService _IPermissionService;
        public PermissionController(IPermissionService _IPermissionService)
        {
            this._IPermissionService = _IPermissionService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _IPermissionService.Where(null, true, false, o => o.Role).Result;
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
            var result = _IPermissionService.Where().Result.Select(o => new { value = o.Id, text = o.Name }).ToList();
            return Json(result);
        }

        public IActionResult InsertOrUpdate(Permission postModel)
        {
            var result = _IPermissionService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public Permission Get(int id)
        {
            var result = _IPermissionService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _IPermissionService.Delete(id);
            _IPermissionService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "Permission";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
