using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class FormsController : Controller
    {
        IFormsService _IFormsService;
        public FormsController(IFormsService _IFormsService)
        {
            this._IFormsService = _IFormsService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request, int selecttype)
        {
            var orders = _IFormsService.Where(o => o.FormTypeId == selecttype, true, false, o => o.FormType).Result;
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


        public IActionResult InsertOrUpdate(Forms postModel)
        {
            var result = _IFormsService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public Forms Get(int id)
        {
            var result = _IFormsService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _IFormsService.Delete(id);
            _IFormsService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "Forms";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
