using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace CMS.Controllers
{
    public class SpecContentValueController : Controller
    {
        ISpecContentValueService _ISpecContentValueService;
        public SpecContentValueController(ISpecContentValueService _ISpecContentValueService)
        {
            this._ISpecContentValueService = _ISpecContentValueService;
        }

        public ActionResult GetPaging([DataSourceRequest] DataSourceRequest request)
        {
            var orders = _ISpecContentValueService.Where().Result;
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

        public IActionResult InsertOrUpdateBulk(List<SpecContentValue> postModel)
        {
            List<RModel<SpecContentValue>> insertAll = new List<RModel<SpecContentValue>>();
            postModel.ForEach(o =>
            {
                var row = _ISpecContentValueService.Where(oo => oo.ContentPageId == o.ContentPageId && oo.SpecId == o.SpecId);
                if (row.ResultType.RType == RType.OK)
                {
                    var rowItem = row.Result.FirstOrDefault();
                    if (rowItem != null)
                    {
                        if (!string.IsNullOrEmpty(rowItem.ContentValue))
                        {
                            rowItem.ContentValue = o.ContentValue;
                            var rowResult = _ISpecContentValueService.Update(rowItem);
                            var res = _ISpecContentValueService.SaveChanges();
                        }
                        else
                        {
                            var rowResult = _ISpecContentValueService.Delete(rowItem);
                            var res = _ISpecContentValueService.SaveChanges();
                        }

                    }
                    else
                    {
                        var res = _ISpecContentValueService.InsertOrUpdate(o);
                        insertAll.Add(res);
                    }

                }
                else
                {

                }


            });
            return Json(insertAll);
        }

        public IActionResult InsertOrUpdate(SpecContentValue postModel)
        {
            var result = _ISpecContentValueService.InsertOrUpdate(postModel);
            return Json(result);
        }

        public SpecContentValue Get(int id)
        {
            var result = _ISpecContentValueService.Find(id);
            return (result);
        }

        public IActionResult Delete(int id)
        {
            var result = _ISpecContentValueService.Delete(id);
            _ISpecContentValueService.SaveChanges();
            return Json(result);
        }

        public IActionResult Index()
        {
            ViewBag.pageTitle = "SpecContentValue";
            return View();
        }

        public IActionResult InsertOrUpdatePage()
        {
            ViewBag.postModel = Get(Request.Query["id"].ToInt());
            return View();
        }


    }
}
