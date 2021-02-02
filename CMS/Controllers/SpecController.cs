using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;


namespace CMS.Controllers
{
    public class SpecController : Controller
    {
        ISpecService _ISpecService;
        public SpecController(ISpecService _ISpecService)
        {
            this._ISpecService = _ISpecService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _ISpecService.Where(null, true, false, o => o.Parent, o => o.Orj).Result;
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
        public JsonResult GetSelect(string name, string whereCase)
        {
            if (!string.IsNullOrEmpty(whereCase))
            {
                if (whereCase == "IsTanim")
                {
                    var result = _ISpecService.Where(o => o.IsTanim == true).Result
                   .Select(o => new { value = o.Id, text = o.Name + "(" + o.SpecType.ExGetDescription() + ")" }).ToList();
                    return Json(result);
                }
                else if (whereCase.ToInt() > 0)
                {
                    var result = _ISpecService.Where(o => (whereCase.ToInt() > 0 ? o.SpecType == (SpecType)whereCase.ToInt() : true)).Result
                  .Select(o => new { value = o.Id, text = o.Name + "(" + o.SpecType.ExGetDescription() + ")" }).ToList();
                    return Json(result);

                }
                else
                {
                    var result = _ISpecService.Where().Result
                    .Select(o => new { value = o.Id, text = o.Name + "(" + o.SpecType.ExGetDescription() + ")" }).ToList();
                    return Json(result);
                }

            }
            else
            {
                var result = _ISpecService.Where().Result
                  .Select(o => new { value = o.Id, text = o.Name + "(" + o.SpecType.ExGetDescription() + ")" }).ToList();
                return Json(result);
            }
        }

        public JsonResult GetSpecType()
        {
            var list = Enum.GetValues(typeof(SpecType)).Cast<int>().Select(x => new { name = ((SpecType)x).ToStr(), value = x.ToString(), text = ((SpecType)x).ExGetDescription() }).ToArray();
            return Json(list);
        }


        public IActionResult InsertOrUpdate(Spec postModel)
        {
            var result = _ISpecService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public Spec Get(int id)
        {
            var result = _ISpecService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _ISpecService.Delete(id);
            _ISpecService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "Spec";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
