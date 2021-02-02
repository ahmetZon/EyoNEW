using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Components
{

    public class DynamicInput : ViewComponent
    {

        IHttpContextAccessor _httpContextAccessor;
        IContentPageService _IContentPageService;
        IDocumentsService _IDocumentsService;
        ISpecService _ISpecService;
        ISpecAttrService _ISpecAttrService;
        ISpecContentTypeService _ISpecContentTypeService;
        ISpecContentValueService _ISpecContentValueService;

        public DynamicInput(
            IContentPageService _IContentPageService,
            IDocumentsService _IDocumentsService,
        IHttpContextAccessor httpContextAccessor,
         ISpecService _ISpecService,
        ISpecAttrService _ISpecAttrService,
        ISpecContentTypeService _ISpecContentTypeService,
        ISpecContentValueService _ISpecContentValueService
            )
        {
            this._IContentPageService = _IContentPageService;
            this._httpContextAccessor = httpContextAccessor;
            this._IDocumentsService = _IDocumentsService;
            this._ISpecService = _ISpecService;
            this._ISpecAttrService = _ISpecAttrService;
            this._ISpecContentTypeService = _ISpecContentTypeService;
            this._ISpecContentValueService = _ISpecContentValueService;

        }


        public IViewComponentResult Invoke(DynamicModel postModel)
        {
            if (postModel.PageType == "Documents")
            {
                return View("DynamicInput_Documents", postModel);
            }
            else if (postModel.PageType == "ContentPage")
            {
                return View("DynamicInput_ContentPage", postModel);
            }
            else if (postModel.PageType == "SpecDynamic")
            {
                var cp = postModel.model as ContentPage;
                var specTypes = _ISpecContentTypeService.Where(o => o.ContentTypesId == cp.ContentTypesId, true, false).Result.ToList();
                var result = new List<Spec>();
                if (specTypes.Count > 0)
                {
                    var specListIds = specTypes.Select(o => o.SpecId).ToList();
                    var spec = _ISpecService.Where(null, true, false, o =>
                    o.SpecChilds, o => o.SpecAttrs, o => o.SpecContentValue).Result.ToList();
                    result = spec.Where(o => specListIds.Contains(o.Id)).ToList();
                    result.ForEach(o =>
                    {
                        o.SpecChilds = o.SpecChilds.Count > 0 ? spec.Where(oo => oo.ParentId == o.Id).ToList() : new List<Spec>();
                    });
                }
                ViewBag.spec = result;

                return View("DynamicInput_Spec", postModel);
            }
            else
            {
                return View("DynamicInput", postModel);
            }


        }




    }
}