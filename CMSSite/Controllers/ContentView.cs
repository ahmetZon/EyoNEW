using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CMSSite.Components
{

    public class ContentView : ViewComponent
    {

        IHttpContextAccessor _httpContextAccessor;
        IContentPageService _IContentPageService;
        ISpecService _ISpecService;
        IDocumentsService _IDocumentsService;
        public ContentView(
            IContentPageService _IContentPageService,
            ISpecService _ISpecService,
        IDocumentsService _IDocumentsService,
        IHttpContextAccessor httpContextAccessor
            )
        {
            this._IContentPageService = _IContentPageService;
            this._ISpecService = _ISpecService;
            this._httpContextAccessor = httpContextAccessor;
            this._IDocumentsService = _IDocumentsService;
        }

        public IViewComponentResult Invoke(TemplateType TemplateType)
        {
            var link = HttpContext.Request.Path.Value.Trim('/').ToStr();

            List<ContentPage> contentPages = new List<ContentPage>();
            var content = _IContentPageService.Where(o => o.Link.ToLower() == link.ToLower(), true, false, o => o.ContentPageChilds, o => o.Parent, o => o.Gallery, o => o.Documents, o => o.ThumbImage, o => o.Picture, o => o.BannerImage, o => o.SpecContentValue).Result.FirstOrDefault();
            var Specs = _ISpecService.Where(null, true, false, o=>o.Parent).Result.ToList();
            ViewBag.Specs = Specs;
            int langID = content != null ? content.LangId : 1;
            if (content != null && _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") != content.LangId)
            {
                _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", content.LangId);
                langID = content.LangId;
            }
            var currState = _httpContextAccessor.HttpContext.Session.GetString("currState");

            ViewBag.currState = currState;
            bool isBayi = false;
            bool isEndustri = false;
            bool isMimar = false;
            bool isBireysel = false;
            switch (currState)
            {
                case "Bayi":
                    isBayi = true;
                    break;
                case "Endustri":
                    isEndustri = true;
                    break;
                case "Mimar":
                    isMimar = true;
                    break;
                case "Bireysel":
                    isBireysel = true;
                    break;
                default:
                    isBayi = true;
                    break;
            }



            contentPages = _IContentPageService.Where(x => x.LangId == langID && x.IsDeleted == null && (x.IsBayi == isBayi || x.IsEndustri == isEndustri || x.IsMimar == isMimar || x.IsBireysel == isBireysel) && x.IsInteral == true, true, false, o => o.ContentPageChilds,o=>o.SpecContentValue, o => o.Parent, o => o.Gallery, o => o.Documents, o => o.ThumbImage, o => o.Picture, o => o.BannerImage).Result.ToList();
           // contentPages = _IContentPageService.Where(o => o.LangId == langID, true, false, o => o.ContentPageChilds, o => o.Parent, o => o.Gallery, o => o.Documents, o => o.ThumbImage, o => o.Picture, o => o.BannerImage,o=>o.SpecContentValue).Result.OrderBy(o => o.ContentOrderNo).ToList();

            _httpContextAccessor.HttpContext.Session.Set("contentPages", contentPages.Where(o => o.LangId == langID));

        
          
            ViewBag.content = content;
            ViewBag.LanguageID = langID;
            ViewBag.Pages = contentPages.ToList();

            if (TemplateType == TemplateType.ProjeListeleme)
            {
                ViewBag.contentPages = contentPages.Where(x=>x.ContentTypesId == content.ContentTypesId).ToList();
            }
            else if (TemplateType == TemplateType.BlogListeleme)
            {
                ViewBag.contentPages = contentPages.Where(x => x.ContentTypesId == content.ContentTypesId).ToList();
            }
            else if (TemplateType == TemplateType.UrunListeleme)
            {
                List<int> currContentList = contentPages.Where(x => x.ContentPageId == content.Id && x.IsDeleted == null).Select(x => x.Id).ToList();
               
                ViewBag.categories = contentPages.Where(x => x.ContentPageId == content.Parent.Id && x.IsDeleted == null).ToList();
                //ViewBag.contentPages = contentPages.Where(x => currContentList.Contains(x.ContentPageId ?? 0) && x.IsDeleted == null).OrderBy(o => o.ContentOrderNo).ToList();
                ViewBag.contentPages = contentPages.Where(x => x.ContentPageId == content.Id && x.IsDeleted == null).ToList();
            }
            return View(TemplateType.ToString());
        }
    }
}