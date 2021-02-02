using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace CMS.Controllers
{
    public class UserController : Controller
    {
        IUserService _IUserService;
        public UserController(IUserService _IUserService)
        {
            this._IUserService = _IUserService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _IUserService.Where().Result;
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


        public IActionResult InsertOrUpdate(User postModel)
        {
            var result = _IUserService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public User Get(int id)
        {
            var result = _IUserService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _IUserService.Delete(id);
            _IUserService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "User";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
