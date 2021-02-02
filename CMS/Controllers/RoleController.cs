using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class RoleController : Controller
    {
        IRoleService _IRoleService;
        public RoleController(IRoleService _IRoleService)
        {
            this._IRoleService = _IRoleService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _IRoleService.Where().Result;
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
            var result = _IRoleService.Where().Result.Select(o => new { value = o.Id, text = o.Name }).ToList();
            return Json(result);
        }


        public IActionResult InsertOrUpdate(Role postModel)
        {
            var result = _IRoleService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public Role Get(int id)
        {
            var result = _IRoleService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _IRoleService.Delete(id);
            _IRoleService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "Role";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
