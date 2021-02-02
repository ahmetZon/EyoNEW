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
    public class SectionView : ViewComponent
    {

        IHttpContextAccessor _httpContextAccessor;
        IContentPageService _IContentPageService;

        IDocumentsService _IDocumentsService;
        public SectionView(
            IContentPageService _IContentPageService,
            IDocumentsService _IDocumentsService,
        IHttpContextAccessor httpContextAccessor
            )
        {
            this._IContentPageService = _IContentPageService;
            this._httpContextAccessor = httpContextAccessor;
            this._IDocumentsService = _IDocumentsService;
        }
        public IViewComponentResult Invoke(string type, string viewType, string? link)
        {

            #region dynamicContent

            if (string.IsNullOrEmpty(link))
                link = HttpContext.Request.Path.Value.Trim().Trim('/').ToStr();

            List<ContentPage> contentPages = new List<ContentPage>();
            var content = _IContentPageService.Where(o => o.Link.ToLower() == link.ToLower() , true,false, o => o.Gallery, o => o.Documents, o => o.ThumbImage, o => o.Picture, o => o.BannerImage).Result.FirstOrDefault();
            int langID = 1;
            if (content != null && _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID") != content.LangId)
            {
                _httpContextAccessor.HttpContext.Session.SetInt32("LanguageID", content.LangId);
                langID = content.LangId;
            }

            contentPages = _IContentPageService.Where(x => x.LangId == langID, true, false, o => o.ContentPageChilds, o => o.Parent, o => o.Gallery, o => o.Documents, o => o.ThumbImage, o => o.Picture, o => o.BannerImage).Result.ToList();
            _httpContextAccessor.HttpContext.Session.Set("contentPages", contentPages);
            var document = _IDocumentsService.Where().Result.ToList();
            ViewBag.IsHeaderMenu = contentPages.Where(o => o.IsHeaderMenu == true).OrderBy(o => o.ContentOrderNo).ThenBy(o => o.Name).ToList();
            ViewBag.IsFooterMenu = contentPages.Where(o => o.IsFooterMenu == true).OrderBy(o => o.ContentOrderNo).ThenBy(o => o.Name).ToList();



            ViewBag.contentPages = contentPages;
            ViewBag.content = content;
            ViewBag.viewType = viewType;
            ViewBag.LanguageID = _httpContextAccessor.HttpContext.Session.GetInt32("LanguageID");
            if (content != null)
            {
                int parentID = content.ContentPageId ?? 0;
                if (parentID != 0)
                {
                    while (contentPages.Any(p => p.Id == content.ContentPageId && p.ContentPageId == null) && parentID != 0)
                    {
                        parentID = contentPages.FirstOrDefault(p => p.Id == content.ContentPageId && p.ContentPageId == null).ContentPageId ?? 0;
                    }
                    ViewBag.SideMenu = contentPages.Where(x => x.ContentPageId == parentID).ToList();
                }
                else
                {
                    ViewBag.SideMenu = content;
                }
            }
            #endregion
            return View(type);
        }

    }
}