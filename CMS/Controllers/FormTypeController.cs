using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class FormTypeController : Controller
    {
        IFormTypeService _IFormTypeService;
        public FormTypeController(IFormTypeService _IFormTypeService)
        {
            this._IFormTypeService = _IFormTypeService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _IFormTypeService.Where().Result;
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
            var result = _IFormTypeService.Where().Result.Select(o => new { value = o.Id, text = o.Name }).ToList();
            return Json(result);
        }

        public IActionResult InsertOrUpdate(FormType postModel)
        {
            var result = _IFormTypeService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public FormType Get(int id)
        {
            var result = _IFormTypeService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _IFormTypeService.Delete(id);
            _IFormTypeService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "FormType";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
